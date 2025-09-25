using System;

public interface IMovementManager
{
    public event Action<MovementState> OnMovementStateChanged;
    public MovementState State { get; }

    public void ChangeMovementState(MovementState newState = MovementState.Airborne);

}
