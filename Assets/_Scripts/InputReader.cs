using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    public Vector2 MovementInput { get; private set; }
    public event Action JumpEvent, MoveEvent, ShootEvent, SwingEvent, AbilityEvent;
    private Controls controls;

    private void Start()
    {
        controls = new Controls();
        controls.Player.SetCallbacks(this);

        controls.Player.Enable();
    }

    private void OnDestroy()
    {
        controls.Player.Disable();
    }
    
    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.performed)
        JumpEvent?.Invoke();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnSwing(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnAbility(InputAction.CallbackContext context)
    {
        if(context.performed)
        AbilityEvent?.Invoke();
    }
}