using System.Collections.Generic;
using UnityEngine;

public abstract class GhostStratagySO : ScriptableObject
{
    public abstract Vector2Int GetTargetPosition(PlayerMovement player, Ghost self, List<Ghost> allGhost, Field field, Queue<Vector2Int> scaterTargets, GhostState ghostStat);

    public abstract Vector2Int ScatterTarget(Ghost self, Field field, Queue<Vector2Int> scaterTargets);

    public abstract Vector2Int ChaseTarget(PlayerMovement player, Ghost self, List<Ghost> allGhost, Field field);

    public abstract Vector2Int FrightenedTarget(PlayerMovement player, Ghost self, Field field);
}
