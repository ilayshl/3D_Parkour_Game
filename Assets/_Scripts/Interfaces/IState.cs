public interface IState
{
    public void OnEnter();
    public void Tick(float deltaTime);
    public void PhysicsTick(float fixedDeltaTime);
    public void OnExit();
}
