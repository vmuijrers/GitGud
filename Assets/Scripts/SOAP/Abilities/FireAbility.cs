using UnityEngine;

[CreateAssetMenu(fileName = "Fire Ability", menuName = "SO/Abilities/Ability_Fire")]
public class FireAbility : Ability
{
    public override void DoAbility(IAbilityUser user, GameObject target)
    {
        foreach(var effect in effects.list)
        {
            effect.ApplyEffect(user.transform.gameObject, target);
        }

        Debug.Log("Do some Fire!");
    }
}
