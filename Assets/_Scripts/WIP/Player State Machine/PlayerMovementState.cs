public abstract class PlayerMovementState : MovementState
{
    protected PlayerMovementStateMachine stateMachine;

    public PlayerMovementState(PlayerMovementStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
}

