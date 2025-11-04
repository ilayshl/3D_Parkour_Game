public class PlayerStateFactory
{
    private PlayerStateMachine _context;
    private PlayerController _player;

    public PlayerStateFactory(PlayerStateMachine currentContext, PlayerController player)
    {
        _context = currentContext;
    }

    public PlayerBaseState Walk()
    {
        return new PlayerWalkState(_context, _player, this);
    }

    public PlayerBaseState Jump()
    {
        return new PlayerJumpState(_context, _player, this);
    }

    public PlayerBaseState Grounded()
    {
        return new PlayerGroundedState(_context, _player, this);
    }

    public PlayerBaseState Dash()
    {
        return new PlayerDashState(_context, _player, this);
    }

    public PlayerBaseState Swing()
    {
        return new PlayerSwingState(_context, _player, this);
    }

    public PlayerBaseState Airborne()
    {
        return new PlayerAirborneState(_context, _player, this);
    }
    
}
