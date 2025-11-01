using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Field : MonoBehaviour
{
    public Action<Cell> OnPlayerCellEnter;
    public Action<Cell> OnGameDataLoadCell;

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase[] wallTile;
    [SerializeField] private TileBase[] palletTile;
    [SerializeField] private TileBase[] powerPalletTile;
    [SerializeField] private TileBase[] exitTile;
    [SerializeField] private List<PortalPair> portals;

    private Cell[,] cells;

    private void Awake()
    {
        InitializeField();

        if(GameSesion.Instance != null)
        {
            LoadFieldFromSave(GameSesion.Instance.GetSaveData());
        }
    }

    private void InitializeField()
    {
        tilemap.CompressBounds();
        BoundsInt bounds = tilemap.cellBounds;
        cells = new Cell[bounds.size.x, bounds.size.y];

        foreach (var pos in bounds.allPositionsWithin)
        {
            Vector3Int localPos = new Vector3Int(pos.x, pos.y, 0);
            TileBase tile = tilemap.GetTile(localPos);
            Vector2Int cellPos = new Vector2Int(pos.x - bounds.xMin, pos.y - bounds.yMin);
            Vector3 worldPos = tilemap.GetCellCenterWorld(localPos);
            bool isTeleportCell = IsTeleportCell(cellPos);

            if (IsNededTile(tile, wallTile))
            {
                cells[cellPos.x, cellPos.y] = new Cell(cellPos, worldPos, localPos, CellType.Wall,isTeleportCell);
            }
            else if (IsNededTile(tile, palletTile))
            {
                cells[cellPos.x, cellPos.y] = new Cell(cellPos, worldPos, localPos, CellType.Pallet,isTeleportCell);
            }
            else if (IsNededTile(tile, powerPalletTile))
            {
                cells[cellPos.x, cellPos.y] = new Cell(cellPos, worldPos, localPos, CellType.PowerPallet,isTeleportCell);
            }
            else if (IsNededTile(tile, exitTile))
            {
                cells[cellPos.x, cellPos.y] = new Cell(cellPos, worldPos, localPos, CellType.GhostExit,isTeleportCell);
            }
            else
            {
                cells[cellPos.x, cellPos.y] = new Cell(cellPos, worldPos, localPos, CellType.Empty,isTeleportCell);
            }
        }
    }

    private bool IsTeleportCell(Vector2Int cellPos)
    {
        foreach (var portal in portals)
        {
            if (portal.from == cellPos || portal.to == cellPos)
            {
                return true;
            }
        }
        return false;
    }

    private bool IsNededTile(TileBase tile, TileBase[] neededTiles)
    {
        foreach (var neededTile in neededTiles)
        {
            if (tile == neededTile)
            {
                return true;
            }
        }
        return false;
    }

    public Cell GetCellAtPosition(Vector2Int position)
    {
        if (position.x < 0 || position.x >= cells.GetLength(0) || position.y < 0 || position.y >= cells.GetLength(1))
        {
            return null;
        }
        return cells[position.x, position.y];
    }

    public void SetCellType(Vector2Int position, CellType type)
    {
        Cell cell = GetCellAtPosition(position);
        if (cell != null)
        {
            cell.SetType(type);
        }
    }

    private void LoadFieldFromSave(SaveData data)
    {
        for(int index = 0; index < data.isEmpty.Length; index++)
        {
            bool cellEmpty = data.isEmpty[index];
            if (cellEmpty)
            {
                Vector2Int position = new Vector2Int(index % Width, index / Width);
                CellType type = CellType.Empty;
                SetCellType(position, type);
                OnGameDataLoadCell?.Invoke(GetCellAtPosition(position));
            }
        }
    }
    
    public void OnPlayerEnterCell(Vector2Int position, Vector2Int previousPosition)
    {
        Cell cell = GetCellAtPosition(position);

        if (cell == null)
        {
            return;
        }

        OnPlayerCellEnter?.Invoke(cell);

        SetCellType(cell.Position, CellType.Player);
        SetCellType(previousPosition, CellType.Empty);
    }

    public int CountCellsOfType(CellType cellType)
    {
        int totalCells = 0;
        foreach (Cell cell in cells)
        {
            if (cell.Type == cellType)
            {
                totalCells++;
            }
        }

        return totalCells;
    }

    public void ChangeTile(Vector3Int localPositionOfCell)
    {
        tilemap.SetTile(localPositionOfCell, null);
    }

    public Vector2Int? GetTeleportDestination(Vector2Int teleportPos)
    {
        foreach (PortalPair portal in portals)
        {
            if (portal.from == teleportPos)
            {
                return portal.to;
            }
            if (portal.to == teleportPos)
            {
                return portal.from;
            }
        }

        return null;
    }

    public int Width => cells.GetLength(0);
    public int Height => cells.GetLength(1);
}
