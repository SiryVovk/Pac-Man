using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Action<Vector2> OnDirectionChange;

    [SerializeField] private float duration = 0.5f;

    private PlayerInput playerInput;
    private Field field;

    private Vector2Int direction;
    private Vector2Int nextDirection;
    private Vector2Int gridPosition;

    private bool isMoving = false;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        direction = Vector2Int.zero;
    }


    private void OnEnable()
    {
        playerInput.OnMoveUp += SetUpDirection;
        playerInput.OnMoveDown += SetDownDirection;
        playerInput.OnMoveLeft += SetLeftDirection;
        playerInput.OnMoveRight += SetRightDirection;
    }

    private void OnDisable()
    {
        playerInput.OnMoveUp -= SetUpDirection;
        playerInput.OnMoveDown -= SetDownDirection;
        playerInput.OnMoveLeft -= SetLeftDirection;
        playerInput.OnMoveRight -= SetRightDirection;
    }

    private void Start()
    {
        GhostManager.Instance.RegistrPlayer(this);
        SetDirection(Vector2Int.up);
    }

    public void Init(Field field)
    {
        this.field = field;
    }

    private void SetUpDirection() => SetDirection(Vector2Int.up);
    private void SetDownDirection() => SetDirection(Vector2Int.down);
    private void SetLeftDirection() => SetDirection(Vector2Int.left);
    private void SetRightDirection() => SetDirection(Vector2Int.right);

    private void Update()
    {
        if (!isMoving)
        {
            Move();
        }
    }

    private void SetDirection(Vector2Int newDir)
    {
        nextDirection = newDir;
    }

    private void Move()
    {
        StartCoroutine(MoveCoroutine());
    }

    private IEnumerator MoveCoroutine()
    {
        isMoving = true;

        Vector2Int targetDir = direction;


        if (field.GetCellAtPosition(gridPosition + nextDirection)?.Type != CellType.Wall)
        {
            targetDir = nextDirection;
            OnDirectionChange?.Invoke(targetDir);
        }

        Cell nextCell = field.GetCellAtPosition(gridPosition + targetDir);

        if (nextCell != null && nextCell.Type != CellType.Wall && nextCell.Type != CellType.GhostExit)
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
            gridPosition += targetDir;
            field.OnPlayerEnterCell(gridPosition, gridPosition - targetDir);

            direction = targetDir;
        }

        isMoving = false;
    }

    public void SetGridPosition(Vector2Int newPosition)
    {
        gridPosition = newPosition;
    }

    public Vector2Int GetPlayerGridPosition()
    {
        return gridPosition;
    }
}
