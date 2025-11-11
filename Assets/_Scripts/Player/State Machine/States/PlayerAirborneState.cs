/// <summary>
/// Handles player's movement when there's no ground below them.
/// </summary>
public class PlayerAirborneState : PlayerState
{
    public PlayerAirborneState(StateMachine currentContext, PlayerControllerFacade player, PlayerStateFactory factory) : base(currentContext, player, factory)
    {
        _moveData = new PlayerMovementData(0.2f, 0.1f, 4f);
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void Update()
    {
        CheckTransitionToWalk();
    }

    public override void FixedUpdate()
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