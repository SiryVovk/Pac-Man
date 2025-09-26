using System.Collections.Generic;
using UnityEngine;

public abstract class GhostStratagySO : ScriptableObject
{
    public abstract Vector2Int GetTargetPosition(PlayerMovement player, Ghost self, List<Ghost> allGhost, Field field);
}
