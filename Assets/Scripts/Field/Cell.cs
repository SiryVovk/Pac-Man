using UnityEngine;

public class Cell
{
    public Vector2Int Position { get; private set; }
    public Vector3 InWorldPosition { get;  private set; }
    public Vector3Int LocalPosition { get; private set; }
    public CellType Type { get; private set; }

    public Cell(Vector2Int position, Vector3 inWorldPosition,Vector3Int localPosition , CellType type)
    {
        Position = position;
        InWorldPosition = inWorldPosition;
        LocalPosition = localPosition;
        Type = type;
    }

    public void SetType(CellType type)
    {
        Type = type;
    }
}

public enum CellType
{
    Empty,
    Wall,
    Pallet,
    PowerPallet,
    Player,
    GhostExit
}