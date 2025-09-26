using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] private Field field;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private Vector2Int spawnPosition;

    private Transform playerTransform;
    private PlayerMovement playerMovement;

    private void Start()
    {
        Cell spawnCell = field.GetCellAtPosition(spawnPosition);

        if (spawnCell != null && spawnCell.Type == CellType.Empty)
        {
            Vector3 cellWorldPos = spawnCell.InWorldPosition;
            GameObject player = Instantiate(playerObject, cellWorldPos, Quaternion.identity);
            playerTransform = player.transform;

            playerMovement = player.GetComponent<PlayerMovement>();
            playerMovement.Init(field);
            playerMovement.SetGridPosition(spawnPosition);

            Health health = player.GetComponent<Health>();
            health.OnHealthChanged += GetPlayerToSpawn;
            FindFirstObjectByType<HealthUI>().BindTo(health);

            field.SetCellType(spawnCell.Position, CellType.Player);
        }
        else
        {
            Debug.LogError("Invalid spawn position for the player.");
        }
    }

    private void GetPlayerToSpawn(int damage)
    {
        field.SetCellType(playerMovement.GetPlayerGridPosition(), CellType.Empty);

        Cell cell = field.GetCellAtPosition(spawnPosition);
        field.SetCellType(spawnPosition, CellType.Player);
        Vector3 inWorldPosition = cell.InWorldPosition;
        playerTransform.position = inWorldPosition;
        playerMovement.SetGridPosition(spawnPosition);
    }
}
