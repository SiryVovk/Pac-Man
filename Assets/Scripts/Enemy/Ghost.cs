using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] private float moveDuration = 0.5f;
    [SerializeField] private float respawnTime = 5f;
    [SerializeField] private float changeStateTime = 10f;

    private Field field;
    private GhostSprite ghostSprite;
    private GhostStratagySO ghostStratagy;
    private Vector2Int ghostSpawnPosition;
    private Vector2Int ghostPosition;
    private Vector2Int previousDirection;
    private Vector2Int nextMove;
    private Vector2Int exitTarget;
    private Queue<Vector2Int> scaterTargets = new Queue<Vector2Int>();
    private PowerModeManager powerModeManager;

    private GhostState ghostState;

    private bool isMoving = false;
    private bool isGoingToExit = true;
    private bool isFrozen = false;
    private bool isInitialized = false;

    private float respawnTimeLeft = 0f;

    private Coroutine stateChangeRoutine;
    private Coroutine moveRoutine;

    public void Init(Vector2Int startPosition, Field field, GhostStratagySO startingStratagy, Vector2Int exitTarget, Queue<Vector2Int> scaterTargets, PowerModeManager powerModeManager, Vector2Int ghostSpawnPosition = default)
    {
        this.field = field;
        ghostPosition = startPosition;
        ghostStratagy = startingStratagy;
        this.exitTarget = exitTarget;
        this.scaterTargets = scaterTargets;
        this.powerModeManager = powerModeManager;
        this.ghostSpawnPosition = ghostSpawnPosition;

        AfterInitSubscription();
    }

    private void AfterInitSubscription()
    {
        powerModeManager.OnPowerUpModStarted += HandlePowerUpStart;
        powerModeManager.OnPowerUpModEnded += HandlePowerUpEnd;
    }

    private void Start()
    {
        GhostManager.Instance.RegisterGhost(this);
        ghostSprite = GetComponent<GhostSprite>();

        if(GameSesion.Instance == null)
        {
            isInitialized = true;
        }
    }

    public void LoadSaveData(Vector2Int savedPosition, int savedState, bool isDead, float respawnTimeLeft)
    {

        StopAllCoroutines();
        isFrozen = isDead;

        if (isFrozen)
        {
            ghostPosition = ghostSpawnPosition;
            transform.position = field.GetCellAtPosition(ghostSpawnPosition).InWorldPosition;
            ghostSprite.SetGhostVisibility(false);
            StartCoroutine(RespawnRoutine(respawnTime));
        }
        else
        {
            ghostPosition = savedPosition;
            isGoingToExit = false;
            ghostState = (GhostState)savedState;
            stateChangeRoutine = StartCoroutine(StateChangerRoutin());
            transform.position = field.GetCellAtPosition(savedPosition).InWorldPosition;
        }

        StartCoroutine(WaitForInitialization());
    }

    
    private IEnumerator WaitForInitialization()
    {
        yield return null;

        isInitialized = true;
    }

    private void Update()
    {
        if (isFrozen || !isInitialized)
        {
            return;
        }
        
        if (isGoingToExit && !isMoving)
        {
            MoveToExit();
        }
        else if ( !isMoving && ghostStratagy != null)
        {
            nextMove = ChooseNextDirection(ghostStratagy.GetTargetPosition(GhostManager.Instance.PlayerMovement, this, GhostManager.Instance.GhostsList, field, scaterTargets, ghostState));
            moveRoutine = StartCoroutine(MoveRoutine(nextMove));
        }

        if(stateChangeRoutine == null)
        {
            stateChangeRoutine = StartCoroutine(StateChangerRoutin());
        }
    }

    private void HandlePowerUpStart()
    {
        if (stateChangeRoutine != null)
        {
            StopCoroutine(stateChangeRoutine);
        }

        stateChangeRoutine = StartCoroutine(FrightenedRoutine());
    }

    private void HandlePowerUpEnd()
    {
        StopCoroutine(stateChangeRoutine);
        stateChangeRoutine = null;

        ghostState = GhostState.Scatter;
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

            if (nextCell == null)
            {
                continue;
            }

            bool isWall = nextCell.Type == CellType.Wall;
            bool isExit = nextCell.Type == CellType.GhostExit;
            bool isTeleport = nextCell.isTeleport;
            bool canMoveWhenExiting = isGoingToExit && !isWall;
            bool canMoveNormally = !isGoingToExit && !isWall && !isExit;

            if (!(canMoveWhenExiting || canMoveNormally) || isTeleport)
            {
                continue;
            }

            float distance = Vector2Int.Distance(nextPos, target);
            if (distance < bestDistance)
            {
                bestDistance = distance;
                bestMove = nextPos;
                expectedPreviousDirection = direction;
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

        nextMove = ChooseNextDirection(exitTarget);
        StartCoroutine(MoveRoutine(nextMove));
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
        moveRoutine = null;
    }

    public GhostStratagySO GetGhostStratagy()
    {
        return ghostStratagy;
    }

    public Vector2Int GetGhostPosition()
    {
        return ghostPosition;
    }

    public Vector2Int GetPreviousDirection()
    {
        return previousDirection;
    }

    public GhostState GetGhostState()
    {
        return ghostState;
    }
    
    private IEnumerator StateChangerRoutin()
    {
        yield return new WaitForSeconds(changeStateTime);

        if (ghostState == GhostState.Scatter)
        {
            ghostState = GhostState.Chase;
        }
        else
        {
            ghostState = GhostState.Scatter;
        }

        stateChangeRoutine = null;
    }

    private IEnumerator FrightenedRoutine()
    {
        ghostState = GhostState.Frightened;

        while(true)
        {
            yield return null;
        }
    }

    public void EatenByPlayer()
    {
        GhostManager.Instance.GhostEaten();

        if (moveRoutine != null)
        {
            StopCoroutine(moveRoutine);
            moveRoutine = null;
        }
        isMoving = false;
        GhostManager.Instance.ReleaseCell(nextMove);

        isFrozen = true;
        ghostPosition = ghostSpawnPosition;
        transform.position = field.GetCellAtPosition(ghostSpawnPosition).InWorldPosition;

        ghostSprite.SetGhostVisibility(false);
        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        respawnTimeLeft = respawnTime;
        while (respawnTimeLeft > 0f)
        {
            respawnTimeLeft -= Time.deltaTime;
            yield return null;
        }

        isFrozen = false;
        isGoingToExit = true;
        ghostSprite.SetGhostVisibility(true);
    }

    private IEnumerator RespawnRoutine(float savedRespawnTimeLeft)
    {
        respawnTimeLeft = savedRespawnTimeLeft;
        while (respawnTimeLeft > 0f)
        {
            respawnTimeLeft -= Time.deltaTime;
            yield return null;
        }

        isFrozen = false;
        isGoingToExit = true;
        ghostSprite.SetGhostVisibility(true);
    }

    private void OnDestroy()
    {
        powerModeManager.OnPowerUpModStarted -= HandlePowerUpStart;
        powerModeManager.OnPowerUpModEnded -= HandlePowerUpEnd;
    }

    public bool IsDead()
    {
        return isFrozen;
    }

    public float GetRespawnTimeLeft()
    {
        return respawnTimeLeft;
    }
}

