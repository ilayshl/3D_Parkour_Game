using UnityEngine;

public interface IMovementStateReceiver
{
    public void OnMovementStateChanged(MovementState newState);
}
