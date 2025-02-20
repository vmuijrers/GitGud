using UnityEngine;

public abstract class RuntimeScriptableObject<T> : ScriptableObject
{
    [SerializeField] protected T value;
    public T Value { 
        get { return value; }
        set
        {
            this.value = value;
            OnValueChangedEvent?.Invoke(value);
        }
    }
    [SerializeField] protected T initialValue;

    public System.Action<T> OnValueChangedEvent;

    protected virtual void OnEnable() => OnReset();
    protected virtual void OnDisable() => OnReset();
    protected virtual void OnReset() { Value = initialValue; }
}

