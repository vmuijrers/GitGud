using System.Collections.Generic;
using UnityEngine;

public class SlowEffectExample
{
    public float BaseMoveSpeed { get; set; }
    public float MoveSpeedModifier { get; private set; }
    public float MoveSpeed => BaseMoveSpeed * MoveSpeedModifier;

    private List<BaseSlowEffect> activeEffects = new List<BaseSlowEffect>();

    public SlowEffectExample(float baseMoveSpeed) 
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

    public void AddEffectToList(BaseSlowEffect effect)
    {
        if (!activeEffects.Contains(effect))
        {
            activeEffects.Add(effect);
        }
        UpdateModifier();
    }

    public void RemoveEffectFromList(BaseSlowEffect effect)
    {
        if (activeEffects.Contains(effect))
        {
            activeEffects.Remove(effect);
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

    public float HandleEffects(ref float speedValue)
    {
        for(int i = 0; i < activeEffects.Count - 1; i++)
        {
            activeEffects[i].NextHandler = activeEffects[i + 1];
        }
        if(activeEffects.Count > 0)
        {
            activeEffects[0]?.Handle(ref speedValue);
        }
        return speedValue;
    }

}

public interface IEffect
{
    void Apply(ref float baseValue);
    void Reverse(ref float baseValue);
}

public class AdditiveSlowEffect : BaseSlowEffect
{
    public float Value { get; private set; }

    public AdditiveSlowEffect(float value)
    {
        this.Value = value;
    }

    public override void Apply(ref float speedValue)
    {
        speedValue += Value;
    }

    public override void Reverse(ref float speedValue)
    {
        speedValue -= Value;
    }
}


public interface IHandler
{
    IHandler NextHandler { get; set; }
    void Handle(ref float inputValue);
}

public abstract class BaseSlowEffect : IEffect, IHandler
{
    public IHandler NextHandler { get; set; }

    public void SetNext(IHandler handler)
    {
        NextHandler = handler;
    }

    public void Handle(ref float inputValue)
    {
        Apply(ref inputValue);
        NextHandler?.Handle(ref inputValue);
    }

    public abstract void Apply(ref float speedValue);
    public abstract void Reverse(ref float speedValue);
}

public class MultiplativeSlowEffect : BaseSlowEffect
{
    public float Value { get; private set; }


    public MultiplativeSlowEffect(float value)
    {
        this.Value = value;
    }

    public override void Apply(ref float speedValue)
    {
        speedValue *= Value;
    }

    public override void Reverse(ref float speedValue)
    {
        speedValue /= Value;
    }
}

public class SlowBubbleExample1 : MonoBehaviour
{
    public float value = 3f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<SlowEffectExample>(out SlowEffectExample unit))
        {
            unit.MultiplySpeedModifier(value);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<SlowEffectExample>(out SlowEffectExample unit))
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
        if (other.TryGetComponent<SlowEffectExample>(out SlowEffectExample unit))
        {
            unit.ApplyEffect(slowEffect);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<SlowEffectExample>(out SlowEffectExample unit))
        {
            unit.RemoveEffect(slowEffect);
        }
    }
}

public class SlowBubbleExample3 : MonoBehaviour
{
    private BaseSlowEffect slowEffect;

    public SlowBubbleExample3(BaseSlowEffect slowEffect)
    {
        this.slowEffect = slowEffect;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<SlowEffectExample>(out SlowEffectExample unit))
        {
            unit.AddEffectToList(slowEffect);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<SlowEffectExample>(out SlowEffectExample unit))
        {
            unit.RemoveEffectFromList(slowEffect);
        }
    }
}
