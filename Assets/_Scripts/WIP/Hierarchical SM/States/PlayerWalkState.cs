using System;

public class PlayerWalkState : PlayerState
{
    public PlayerWalkState(StateMachine currentContext, PlayerController player, PlayerStateFactory factory) : base(currentContext, player, factory)
    {
        _moveData = new PlayerMovementData(5f, 1f, 1f);
    }

    public override void CheckTransitionIn()
    {
        
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void Update(float deltaTime)
    {
        CheckTransitionToAirborne();
    }

    public override void FixedUpdate(float fixedDeltaTime)
    {
        _player.HandleMove();
    }

    private void CheckTransitionToAirborne()
    {
        if (!IsGrounded() || CheckForJump()) EndState();
    }

    private bool IsGrounded()
    {
        return _player.IsGrounded;
    }

    private bool CheckForJump()
    {
        return false;
    }

    public override void OnExit()
    {
        
    }
}
