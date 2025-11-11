/// <summary>
/// A temporary script that mimics a specific ability. This will be put on IAbility and PlayerAbilityFacade.
/// </summary>
public class PlayerAbilityState : PlayerState
{
    public PlayerAbilityState(StateMachine currentContext, PlayerControllerFacade player, PlayerStateFactory factory) : base(currentContext, player, factory)
    {
        _moveData = new PlayerMovementData(0.1f, 1f, 1f);
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void Update()
    {
        //Check if finished- if so, EndState();
    }

    public override void FixedUpdate()
    {
        throw new System.NotImplementedException();
    }

    public override void OnExit()
    {
        throw new System.NotImplementedException();
    }

    private void HandleDash()
    {

    }
}
