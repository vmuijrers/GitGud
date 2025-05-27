using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public abstract class Ability : ScriptableObject
{
    [SerializeReference] public ReorderableList<AbilityEffect> effects = new ReorderableList<AbilityEffect>();
    public abstract void DoAbility(IAbilityUser caster, GameObject target);
}

public interface IAbilityUser
{
    Transform transform { get; }
    int Mana { get; }
    bool CanCastAbility();
}
public interface IAbilityEffect
{
    void ApplyEffect(GameObject caster, GameObject target);
}
[System.Serializable]
public abstract class AbilityEffect : IAbilityEffect
{
    public abstract void ApplyEffect(GameObject caster, GameObject target);
}

[System.Serializable]
public class DamageEffect : AbilityEffect
{
    public int damage;

    public override void ApplyEffect(GameObject caster, GameObject target)
    {
        target.GetComponent<Health>().TakeDamage(damage);
    }
}