using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GhostStrategies/ClydeStrategy")]
public class ClydeStrategy : GhostStratagySO
{
    public override Vector2Int GetTargetPosition(PlayerMovement player, Ghost self, List<Ghost> allGhost, Field field)
    {
        float distanse = Vector2Int.Distance(self.GetGhostPosition(), player.GetPlayerGridPosition());

        if (distanse > 8)
        {
            return player.GetPlayerGridPosition();
        }
        else
        {
            return player.GetPlayerGridPosition();
        }
    }
}
