using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GhostStrategies/BlinkyStrategy")]
public class BlinkyStrategy : GhostStratagySO
{
    public override Vector2Int GetTargetPosition(PlayerMovement player, Ghost self, List<Ghost> allGhost, Field field)
    {
        return player.GetPlayerGridPosition();
    }
}
