using UnityEngine;

public abstract class PlayerBaseState
{
    protected PlayerStateMachine _context;
    protected PlayerStateFactory _factory;
    protected PlayerController _player;
    protected Rigidbody _rb;

    /* protected PlayerBaseState _currentSuperState;
    protected PlayerBaseState _currentSubState;
 */
    public PlayerBaseState(PlayerStateMachine currentContext, PlayerController player, PlayerStateFactory stateFactory)
    {
        _context = currentContext;
        _factory = stateFactory;
        _player = player;
        _rb = player.Rigidbody;
    }

    public abstract void OnEnter();
    public abstract void Update();
    public abstract void FixedUpdate();
    public abstract void OnExit();
    public abstract void CheckSwitchStates();
    public abstract void InitializeSubState();

    protected void SwitchState(PlayerBaseState newState)
    {
        _context.ChangeState(newState);
    }

    /* protected void SetSuperState(PlayerBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }
    protected void SetSubState(PlayerBaseState newSubState)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    } */

}
