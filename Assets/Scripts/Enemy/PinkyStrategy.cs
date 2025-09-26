using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GhostStrategies/PinkyStrategy")]
public class PinkyStrategy : GhostStratagySO
{
    private int numberOfCellsAhead = 4;

    public override Vector2Int GetTargetPosition(PlayerMovement player, Ghost self, List<Ghost> allGhost, Field field)
    {
        Vector2Int targetPosition = player.GetPlayerGridPosition();

        for (int i = 4; i >= 0; i--)
        {
            targetPosition = player.GetPlayerGridPosition() + player.GetPlayerDirection() * i;
            Cell cell = field.GetCellAtPosition(targetPosition);

            if (cell == null)
            {
                continue;    
            }
            
            if (cell.Type != CellType.Wall)
            {
                return targetPosition;
            }
        }

        return targetPosition;
    }
}
