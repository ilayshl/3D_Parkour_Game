public class PlayerSwingState : PlayerState
{
    public PlayerSwingState(StateMachine currentContext, PlayerManager player, PlayerStateFactory factory) : base(currentContext, player, factory)
    {
        _moveData = new PlayerMovementData(0.2f, 0.1f, 4f);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _player.HandleSwingStart();
    }

    public override void Update()
    {
        if (!_player.InputReader.IsSwinging)
        {
            EndState();
        }

        if(_player.InputReader.IsJumping)
        {
            ShortenRope();
        }
    }

    public override void FixedUpdate()
    {
        _player.HandleSwingMove();
    }

    public override void OnExit()
    {
        _player.HandleSwingStop();
    }

    private void ShortenRope()
    {
        _player.ShortenRope();
    }
}
