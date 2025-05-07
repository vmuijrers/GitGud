using CustomEventManager;
using UnityEngine;
using UnityEngine.Events;

public class EventReaction : MonoBehaviour
{
    public EventTypeCustom EventType;
    public UnityEvent OnEvent;

    private void OnEnable()
    {
        EventManager.AddListener(EventType, () => OnEvent?.Invoke());
    }

    private void OnDisable()
    {
        EventManager.RemoveListener(EventType, () => OnEvent?.Invoke());
    }
}
