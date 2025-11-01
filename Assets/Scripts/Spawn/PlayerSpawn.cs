using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] private Field field;
    [SerializeField] private GameEndCondition gameEndCondition;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private Vector2Int spawnPosition;
    [SerializeField] private PowerModeManager powerModeManager;

    private Transform playerTransform;
    private PlayerMovement playerMovement;

    private bool initialized = false;

    private void Start()
    {
        if (GameSesion.Instance != null)
        {
            SaveData saveData = GameSesion.Instance.GetSaveData();

            SpawnPlayer(saveData);
        }
        else
        {
            SpawnPlayer();
        }
    }

    private void SpawnPlayer(SaveData saveData = null)
    {
        Cell spawnCell = field.GetCellAtPosition(spawnPosition);

        if (spawnCell != null)
        {
            Vector3 cellWorldPos = spawnCell.InWorldPosition;
            GameObject player = Instantiate(playerObject, cellWorldPos, Quaternion.identity);
            playerTransform = player.transform;

            playerMovement = player.GetComponent<PlayerMovement>();
            playerMovement.SetGridPosition(spawnPosition);
            playerMovement.Init(field, saveData);

            Health health = player.GetComponent<Health>();
            health.OnHealthChanged += RespawnPlayer;
            FindFirstObjectByType<HealthUI>().BindTo(health);
            gameEndCondition.InitHealthComponent(health);

            PlayerCollision playerCollision = player.GetComponent<PlayerCollision>();
            playerCollision.Init(powerModeManager);
        }
        else
        {
            Debug.LogError("Invalid spawn position for the player.");
        }
    }

    private void RespawnPlayer(int damage)
    {
        if(!initialized)
        {
            initialized = true;
            return;
        }

        field.SetCellType(playerMovement.GetPlayerGridPosition(), CellType.Empty);

        Cell cell = field.GetCellAtPosition(spawnPosition);
        field.SetCellType(spawnPosition, CellType.Player);
        Vector3 inWorldPosition = cell.InWorldPosition;
        playerTransform.position = inWorldPosition;
        playerMovement.SetGridPosition(spawnPosition);
        playerMovement.SetPlayerDirection(Vector2Int.zero);
    }
}
