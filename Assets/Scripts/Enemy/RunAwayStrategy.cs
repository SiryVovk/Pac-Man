using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GhostStrategies/RunAwayStrategy")]
public class RunAwayStrategy : GhostStratagySO
{
    public override Vector2Int GetTargetPosition(PlayerMovement player, Ghost self, List<Ghost> allGhost, Field field)
    {
        throw new System.NotImplementedException();
    }
}
