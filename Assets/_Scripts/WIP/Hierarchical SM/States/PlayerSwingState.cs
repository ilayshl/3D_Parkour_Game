public class PlayerSwingState : PlayerState
{
    
    public PlayerSwingState(StateMachine currentContext, PlayerManager player, PlayerStateFactory factory) : base(currentContext, player, factory)
    {
        _moveData = new PlayerMovementData(1f, 1f, 1f);
    }

    public override void CheckTransitionIn()
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void Update()
    {
        //Check if finished- if so, EndState();
        if (!_player.InputReader.IsSwinging)
        {
            _context.ChangeState(_factory.Airborne());
        }
    }

    public override void FixedUpdate()
    {
        _player.HandleSwingMove();
    }

    public override void OnExit()
    {
        //Play sound or something
    }
}
