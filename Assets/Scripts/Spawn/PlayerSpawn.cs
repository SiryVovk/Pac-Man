using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] private Field field;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Vector2Int spawnPosition;

    private void Start()
    {;
        Cell spawnCell = field.GetCellAtPosition(spawnPosition);

        if (spawnCell != null && spawnCell.Type == CellType.Empty)
        {
            Vector3 cellWorldPos = spawnCell.InWorldPosition;
            playerTransform.position = cellWorldPos;
            field.SetCellType(spawnCell.Position, CellType.Player);
        }
        else
        {
            Debug.LogError("Invalid spawn position for the player.");
        }
    }
}
