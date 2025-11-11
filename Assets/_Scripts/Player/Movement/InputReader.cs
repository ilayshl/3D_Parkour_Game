using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Reads input and translates it to values and events.
/// </summary>
public class InputReader : Controls.IPlayerActions
{
    public Vector2 MovementInput { get; private set; } //X, Y values
    public Vector2 LookInput { get; private set; }
    public event Action JumpEvent, ShootEvent, SwingEvent, AbilityEvent; //Button inputs
    public bool IsSwinging { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsUsingAbility { get; private set; }
    public event Action SwingCancelEvent;
    private Controls controls;

    public void Initiate()
    {
        controls = new Controls();
        controls.Player.SetCallbacks(this);

        controls.Player.Enable();
    }

    public void Disable()
    {
        controls.Player.Disable();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpEvent?.Invoke();
            IsJumping = true;
        }
        else if(context.canceled)
        {
            IsJumping = false;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ShootEvent?.Invoke();
            Debug.Log("Shoot Event");
        }
    }

    public void OnSwing(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SwingEvent?.Invoke();
            IsSwinging = true;
        }
        else if(context.canceled)
        {
            SwingCancelEvent?.Invoke();
            IsSwinging = false;
        }
            
    }

    public void OnAbility(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AbilityEvent?.Invoke();
            IsUsingAbility = true;
        }
        else if(context.canceled)
        {
            IsUsingAbility = false;
        }
    }
}