public class PlayerStateFactory
{
    private StateMachine _context;
    private PlayerController _player;

    public PlayerStateFactory(StateMachine currentContext, PlayerController player)
    {
        _context = currentContext;
        _player = player;
    }

    public PlayerState Walk()
    {
        return new PlayerWalkState(_context, _player, this);
    }

    public PlayerState Airborne()
    {
        return new PlayerAirborneState(_context, _player, this);
    }

    public PlayerState Dash()
    {
        return new PlayerDashState(_context, _player, this);
    }

    public PlayerState Swing()
    {
        return new PlayerSwingState(_context, _player, this);
    }
}
