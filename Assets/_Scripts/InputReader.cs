using System;
using UnityEngine;
using UnityEngine.InputSystem;

//Reads input and translates it to values and events.
public class InputReader : Controls.IPlayerActions
{
    public Vector2 MovementInput { get; private set; } //X, Y values
    public Vector2 LookInput { get; private set; }
    public event Action JumpEvent, ShootEvent, SwingEvent, AbilityEvent; //Button inputs
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
            Debug.Log("Swing Event");
        }
            
    }

    public void OnAbility(InputAction.CallbackContext context)
    {
        if (context.started)
            AbilityEvent?.Invoke();
    }
}