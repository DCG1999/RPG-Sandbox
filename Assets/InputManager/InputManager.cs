using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[System.Serializable]
public class MoveEvent : UnityEvent<float, float> { }

[System.Serializable]
public class RunEvent : UnityEvent<bool> { }

public sealed class InputManager : MonoBehaviour
{
    private PlayerControls playerControls;
    public MoveEvent moveEvent;
    public RunEvent runEvent;

    private void Awake()
    {
        playerControls.Player.Enable();
        playerControls.Player.Move.performed += OnMove;
        playerControls.Player.Move.canceled += OnMove;
        playerControls.Player.Run.performed += OnRun;
        playerControls.Player.Run.performed += OnRun;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        moveEvent.Invoke(moveInput.x, moveInput.y);
    }

    private void OnRun(InputAction.CallbackContext context)
    {
        bool runInput = context.ReadValueAsButton();
        runEvent.Invoke(runInput);
    }
}
