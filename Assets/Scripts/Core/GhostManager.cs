using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    public static GhostManager Instance { get; private set; }
    public PlayerMovement PlayerMovement { get; private set; }
    public List<Ghost> GhostsList { get; private set; } = new List<Ghost>();
    private HashSet<Vector2Int> reservedCells = new HashSet<Vector2Int>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void RegisterGhost(Ghost ghost)
    {
        if (!GhostsList.Contains(ghost))
        {
            GhostsList.Add(ghost);
        }
    }

    public void RegistrPlayer(PlayerMovement playerMovement)
    {
        PlayerMovement = playerMovement;
    }

    public bool TryReserveCell(Ghost ghost, Vector2Int pos)
    {
        if (reservedCells.Contains(pos))
            return false;

        reservedCells.Add(pos);
        return true;
    }

    public void ReleaseCell(Vector2Int pos)
    {
        reservedCells.Remove(pos);
    }
}

