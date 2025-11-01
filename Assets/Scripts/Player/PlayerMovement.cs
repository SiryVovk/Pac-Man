using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Action<Vector2> OnDirectionChange;

    [SerializeField] private float duration = 0.5f;

    private PlayerInput playerInput;
    private Field field;
    private Health health;

    private Vector2Int direction;
    private Vector2Int nextDirection;
    private Vector2Int gridPosition;
    private Coroutine moveRoutin;

    private bool isMoving = false;
    private bool isInitialized = false;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        health = GetComponent<Health>();
        direction = Vector2Int.zero;
    }


    private void OnEnable()
    {
        playerInput.OnMoveUp += SetUpDirection;
        playerInput.OnMoveDown += SetDownDirection;
        playerInput.OnMoveLeft += SetLeftDirection;
        playerInput.OnMoveRight += SetRightDirection;
        health.OnHealthChanged += SetMovingToZero;
    }

    private void OnDisable()
    {
        playerInput.OnMoveUp -= SetUpDirection;
        playerInput.OnMoveDown -= SetDownDirection;
        playerInput.OnMoveLeft -= SetLeftDirection;
        playerInput.OnMoveRight -= SetRightDirection;
        health.OnHealthChanged -= SetMovingToZero;
    }

    private void Start()
    {
        GhostManager.Instance.RegistrPlayer(this);
    }

    public void Init(Field field, SaveData saveData = null)
    {
        this.field = field;
        LoadData(saveData);
        isInitialized = true;
    }

    private void LoadData(SaveData saveData)
    {
        if (moveRoutin != null)
        {
            StopCoroutine(moveRoutin);
            moveRoutin = null;
        }

        isMoving = true;
        if (saveData != null)
        {
            Vector2Int oldPosition = gridPosition;
            gridPosition = new Vector2Int(saveData.playerPosition[0], saveData.playerPosition[1]);
            field.OnPlayerEnterCell(gridPosition, oldPosition);
            Vector3 newPos = field.GetCellAtPosition(gridPosition).InWorldPosition;
            transform.position = newPos;

            Vector2Int loadedDir = new Vector2Int(saveData.playerDirection[0], saveData.playerDirection[1]);
            direction = loadedDir;
            nextDirection = loadedDir;

            OnDirectionChange?.Invoke(loadedDir);
        }

        StartCoroutine(UnlockMovementNextFrame());
    }

    private IEnumerator UnlockMovementNextFrame()
    {
        yield return null; // дочекайся одного кадру
        isMoving = false;
    }

    private void SetUpDirection() => SetDirection(Vector2Int.up);
    private void SetDownDirection() => SetDirection(Vector2Int.down);
    private void SetLeftDirection() => SetDirection(Vector2Int.left);
    private void SetRightDirection() => SetDirection(Vector2Int.right);

    private void Update()
    {
        if(!isInitialized || isMoving)
        {
            return;
        }

        Move();

    }

    private void SetDirection(Vector2Int newDir)
    {
        nextDirection = newDir;
    }

    private void Move()
    {
        moveRoutin = StartCoroutine(MoveCoroutine());
    }

    private IEnumerator MoveCoroutine()
    {
        isMoving = true;

        Vector2Int targetDir = GetValidDirection();
        Cell nextCell = field.GetCellAtPosition(gridPosition + targetDir);

        if (CanMoveTo(nextCell))
        {
            yield return MoveToCell(nextCell, targetDir);
        }

        yield return TryTeleport(targetDir);

        isMoving = false;
        moveRoutin = null;
    }

    private Vector2Int GetValidDirection()
    {
        if (field.GetCellAtPosition(gridPosition + nextDirection)?.Type != CellType.Wall)
        {
            OnDirectionChange?.Invoke(nextDirection);
            return nextDirection;
        }

        return direction;
    }
    
    private bool CanMoveTo(Cell nextCell)
    {
        return nextCell != null && nextCell.Type != CellType.Wall && nextCell.Type != CellType.GhostExit;
    }

    private IEnumerator MoveToCell(Cell nextCell, Vector2Int targetDir)
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = nextCell.InWorldPosition;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
        Vector2Int oldPosition = gridPosition;
        gridPosition += targetDir;
        field.OnPlayerEnterCell(gridPosition, oldPosition);

        direction = targetDir;
    }

    private IEnumerator TryTeleport(Vector2Int targetDir)
    {
        var teleportDestination = field.GetTeleportDestination(gridPosition);

        if (!teleportDestination.HasValue)
        {
            yield break;
        }
        
        Vector3 offscreenOffset = (Vector2)targetDir * 0.5f;
        Vector3 startOffscreen = transform.position + offscreenOffset;

        float teleportHalfDuration = duration / 2;

        float time = 0;
        while (time < teleportHalfDuration)
        {
            transform.position = Vector3.Lerp(transform.position, startOffscreen, time / 0.2f);
            time += Time.deltaTime;
            yield return null;
        }

        Vector2Int beforTeleportPosition = gridPosition;
        gridPosition = teleportDestination.Value;
        field.OnPlayerEnterCell(gridPosition, beforTeleportPosition);
        Vector3 newPos = field.GetCellAtPosition(gridPosition).InWorldPosition - offscreenOffset;
        transform.position = newPos;

        time = 0;
        while (time < teleportHalfDuration)
        {
            transform.position = Vector3.Lerp(transform.position, field.GetCellAtPosition(gridPosition).InWorldPosition, time / 0.2f);
            time += Time.deltaTime;
            yield return null;
        }
    }

    public void SetGridPosition(Vector2Int newPosition)
    {
        gridPosition = newPosition;
    }

    public Vector2Int GetPlayerGridPosition()
    {
        return gridPosition;
    }

    private void SetMovingToZero(int damage)
    {
        if (moveRoutin != null)
        {
            StopCoroutine(moveRoutin);
            moveRoutin = null;
            isMoving = false;
        }

        direction = Vector2Int.zero;
        nextDirection = Vector2Int.zero;

        OnDirectionChange?.Invoke(Vector2Int.zero);
    }

    public Vector2Int GetPlayerDirection()
    {
        return direction;
    }

    public void SetPlayerDirection(Vector2Int newDirection)
    {
        direction = newDirection;
    }
}
