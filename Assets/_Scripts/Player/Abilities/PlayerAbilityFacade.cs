using UnityEngine;

/// <summary>
/// Placeholder: Will add different abilities.
/// </summary>
public class PlayerAbilityFacade
{
    IAbility _equippedAbility;

    public PlayerAbilityFacade(IAbility ability)
    {
        _equippedAbility = ability;
    }

    public void Perform()
    {
        _equippedAbility.Perform();
    }


}
