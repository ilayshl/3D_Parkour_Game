using System;

public interface IMovementManager
{
    public Action<MovementState> OnMovementStateChanged { get; set; }
    public MovementState State { get; }

    public void ChangeMovementState(MovementState newState = MovementState.Airborne);

}
