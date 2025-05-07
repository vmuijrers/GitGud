using CustomEventManager;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputEvent : MonoBehaviour
{
    public EventTypeCustom eventType;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            EventManager.RaiseEvent(eventType);
        }
    }
}
