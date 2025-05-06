using System.Collections.Generic;
using UnityEngine;

public class SlowEffectExampleBad
{
    public float BaseMoveSpeed { get; set; }
    public float MoveSpeedModifier { get; private set; }
    public float MoveSpeed => BaseMoveSpeed * MoveSpeedModifier;

    private List<IEffect> activeEffects = new List<IEffect>();

    public SlowEffectExampleBad(float baseMoveSpeed) 
    { 
        BaseMoveSpeed = baseMoveSpeed;
        MoveSpeedModifier = 1f;
    }

    public void MultiplySpeedModifier(float speedModifier)
    {
        MoveSpeedModifier *= speedModifier;
    }

    public void DivideSpeedModifier(float speedModifier)
    {
        MoveSpeedModifier /= speedModifier;
    }

    public void AddMoveSpeedModifier(float addedValue)
    {
        MoveSpeedModifier += addedValue;
    }

    public void SubtractMoveSpeedModifier(float addedValue)
    {
        MoveSpeedModifier -= addedValue;
    }

    public void ApplyEffect(IEffect effect)
    {
        float value = MoveSpeedModifier;
        effect.Apply(ref value);
        MoveSpeedModifier = value;
    }

    public void RemoveEffect(IEffect effect)
    {
        float value = MoveSpeedModifier;
        effect.Reverse(ref value);
        MoveSpeedModifier = value;
    }

    public void AddEffectToList(IEffect effect)
    {
        if (!activeEffects.Contains(effect))
        {
            activeEffects.Add(effect);
        }
        UpdateModifier();
    }

    private void UpdateModifier()
    {
        float baseModifier = 1f;
        foreach(IEffect effect in activeEffects)
        {
            effect.Apply(ref baseModifier);
        }
        MoveSpeedModifier = baseModifier;
    }

    public void RemoveEffectFromList(IEffect effect)
    {
        if (activeEffects.Contains(effect))
        {
            activeEffects.Remove(effect);
        }
        UpdateModifier();
    }
}

public interface IEffect
{
    void Apply(ref float baseValue);
    void Reverse(ref float baseValue);
}

public class AdditiveSlowEffect : IEffect
{
    public float Value { get; private set; }

    public AdditiveSlowEffect(float value)
    {
        this.Value = value;
    }

    public void Apply(ref float speedValue)
    {
        speedValue += Value;
    }

    public void Reverse(ref float speedValue)
    {
        speedValue -= Value;
    }
}

public class MultiplativeSlowEffect : IEffect
{
    public float Value { get; private set; }

    public MultiplativeSlowEffect(float value)
    {
        this.Value = value;
    }

    public void Apply(ref float speedValue)
    {
        speedValue *= Value;
    }

    public void Reverse(ref float speedValue)
    {
        speedValue /= Value;
    }
}

public class SlowBubbleExample1 : MonoBehaviour
{
    public float value = 3f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<SlowEffectExampleBad>(out SlowEffectExampleBad unit))
        {
            unit.MultiplySpeedModifier(value);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<SlowEffectExampleBad>(out SlowEffectExampleBad unit))
        {
            unit.DivideSpeedModifier(value);
        }
    }
}

public class SlowBubbleExample2 : MonoBehaviour
{
    public IEffect slowEffect;

    public SlowBubbleExample2(IEffect slowEffect)
    {
        this.slowEffect = slowEffect;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<SlowEffectExampleBad>(out SlowEffectExampleBad unit))
        {
            unit.ApplyEffect(slowEffect);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<SlowEffectExampleBad>(out SlowEffectExampleBad unit))
        {
            unit.RemoveEffect(slowEffect);
        }
    }
}

public class SlowBubbleExample3 : MonoBehaviour
{
    private IEffect slowEffect;

    public SlowBubbleExample3(IEffect slowEffect)
    {
        this.slowEffect = slowEffect;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<SlowEffectExampleBad>(out SlowEffectExampleBad unit))
        {
            unit.AddEffectToList(slowEffect);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<SlowEffectExampleBad>(out SlowEffectExampleBad unit))
        {
            unit.RemoveEffectFromList(slowEffect);
        }
    }
}