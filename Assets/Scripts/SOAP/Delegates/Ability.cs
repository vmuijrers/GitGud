using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public abstract void DoAbility(IAbilityUser user);
}

[CreateAssetMenu(fileName = "Fire Ability", menuName = "SO/Delegates/Ability_Fire")]
public class FireAbility : Ability
{
    public override void DoAbility(IAbilityUser user)
    {
        Debug.Log("Do some Fire!");
    }
}

public interface IAbilityUser
{
    Transform transform { get; }
    int Mana { get; }
    bool CanCastAbility();
}