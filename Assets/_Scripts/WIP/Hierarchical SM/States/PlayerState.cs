using UnityEngine;

public abstract class PlayerState
{
    protected readonly StateMachine _context;
    protected readonly PlayerController _player;
    protected readonly PlayerStateFactory _factory;
    protected readonly Rigidbody _rb;
    protected PlayerMovementData _moveData;

    public PlayerState(StateMachine currentContext, PlayerController player, PlayerStateFactory factory)
    {
        _context = currentContext;
        _player = player;
        _factory = factory;
        _rb = player.Rigidbody;
    }

    public virtual void OnEnter()
    {
        _player.SetMoveData(_moveData);
    }

    public abstract void Update(float deltaTime);
    public abstract void FixedUpdate(float fixedDeltaTime); //Physics calculations.
    public abstract void OnExit();
    public virtual void CheckTransitionIn() { return; } //Checks for transition predicate.
    protected virtual void EndState() { _context.ChangeState(_factory.Airborne()); }
}
