using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace StateMachines
{

    public class StateMachineExample : MonoBehaviour
    {
        public enum StateEnum
        {
            Idle = 0,
            Chase = 1,
            Patrol = 2
        }

        [SerializeField] private StateEnum state;
        private StateMachine stateMachine;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            IState idleState = new IdleState(new Enemy());
            IState attackState = new AttackState(new Enemy());

            stateMachine = new StateMachine(idleState, idleState, attackState);
        }

        // Update is called once per frame
        void Update()
        {
            stateMachine.OnUpdate();

            switch (state)
            {
                case StateEnum.Idle:

                    //Behaviour Logic

                    //Transition Logic
                    if (Input.GetMouseButtonDown(0))
                    {
                        Debug.Log("Switch to Chase!");
                        state = StateEnum.Chase;
                    }

                    break;
                case StateEnum.Chase:

                    //Behaviour Logic


                    if (Input.GetMouseButtonDown(0))
                    {
                        Debug.Log("Switch to Patrol!");
                        state = StateEnum.Patrol;
                    }
                    break;
                case StateEnum.Patrol:
                    if (Input.GetMouseButtonDown(0))
                    {
                        Debug.Log("Switch to Idle!");
                        state = StateEnum.Idle;
                    }
                    break;
            }
        }
    }


    public interface IState
    {
        void Setup();
        void OnEnter();
        void OnExit();
        void OnUpdate();
    }

    public class StateMachine
    {
        private List<IState> states = new List<IState>();
        private IState currentState;

        public StateMachine(IState startState, params IState[] _states)
        {
            foreach (IState state in _states)
            {
                state.Setup();
                states.Add(state);
            }
            SwitchState(startState);
        }

        public void OnUpdate()
        {
            if (currentState != null)
            {
                currentState.OnUpdate();
            }
        }

        public void AddState(IState state)
        {
            states.Add(state);
        }

        public void SwitchState(IState state)
        {
            currentState?.OnExit();
            currentState = state;
            currentState?.OnEnter();
        }
    }

    public abstract class State<T> : IState
    {
        public T Owner { get; protected set; }

        public State(T owner)
        {
            Owner = owner;
        }

        public abstract void OnEnter();
        public abstract void OnExit();
        public abstract void OnUpdate();
        public abstract void Setup();
    }

    public class IdleState : State<Enemy>
    {
        public IdleState(Enemy owner) : base(owner)
        {

        }

        public override void Setup()
        {
        }

        public override void OnEnter()
        {
        }

        public override void OnExit()
        {
        }

        public override void OnUpdate()
        {
        }


    }
}
