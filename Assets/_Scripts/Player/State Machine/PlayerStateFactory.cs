public class PlayerStateFactory
{
    private StateMachine _context;
    private PlayerManager _player;

    public PlayerStateFactory(StateMachine currentContext, PlayerManager player)
    {
        _context = currentContext;
        _player = player;
    }

    /* Trying out Generics for this- can't figure out a solution though. I need to make a type param return a new instance of the type...
    public T GetState<T>() where T: PlayerState
    {
        return new T(_context, _player, this);
    } */

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
