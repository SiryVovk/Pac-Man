using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerInputAction;

public class UIInputs : MonoBehaviour,IUIActions
{
    public static Action PauseAction;

    private PlayerInputAction inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputAction();
        inputActions.UI.SetCallbacks(this);
        inputActions.UI.Enable();
    }

    private void OnDisable()
    {
        inputActions.UI.Disable();
    }

    public void OnExit(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            PauseAction?.Invoke();
        }
    }
}
