using UnityEngine;
using UnityEngine.Tilemaps;

public class Field : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase[] wallTile;
    [SerializeField] private TileBase[] palletTile;
    [SerializeField] private TileBase[] powerPalletTile;


    private Cell[,] cells;

    private void Awake()
    {
        InitializeField();
        DebugPrintField();
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
                cells[cellPos.x, cellPos.y] = new Cell(cellPos, worldPos, CellType.Wall);
            }
            else if (IsNededTile(tile, palletTile))
            {
                cells[cellPos.x, cellPos.y] = new Cell(cellPos, worldPos, CellType.Pallet);
            }
            else if (IsNededTile(tile, powerPalletTile))
            {
                cells[cellPos.x, cellPos.y] = new Cell(cellPos, worldPos, CellType.PowerPallet);
            }
            else
            {
                cells[cellPos.x, cellPos.y] = new Cell(cellPos, worldPos, CellType.Empty);
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

    public void DebugPrintField()
    {
        if (cells == null) return;

        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        // Верхній рядок з індексами X
        sb.Append("   ");
        for (int x = 0; x < cells.GetLength(0); x++)
            sb.Append($"{x,2} ");
        sb.AppendLine();

        for (int y = cells.GetLength(1) - 1; y >= 0; y--)
        {
            sb.Append($"{y,2} "); // Індекс Y зліва
            for (int x = 0; x < cells.GetLength(0); x++)
            {
                char c = '.';
                if (cells[x, y] != null)
                {
                    switch (cells[x, y].Type)
                    {
                        case CellType.Wall: c = '#'; break;
                        case CellType.Pallet: c = '.'; break;
                        case CellType.PowerPallet: c = 'O'; break;
                        case CellType.Empty: c = ' '; break;
                    }
                }
                sb.Append($" {c} ");
            }
            sb.AppendLine();
        }

        Debug.Log(sb.ToString());
    }
}
