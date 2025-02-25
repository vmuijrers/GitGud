using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SOTester : MonoBehaviour
{
    public FloatVariable floatVariable;
    public ListFloatVariable floatVariableList;
    public ListGOVariable goVariable;
    public ListUnitVariable UnitListVariableList;
    public SomeListVariable someClassVariable;

    public EventBus OnDeathEvent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    private void OnEnable()
    {
        OnDeathEvent.Subscribe(OnHandleEvent);
        OnDeathEvent.Subscribe(OnHandleEvent2);
        floatVariable.OnValueChangedEvent += OnFloatVarChanged;
        UnitListVariableList.OnValueItemAddedEvent += OnUnitListChanged;
    }

    private void OnUnitListChanged(Unit unit)
    {
        Debug.Log("Added a unit: " + unit.name);
    }

    private void OnDisable()
    {
        OnDeathEvent.UnSubscribe(OnHandleEvent);
        OnDeathEvent.UnSubscribe(OnHandleEvent2);
        floatVariable.OnValueChangedEvent -= OnFloatVarChanged;
        UnitListVariableList.OnValueItemAddedEvent -= OnUnitListChanged;
    }

    public void OnHandleEvent()
    {
        Debug.Log("Handled Event!");
    }
    public void OnHandleEvent2()
    {
        Debug.Log("Handled Event 2!");
    }
    private void OnFloatVarChanged(float arg0)
    {
        Debug.Log(arg0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnDeathEvent.RaiseEvent();
            floatVariable.Value += 1;
            floatVariableList.Add(floatVariable.Value);
            UnitListVariableList.Add(GetComponent<Unit>());
        }
    }

}
