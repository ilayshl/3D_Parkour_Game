using UnityEngine;

/// <summary>
/// An abstract state that all states inherit from.
/// </summary>
public abstract class PlayerState
{
    protected readonly StateMachine _context;
    protected readonly PlayerControllerFacade _player;
    protected readonly PlayerStateFactory _factory;
    protected readonly Rigidbody _rb;
    protected PlayerMovementData _moveData;

    public PlayerState(StateMachine currentContext, PlayerControllerFacade player, PlayerStateFactory factory)
    {
        _context = currentContext;
        _player = player;
        _factory = factory;
        _rb = player.Rigidbody;
    }

    public virtual void OnEnter()
    {
        _player.SetMovementData(_moveData);
    }

    public abstract void Update();
    public abstract void FixedUpdate(); //Physics calculations.
    public abstract void OnExit();
    protected virtual void EndState() { _context.ChangeState(_factory.Airborne()); }
}
