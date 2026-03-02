using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace StateMachines
{
    // Context
    public class StateMachineExample : MonoBehaviour
    {
        public enum StateEnum
        {
            Idle = 0,
            Chase = 1,
            Patrol = 2
        }

        [field: SerializeField] public int Health { get; set; } = 100;

        //[SerializeField] private StateEnum state;
        private StateMachine stateMachine;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            IState idleState = new IdleState(this);
            IState attackState = new AttackState(this);
            ITransition idle2AttackState = new Transition<StateMachineExample>(idleState, attackState, this, (x) => x.Health < 30); // lambda function
            ITransition attack2IdleState = new Transition<StateMachineExample>(attackState, idleState, this, (x) => x.Health > 50); // lambda function
            stateMachine = new StateMachine(idleState, idleState, attackState);
            stateMachine.AddTransition(idle2AttackState);
            stateMachine.AddTransition(attack2IdleState);
            stateMachine.SwitchState(idleState);
        }

        private bool SomeFunc(Enemy enemy)
        {
            return true;
        }

        // Update is called once per frame
        void Update()
        {
            stateMachine.OnUpdate();

            //switch (state)
            //{
            //    case StateEnum.Idle:

            //        //Behaviour Logic

            //        //Transition Logic
            //        if (Input.GetMouseButtonDown(0))
            //        {
            //            Debug.Log("Switch to Chase!");
            //            state = StateEnum.Chase;
            //        }

            //        break;
            //    case StateEnum.Chase:

            //        //Behaviour Logic


            //        if (Input.GetMouseButtonDown(0))
            //        {
            //            Debug.Log("Switch to Patrol!");
            //            state = StateEnum.Patrol;
            //        }
            //        break;
            //    case StateEnum.Patrol:
            //        if (Input.GetMouseButtonDown(0))
            //        {
            //            Debug.Log("Switch to Idle!");
            //            state = StateEnum.Idle;
            //        }
            //        break;
            //}
        }
    }


    public interface IState
    {
        string Name { get; }
        void Setup();
        void OnEnter();
        void OnExit();
        void OnUpdate();
    }

    public interface ITransition
    {
        IState FromState { get; }
        IState ToState { get; }
        bool Evaluate();
    }

    public class Transition<T> : ITransition
    {
        private T owner;
        public System.Func<T, bool> condition { get; protected set; }
        public IState FromState { get; protected set; }
        public IState ToState { get; protected set; }

        public Transition(IState fromState, IState toState, T owner, System.Func<T, bool> condition)
        {
            this.owner = owner;
            this.condition = condition;
            this.FromState = fromState;
            this.ToState = toState;
        }

        public bool Evaluate()
        {
            return condition.Invoke(owner);
        }
    }

    public class EventTransition<T>
    {
        private System.Action<T> eventReference;
        private System.Action<T> action;
        public IState fromState { get; private set; }
        public IState toState { get; private set; }

        public EventTransition(IState fromState, IState toState, T owner, System.Action<T> eventReference, System.Action<T> action)
        {
            this.fromState = fromState;
            this.toState = toState;
            this.eventReference = eventReference;
            this.action = action;
        }

        public void Register()
        {
            eventReference += action;
        }

        public void UnRegister()
        {
            eventReference -= action;
        }
    }

    public class StateMachine
    {
        private List<IState> states = new List<IState>();
        private IState currentState;
        private List<ITransition> transitions = new List<ITransition>();
        private List<ITransition> activeTransitions = new List<ITransition>();

        public StateMachine(params IState[] _states)
        {
            foreach (IState state in _states)
            {
                state.Setup();
                states.Add(state);
            }
        }

        public void OnUpdate()
        {
            // Check Transitions
            foreach(var transition in activeTransitions)
            {
                if (transition.Evaluate())
                {
                    SwitchState(transition.ToState);
                    return;
                }
            }

            if (currentState != null)
            {
                currentState.OnUpdate();
            }
        }

        public void AddTransition(ITransition transition)
        {
            transitions.Add(transition);
        }

        public void AddState(IState state)
        {
            states.Add(state);
        }

        public void SwitchState(IState toState)
        {
            currentState?.OnExit();
            currentState = toState;
            currentState?.OnEnter();
            activeTransitions = transitions.FindAll(x => x.FromState == currentState);
        }
    }

    public abstract class State<T> : IState
    {
        public T Owner { get; protected set; }

        public string Name => GetType().ToString();

        public State(T owner)
        {
            Owner = owner;
        }

        public abstract void OnEnter();
        public abstract void OnExit();
        public abstract void OnUpdate();
        public abstract void Setup();
    }

    public class IdleState : State<StateMachineExample>
    {
        public IdleState(StateMachineExample owner) : base(owner)
        {

        }

        public override void Setup()
        {
        }

        public override void OnEnter()
        {
            Debug.Log("Enterd State: " + GetType().ToString());
        }

        public override void OnExit()
        {
            Debug.Log("Exited State: " + GetType().ToString());
        }

        public override void OnUpdate()
        {
            //Debug.Log("Updating State: " + GetType().ToString());

        }


    }

    public class AttackState : State<StateMachineExample>
    {
        public AttackState(StateMachineExample owner) : base(owner)
        {

        }

        public override void Setup()
        {
        }

        public override void OnEnter()
        {
            Debug.Log("Enterd State: " + GetType().ToString());
        }

        public override void OnExit()
        {
            Debug.Log("Exited State: " + GetType().ToString());

        }

        public override void OnUpdate()
        {
            //Debug.Log("Updating State: " + GetType().ToString());
        }


    }
}
