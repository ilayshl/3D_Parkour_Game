public class PlayerSwingState : PlayerState
{
    private bool _isJumping;

    public PlayerSwingState(StateMachine currentContext, PlayerManager player, PlayerStateFactory factory) : base(currentContext, player, factory)
    {
        _moveData = new PlayerMovementData(1f, 1f, 1f);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _player.InputReader.JumpEvent += GetJumpEvent;
        _player.HandleSwingStart();
    }

    public override void Update()
    {
        if (!_player.InputReader.IsSwinging)
        {
            EndState();
        }

        /* if(_isJumping)
        {
            ShortenRope();
        } */
    }

    public override void FixedUpdate()
    {
        _player.HandleSwingMove();
    }

    public override void OnExit()
    {
        _player.InputReader.JumpEvent -= GetJumpEvent;
        _player.HandleSwingStop();
    }

    private void GetJumpEvent()
    {
        _player.ShortenRope();
    }
}
