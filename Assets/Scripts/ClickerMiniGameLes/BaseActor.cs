using UnityEngine;
using System;

namespace ClickerExample
{
    public abstract class BaseActor : MonoBehaviour, IClickable, IDamageable, ISetupable
    {
        [field: SerializeField] public int Health { get; protected set; }

        public event Action<BaseActor> DeathEvent;

        public abstract void Setup();
        public abstract void Move();
        public abstract void OnClick();

        public virtual void OnTakeDamage(int damage)
        {
            Health -= damage;
            if(Health < 0)
            {
                Die();
            }
        }

        protected virtual void Die()
        {
            DeathEvent?.Invoke(this);
        }
    }
}

