using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "EventClassSO")]
public class SomeEventClass : ScriptableObject
{
    public UnityEvent someEvent;

    public void DoSomething()
    {

    }
}
