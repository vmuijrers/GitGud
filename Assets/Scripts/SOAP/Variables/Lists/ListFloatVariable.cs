using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "FloatListVariable", menuName = "SO/FloatList")]
public class ListFloatVariable : RuntimeScriptableObject<List<float>>
{
    public void Add(float value)
    {
        if(Value == null) { OnReset(); }
        Value.Add(value);
        OnValueChangedEvent?.Invoke(Value);
    }

    public void Remove(float value)
    {
        if (Value == null) { return; }
        Value.Remove(value);
        OnValueChangedEvent?.Invoke(Value);
    }

    protected override void OnReset()
    {
        if(initialValue != null)
        {
            Value = new List<float>();
            for (int i = 0; i < initialValue.Count; i++)
            {
                Value.Add(initialValue[i]);
            }
        }
        else
        {
            Value = new List<float>();
        }
    }
}

