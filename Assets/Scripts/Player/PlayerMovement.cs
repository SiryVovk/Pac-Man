using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput playerInput;

    private Vector2 direction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        direction = Vector2.zero;
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

    private void SetRightDirection()
    {
        direction = Vector2.right;
        DebugLogDirection();
    }

    private void SetLeftDirection()
    {
        direction = Vector2.left;
        DebugLogDirection();
    }

    private void SetUpDirection()
    {
        direction = Vector2.up;
        DebugLogDirection();
    }

    private void SetDownDirection()
    {
        direction = Vector2.down;
        DebugLogDirection();
    }

    private void DebugLogDirection()
    {
        Debug.Log("Direction: " + direction);
    }
}
