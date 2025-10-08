using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IMovementManager
{
    public event Action<MovementState> OnMovementStateChanged;
    public MovementState State { get => state; }
    private MovementState state;

    public void ChangeMovementState(MovementState newState = MovementState.Airborne)
    {
        if (newState == state) return;
        switch (newState)
        {
            case MovementState.Freeze:

                break;
            case MovementState.Grappling:

                break;
            case MovementState.Swinging:

                break;
            case MovementState.Walking:

                break;
            case MovementState.Sprinting:

                break;
            case MovementState.Crouching:

                break;
            case MovementState.Airborne:

                break;
        }

        state = newState;
        OnMovementStateChanged?.Invoke(state);
    }

}
