using UnityEngine;

[CreateAssetMenu(fileName = "EventBus_", menuName = "SO/Events/EventBus")]
public class EventBus : ScriptableObject
{
    private event System.Action eventToInvoke;

    public void Subscribe(System.Action action)
    {
        eventToInvoke += action;
    }

    public void UnSubscribe(System.Action action)
    {
        eventToInvoke -= action;
    }

    public void RaiseEvent()
    {
        eventToInvoke?.Invoke();
    }

    public void ClearEvent() => eventToInvoke = null;

    public void OnEnable() => ClearEvent();
    public void OnDisable() => ClearEvent();
}
