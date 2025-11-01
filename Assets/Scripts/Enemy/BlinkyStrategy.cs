using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GhostStrategies/BlinkyStrategy")]
public class BlinkyStrategy : GhostStratagySO
{
    public override Vector2Int GetTargetPosition(PlayerMovement player, Ghost self, List<Ghost> allGhost, Field field, Queue<Vector2Int> scaterTargets, GhostState ghostStat)
    {
        switch (ghostStat)
        {
            case GhostState.Scatter:
                return ScatterTarget(self, field, scaterTargets);
            case GhostState.Chase:
                return ChaseTarget(player, self, allGhost, field);
            case GhostState.Frightened:
                return FrightenedTarget(player, self, field);
            default:
                return ScatterTarget(self, field, scaterTargets);
        }
    }

    public override Vector2Int ScatterTarget(Ghost self, Field field, Queue<Vector2Int> scaterTargets)
    {
        Vector2Int ghostPosition = scaterTargets.Dequeue();
        scaterTargets.Enqueue(ghostPosition);
        return ghostPosition;
    }
    
    public override Vector2Int ChaseTarget(PlayerMovement player, Ghost self, List<Ghost> allGhost, Field field)
    {
        return player.GetPlayerGridPosition();
    }

    public override Vector2Int FrightenedTarget(PlayerMovement player, Ghost self, Field field)
    {
        Vector2Int ghostPosition = self.GetGhostPosition();
        Vector2Int previousDirection = self.GetPreviousDirection();

        List<Vector2Int> posibleDirections = new List<Vector2Int>
        {
            Vector2Int.up, Vector2Int.down , Vector2Int.left, Vector2Int.right
        };

        Vector2Int oppositDirection = -previousDirection;
        posibleDirections.Remove(oppositDirection);

        List<Vector2Int> validDirections = new List<Vector2Int>();

        foreach (Vector2Int direction in posibleDirections)
        {
            Cell cellWalkIn = field.GetCellAtPosition(ghostPosition + direction);
            if (cellWalkIn.Type != CellType.GhostExit || cellWalkIn.Type != CellType.Wall)
            {
                validDirections.Add(direction);
            }
        }

        if (validDirections.Count == 0)
        {
            validDirections.Add(oppositDirection);
        }
        
        Vector2Int chosenDir = validDirections[Random.Range(0, validDirections.Count)];

        return ghostPosition + chosenDir;
    }

}
