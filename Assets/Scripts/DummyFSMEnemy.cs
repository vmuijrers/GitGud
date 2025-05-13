using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Linq.Expressions;
using UnityEditor;
using Unity.VisualScripting;

public class DummyFSMEnemy : MonoBehaviour
{
    public float Health = 100;
    public float Speed = 100;
    private FSM fsm;

    void Start()
    {
        fsm = new FSM();
        IState attackState = new AttackState(this);
        IState idleState = new IdleState(this);

        fsm.AddState(attackState);
        fsm.AddState(idleState);

        fsm.AddTransition(new Transition(idleState, attackState, () => Keyboard.current.spaceKey.wasPressedThisFrame));
        fsm.AddTransition(new Transition(attackState, idleState, () => Keyboard.current.spaceKey.wasPressedThisFrame));

        fsm.SwitchState(idleState);
    }

    private void Update()
    {
        fsm.OnUpdate();
    }
}

public class FSM
{
    public IState CurrentState { get; private set; }
    private Dictionary<System.Type, IState> allStates = new Dictionary<System.Type, IState>();
    private List<Transition> transitions = new List<Transition>();
    private List<Transition> currentTransitions = new List<Transition>();

    public void OnUpdate()
    {
        foreach(var transition in currentTransitions)
        {
            if (transition.Evaluate())
            {
                SwitchState(transition.toState);
                return;
            }
        }

        CurrentState?.OnUpdate();
    }

    public void AddState(IState state)
    {
        allStates.TryAdd(state.GetType(), state);
    }

    public void RemoveState(System.Type type)
    {
        if(allStates.ContainsKey(type))
        {
            allStates.Remove(type);
        }
    }

    public void SwitchState(IState state)
    {
        CurrentState?.OnExit();
        CurrentState = state;
        if (CurrentState != null)
        {
            currentTransitions = transitions.FindAll(x => x.fromState == CurrentState || x.fromState == null);
        }
        CurrentState?.OnEnter();
    }

    public void SwitchState(System.Type type)
    {
        if (allStates.ContainsKey(type))
        {
            SwitchState(allStates[type]);
        }
    }

    public void AddTransition(Transition transition)
    {
        transitions.Add(transition);
    }
}

public interface IState
{
    void OnUpdate();
    void OnEnter();
    void OnExit();
}

public abstract class BaseState<T> : IState
{
    protected T owner;
    public BaseState(T owner)
    {
        this.owner = owner;
    }

    public abstract void OnUpdate();
    public abstract void OnEnter();
    public abstract void OnExit();
}

public class Transition
{
    public readonly IState fromState;
    public readonly IState toState;
    private System.Func<bool> condition;

    public Transition(IState fromState, IState toState, System.Func<bool> condition)
    {
        this.fromState = fromState;
        this.toState = toState;
        this.condition = condition;
    }

    public bool Evaluate()
    {
        return condition();
    }
}

public class IdleState : BaseState<DummyFSMEnemy>
{
    public IdleState(DummyFSMEnemy owner) : base(owner) { }

    public override void OnEnter()
    {
        Debug.Log("Entered Idle State");

    }

    public override void OnExit()
    {
        Debug.Log("Exited Idle State");

    }

    public override void OnUpdate()
    {
        Debug.Log("Update Idle State");
    }
}

public class AttackState : BaseState<DummyFSMEnemy>
{
    public AttackState(DummyFSMEnemy owner) : base(owner) { }

    public override void OnEnter()
    {
        Debug.Log("Entered Attack State");
    }

    public override void OnExit()
    {
        Debug.Log("Exited Attack State");

    }

    public override void OnUpdate()
    {
        Debug.Log("Update Attack State");

    }
}




