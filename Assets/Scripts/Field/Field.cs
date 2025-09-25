using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Field : MonoBehaviour
{
    public Action<Cell> OnPlayerCellEnter;

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase[] wallTile;
    [SerializeField] private TileBase[] palletTile;
    [SerializeField] private TileBase[] powerPalletTile;
    [SerializeField] private TileBase[] exitTile;


    private Cell[,] cells;

    private void Awake()
    {
        InitializeField();
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

            if (IsNededTile(tile, wallTile))
            {
                cells[cellPos.x, cellPos.y] = new Cell(cellPos, worldPos, localPos, CellType.Wall);
            }
            else if (IsNededTile(tile, palletTile))
            {
                cells[cellPos.x, cellPos.y] = new Cell(cellPos, worldPos, localPos, CellType.Pallet);
            }
            else if (IsNededTile(tile, powerPalletTile))
            {
                cells[cellPos.x, cellPos.y] = new Cell(cellPos, worldPos, localPos, CellType.PowerPallet);
            }
            else if (IsNededTile(tile, exitTile))
            {
                cells[cellPos.x,cellPos.y] = new Cell(cellPos, worldPos, localPos, CellType.GhostExit);
            }
            else
            {
                cells[cellPos.x, cellPos.y] = new Cell(cellPos, worldPos, localPos, CellType.Empty);
            }
        }
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
}
