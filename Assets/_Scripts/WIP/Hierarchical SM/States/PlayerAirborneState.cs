public class PlayerAirborneState : PlayerState
{
    public PlayerAirborneState(StateMachine currentContext, PlayerController player, PlayerStateFactory factory) : base(currentContext, player, factory)
    {
        _moveData = new PlayerMovementData(0.2f, 0.1f, 4f);
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
        CheckTransitionToWalk();
    }

    public override void FixedUpdate(float fixedDeltaTime)
    {
        _player.HandleMove();
    }

    private void CheckTransitionToWalk()
    {
        if (_player.IsGrounded) _context.ChangeState(_factory.Walk());
    }

    public override void OnExit()
    {

    }

}