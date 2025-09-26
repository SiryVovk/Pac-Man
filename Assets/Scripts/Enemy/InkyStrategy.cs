using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "GhostStrategies/InkyStrategy")]
public class InkyStrategy : GhostStratagySO
{
    public override Vector2Int GetTargetPosition(PlayerMovement player, Ghost self, List<Ghost> allGhost, Field field)
    {
        Ghost blinky = allGhost.FirstOrDefault(g => g.GetGhostStratagy() is BlinkyStrategy);

        if (blinky == null)
        {
            return player.GetPlayerGridPosition();
        }

        Vector2Int playerPositionAhead = player.GetPlayerGridPosition();
        for (int i = 2; i >= 0; i--)
        {
            playerPositionAhead = player.GetPlayerGridPosition() + player.GetPlayerDirection() * i;
            Cell cell = field.GetCellAtPosition(playerPositionAhead);

            if (cell == null)
            {
                continue;
            }
            
            if (cell.Type != CellType.Wall)
            {
                break;
            }
        }

        Vector2Int vector = playerPositionAhead - blinky.GetGhostPosition();

        return blinky.GetGhostPosition() + 2 * vector;
    }
}
