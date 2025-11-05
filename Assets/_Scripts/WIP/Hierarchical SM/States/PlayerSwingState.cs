public class PlayerSwingState : PlayerState
{
    
    public PlayerSwingState(StateMachine currentContext, PlayerController player, PlayerStateFactory factory) : base(currentContext, player, factory)
    {
        _moveData = new PlayerMovementData(1f, 1f, 1f);
    }

    public override void CheckTransitionIn()
    {
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
        _player.HandleSwingMove();
    }

    public override void OnExit()
    {
        throw new System.NotImplementedException();
    }
}
