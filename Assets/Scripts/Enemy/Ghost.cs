using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] private float moveDuration = 0.7f;

    private Field field;
    private GhostStratagySO ghostStratagy;
    private Vector2Int ghostPosition;
    private Vector2Int previousDirection;
    private Vector2Int exitTarget;

    private bool isMoving = false;
    private bool isGoingToExit = true;

    public void Init(Vector2Int startPosition, Field field, GhostStratagySO startingStratagy, Vector2Int exitTarget)
    {
        this.field = field;
        ghostPosition = startPosition;
        ghostStratagy = startingStratagy;
        this.exitTarget = exitTarget;
    }

    private void Start()
    {
        GhostManager.Instance.RegisterGhost(this);
    }

    private void Update()
    {
        if (isGoingToExit && !isMoving)
        {
            MoveToExit();
        }
        else if (!isMoving && ghostStratagy != null)
        {
            Vector2Int nextMove = ChooseNextDirection(ghostStratagy.GetTargetPosition(GhostManager.Instance.PlayerMovement, this, GhostManager.Instance.GhostsList));
            StartCoroutine(MoveRoutine(nextMove));
        }
    }

    private Vector2Int ChooseNextDirection(Vector2Int targetPosition)
    {
        Vector2Int target = targetPosition;

        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        Vector2Int bestMove = ghostPosition;
        Vector2Int expectedPreviousDirection = Vector2Int.zero;
        float bestDistance = float.MaxValue;

        foreach (Vector2Int direction in directions)
        {
            if (direction == -previousDirection) continue;

            Vector2Int nextPos = ghostPosition + direction;
            Cell nextCell = field.GetCellAtPosition(nextPos);

            if (nextCell != null && nextCell.Type != CellType.Wall)
                {
                    float distance = Vector2Int.Distance(nextPos, target);
                    if (distance < bestDistance)
                    {
                        bestDistance = distance;
                        bestMove = nextPos;
                        expectedPreviousDirection = direction;
                    }
                }
        }

        previousDirection = expectedPreviousDirection;

        return bestMove;
    }

    private void MoveToExit()
    {
        if (ghostPosition == exitTarget)
        {
            isGoingToExit = false;
            return;
        }

        Vector2Int move = ChooseNextDirection(exitTarget);
        StartCoroutine(MoveRoutine(move));
    }

    private bool isGhostAhead(List<Ghost> ghostList, Vector2Int nextPos)
    {
        foreach (Ghost ghost in ghostList)
        {
            if (ghost.ghostPosition == nextPos)
            {
                return true;
            }
        }

        return false;
    }

    private IEnumerator MoveRoutine(Vector2Int move)
    {
        if (!GhostManager.Instance.TryReserveCell(this, move))
        {
            isMoving = false;
            yield break; // не можна йти туди
        }
        isMoving = true;

        Cell nextCell = field.GetCellAtPosition(move);

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = nextCell.InWorldPosition;

        float elapsTime = 0f;
        while (elapsTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsTime / moveDuration);
            elapsTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        ghostPosition = nextCell.Position;
        GhostManager.Instance.ReleaseCell(move);
        isMoving = false;
    }
}
