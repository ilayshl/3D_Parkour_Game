public class PlayerDashState : PlayerState
{
    public PlayerDashState(StateMachine currentContext, PlayerController player, PlayerStateFactory factory) : base(currentContext, player, factory)
    {
            _moveData = new PlayerMovementData(0.1f, 1f, 1f);
    }

    public override void CheckTransitionIn()
    {
        throw new System.NotImplementedException();
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }
    
    public override void Update(float deltaTime)
    {
        //Check if finished- if so, EndState();
    }

    public override void FixedUpdate(float fixedDeltaTime)
    {
        throw new System.NotImplementedException();
    }

    public override void OnExit()
    {
        throw new System.NotImplementedException();
    }
}
