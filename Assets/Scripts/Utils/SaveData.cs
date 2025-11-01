using System;
using UnityEngine;

[Serializable]
public class SaveData
{
    public int sceneIndex;
    public bool[] isEmpty;
    public int[] playerPosition;
    public int[] playerDirection;
    public float score;
    public int lives;
    public int[,] ghostPositions;
    public int[,] ghostState;
    public int[,] isGhostDead;
    public float[,] ghostRespawnTime;
    public bool powerModeActive;
    public float powerModeTimeLeft;


    public SaveData(int sceneIndex, Field field, PlayerMovement player, Score score, Health health, GhostManager ghostManager, PowerModeManager powerModeManager)
    {
        this.sceneIndex = sceneIndex;

        isEmpty = new bool[field.Width * field.Height];

        for (int x = 0; x < field.Width; x++)
        {
            for (int y = 0; y < field.Height; y++)
            {
                Cell cell = field.GetCellAtPosition(new Vector2Int(x, y));
                isEmpty[x + y * field.Width] = cell.Type == CellType.Empty;
            }
        }

        SavePlayerData(player, health);

        this.score = score.GetScore();

        SaveGhostData(ghostManager);
        powerModeActive = powerModeManager.IsPowerModeActive();
        powerModeTimeLeft = powerModeManager.GetPowerModeTimeLeft();
    }

    private void SavePlayerData(PlayerMovement player, Health health)
    {
        Vector2Int position = player.GetPlayerGridPosition();
        playerPosition = new int[] { position.x, position.y };

        Vector2Int direction = player.GetPlayerDirection();
        playerDirection = new int[] { direction.x, direction.y };

        lives = health.GetCurrentHealth();
    }

    private void SaveGhostData(GhostManager ghostManager)
    {
        var ghosts = ghostManager.GetAllGhosts();
        ghostPositions = new int[ghosts.Count, 2];
        ghostState = new int[ghosts.Count, 1];
        isGhostDead = new int[ghosts.Count, 1];
        ghostRespawnTime = new float[ghosts.Count, 1];

        for (int i = 0; i < ghosts.Count; i++)
        {
            Vector2Int pos = ghosts[i].GetGhostPosition();
            ghostPositions[i, 0] = pos.x;
            ghostPositions[i, 1] = pos.y;

            ghostState[i, 0] = (int)ghosts[i].GetGhostState();
            isGhostDead[i, 0] = ghosts[i].IsDead() ? 1 : 0;
            ghostRespawnTime[i, 0] = ghosts[i].GetRespawnTimeLeft();
        }
    }
}
