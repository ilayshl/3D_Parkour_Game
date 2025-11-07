using System;

public class PlayerWalkState : PlayerState
{
    public PlayerWalkState(StateMachine currentContext, PlayerManager player, PlayerStateFactory factory) : base(currentContext, player, factory)
    {
        _moveData = new PlayerMovementData(5f, 1f, 1f);
    }

    public override void CheckTransitionIn()
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
        _player.InputReader.JumpEvent += Jump;
    }

    public override void Update()
    {
        CheckTransitionToAirborne();
    }

    public override void FixedUpdate()
    {
        _player.HandleMove();
    }

    private void CheckTransitionToAirborne()
    {
        if (!IsGrounded())
        {
            EndState();
        }
    }

    private bool IsGrounded()
    {
        return _player.IsGrounded;
    }

    private void Jump()
    {
        _player.HandleJump();
    }

    public override void OnExit()
    {
        _player.InputReader.JumpEvent -= Jump;
    }
}
