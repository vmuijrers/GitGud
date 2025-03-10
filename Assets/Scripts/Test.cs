using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AbilitySystem
{
    public class Test : MonoBehaviour
    {
        private Rigidbody rb;

        void Start()
        {
            string someString = $"{3} yo this is cool";
            rb = GetComponent<Rigidbody>() ?? gameObject.AddComponent<Rigidbody>();
            int? a = 5;
            int b = a ?? 3;
            if (a == null)
            {
                b = 3;
            }
            Debug.Log(b);
        }

        void Update()
        {

        }
    }

    public class UnitController : MonoBehaviour
    {
        public List<AbilityUnit> selection = new List<AbilityUnit>();
        private Ability currentSelectedAbility = null;
        private AbilityUnit currentActiveCaster = null;
        public InputAction mouseClickAction;
        public InputAction cancelAction;

        private void Update()
        {
            if (HasActiveAbilityAndCaster())
            {
                if (mouseClickAction.WasPressedThisFrame())
                {
                    currentSelectedAbility.OnClickedAtScreenPosition(Mouse.current.position.ReadValue());
                }

                if (cancelAction.WasPressedThisFrame())
                {
                    ClearAbilityAndCaster();
                }

                return;
            }

            ClearAbilityAndCaster();

            foreach (var unit in selection)
            {
                currentSelectedAbility = SelectAbilityForUnit(unit);
                if (HasActiveAbility())
                {
                    currentSelectedAbility.AbilityCommandEvent += OnHandleAbilityCommandSendSuccessfully;
                    currentActiveCaster = unit;
                    return; //Skip a frame
                }
            }
        }

        private void ClearAbilityAndCaster()
        {
            if (currentSelectedAbility != null)
            {
                currentSelectedAbility.AbilityCommandEvent -= OnHandleAbilityCommandSendSuccessfully;
            }
            currentSelectedAbility = null;
            currentActiveCaster = null;
        }

        private void OnHandleAbilityCommandSendSuccessfully(AbilityCommand cmd)
        {
            cmd.ability.AbilityCommandEvent -= OnHandleAbilityCommandSendSuccessfully;
        }

        private bool HasActiveAbilityAndCaster()
        {
            return HasActiveAbility() && HasActiveCaster();
        }

        private bool HasActiveAbility()
        {
            return currentSelectedAbility != null;
        }

        private bool HasActiveCaster()
        {
            return (currentActiveCaster != null && !currentActiveCaster.IsDead);
        }

        private Ability SelectAbilityForUnit(AbilityUnit unit)
        {
            var ability = unit.CheckForActiveAbilities();
            if (ability != null)
            {
                return ability;
            }
            return null;
        }
    }

    public class AbilityUnit : MonoBehaviour
    {
        public float Health = 100;
        public bool IsDead => Health > 0;
        public List<AbilitySlot> abilities = new List<AbilitySlot>();



        public Ability CheckForActiveAbilities()
        {
            return abilities.FirstOrDefault(x => x.ability.IsReady() && x.inputAction.WasPressedThisFrame()).ability;
        }

    }

    [System.Serializable]
    public class AbilitySlot : ScriptableObject
    {
        public InputAction inputAction;
        public Ability ability;
    }

    public abstract class Ability : ScriptableObject
    {
        public LayerMask abilityTargetingHitLayer;
        public float castRange = 10;
        public float maxCooldown = 10;
        private Timer cooldownTimer;

        public event System.Action<Ability> AbilityCastSuccessfullyEvent;
        public event System.Action<AbilityCommand> AbilityCommandEvent;

        void OnSetup()
        {
            cooldownTimer = new Timer(maxCooldown, true, OnCooldownDone);
        }

        public bool IsReady() => cooldownTimer.IsTimerDone;

        public void OnClickedAtScreenPosition(Vector2 mouseScreenPosition)
        {
            var ray = Camera.main.ScreenPointToRay(mouseScreenPosition);
            Vector3 worldPosition;
            GameObject hitObject = null;
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 100, abilityTargetingHitLayer))
            {
                hitObject = hitInfo.collider.gameObject;
                worldPosition = hitInfo.point;
                HandleClick(worldPosition, hitObject);
            }
        }

        public abstract void HandleClick(Vector3 worldPosition, GameObject hitObject);

        protected virtual void OnCooldownDone()
        {
            Debug.Log("Cooldown Done!");
        }

        public void StartCooldown()
        {
            cooldownTimer.Start();
        }

        public virtual void OnCastAbility()
        {
            AbilityCastSuccessfullyEvent?.Invoke(this);
        }

        public void SendAbilityCommand(AbilityCommand cmd)
        {
            AbilityCommandEvent?.Invoke(cmd);
        }
    }

    public abstract class AbilityCommand
    {
        public Ability ability;

        public void Execute(AbilityUnit unit)
        {
            //unit.BehaviourTree.SendMessage(ability.name, ability.targetPosition);
                //while (Caster and Target Conditions == OK && NotInRange)
                    //Move in Range
                //Cast ability
        }
    }

    public class PositionalCommand : AbilityCommand
    {
        public string cmdName;
        public Vector3 pos;
    }

    public abstract class PositionalAbility : Ability
    {
        public override void HandleClick(Vector3 worldPosition, GameObject hitObject)
        {
            var cmd = new PositionalCommand();
            cmd.pos = worldPosition;
            cmd.ability = this;
            SendAbilityCommand(cmd);
        }

        public abstract void CastAbility(PositionalCommand cmd);
    }

    public class Timer
    {
        public float currentTime;
        public float initialTime;
        public bool isPaused = false;

        public event System.Action OnTimerDone;

        public Timer(float initialTime, bool isPaused, System.Action OnDone = null)
        {
            this.initialTime = initialTime;
            this.isPaused = isPaused;
            Reset();
            OnTimerDone = OnDone;
        }

        public bool IsTimerDone => currentTime < 0;

        public void Reset()
        {
            currentTime = initialTime;
        }

        public void Pause()
        {
            isPaused = true;
        }

        public void Resume()
        {
            isPaused = false;
        }

        public void Start()
        {
            Reset();
            Resume();
        }

        public void OnUpdate()
        {
            if (isPaused) { return; }
            if (currentTime < 0) { return; }
            currentTime -= Time.deltaTime;
            if (currentTime < 0)
            {
                OnTimerDone?.Invoke();
            }
        }
    }
}
