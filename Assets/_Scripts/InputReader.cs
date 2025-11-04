using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    public Vector2 MovementInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public event Action JumpEvent, ShootEvent, SwingEvent, AbilityEvent;
    //public event Action<Vector2> MoveEvent, LookEvent;
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
        if (context.performed)
            JumpEvent?.Invoke();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
        //MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
            Debug.Log("Shoot Event");
    }

    public void OnSwing(InputAction.CallbackContext context)
    {
        if (context.performed)
            Debug.Log("Swing Event");
    }

    public void OnAbility(InputAction.CallbackContext context)
    {
        if (context.performed)
            AbilityEvent?.Invoke();
    }
}