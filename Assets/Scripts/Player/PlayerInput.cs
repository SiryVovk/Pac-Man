using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerInputAction;

public class PlayerInput : MonoBehaviour, IPlayerActions
{
    public event Action OnMoveUp;
    public event Action OnMoveDown;
    public event Action OnMoveLeft;
    public event Action OnMoveRight;

    private PlayerInputAction inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputAction();
        inputActions.Player.SetCallbacks(this);
        inputActions.Player.Enable();
    }
    private void OnDestroy()
    {
        inputActions.Player.Disable();
    }

    public void OnDown(InputAction.CallbackContext context)
    {
        OnMoveDown?.Invoke();
    }

    public void OnLeft(InputAction.CallbackContext context)
    {
        OnMoveLeft?.Invoke();
    }

    public void OnRight(InputAction.CallbackContext context)
    {
        OnMoveRight?.Invoke();
    }

    public void OnUp(InputAction.CallbackContext context)
    {
        OnMoveUp?.Invoke();
    }
}
