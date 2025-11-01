
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkaterTarget
{
    [SerializeField] private List<Vector2Int> points;

    public Queue<Vector2Int> GetPointsQueue()
    {
        return new Queue<Vector2Int>(points);
    }
}
