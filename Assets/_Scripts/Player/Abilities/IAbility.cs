/// <summary>
/// An interface that every ability has to implement. Referenced by PlayerAbilityFacade
/// </summary>
public interface IAbility
{
    public void Perform();
    public bool IsFinished();
}
