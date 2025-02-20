using System.Collections.Generic;

public abstract class ListVariable<T> : RuntimeScriptableObject<List<T>>
{
    public System.Action<T> OnValueItemAddedEvent;
    public System.Action<T> OnValueItemRemovedEvent;

    public void Add(T value)
    {
        if (Value == null) { OnReset(); }
        Value.Add(value);
        OnValueChangedEvent?.Invoke(Value);
        OnValueItemAddedEvent?.Invoke(value);
    }

    public void Remove(T value)
    {
        if (Value == null) { return; }
        Value.Remove(value);
        OnValueChangedEvent?.Invoke(Value);
        OnValueItemRemovedEvent?.Invoke(value);
    }

    protected override void OnReset()
    {
        if (initialValue != null)
        {
            Value = new List<T>();
            for (int i = 0; i < initialValue.Count; i++)
            {
                Value.Add(initialValue[i]);
            }
        }
        else
        {
            Value = new List<T>();
        }
    }
}

[System.Serializable]
public class SomeClass
{
    public int someInt;
    public string someString;
}