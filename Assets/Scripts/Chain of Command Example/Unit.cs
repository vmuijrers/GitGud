using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ChainOfResponsibility
{
    public class Unit : MonoBehaviour
    {
        public float HP = 100;
        //private List<IDamageHandler> damageHandlers = new List<IDamageHandler>();
        private IDamageHandler armor;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Armor armor = new Armor(10);
            MagicShield magicShield = new MagicShield(0.5f);
            DamageAmplifier amplifier = new DamageAmplifier(2f);

            armor.NextHandler = magicShield;
            magicShield.NextHandler = amplifier;
            this.armor = armor;
        }

        // Update is called once per frame
        void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                TakeDamage(new DamageRequest(100));
            }
        }

        public void TakeDamage(DamageRequest damageRequest)
        {
            armor.HandleDamage(damageRequest);
            HP -= damageRequest.Damage;
            HP = Mathf.Max(0, HP);
            Debug.Log(damageRequest.Damage);
            Debug.Log($"HP = {HP}");
        }
    }

    public interface IDamageHandler
    {
        IDamageHandler NextHandler { get; set; }
        void HandleDamage(DamageRequest damageRequest);
    }

    public class Armor : IDamageHandler
    {
        public IDamageHandler NextHandler { get; set; }
        private float armorAmount;

        public Armor(float armorAmount)
        {
            this.armorAmount = armorAmount;
        }

        public void HandleDamage(DamageRequest damageRequest)
        {
            damageRequest.Damage -= armorAmount;
            damageRequest.Damage = Mathf.Max(0, damageRequest.Damage);
            NextHandler?.HandleDamage(damageRequest);
        }
    }

    public class MagicShield : IDamageHandler
    {
        public IDamageHandler NextHandler { get; set; }
        private float shieldAbsorptionPercentage = 1f;

        public MagicShield(float shieldAbsorptionPercentage)
        {
            this.shieldAbsorptionPercentage = shieldAbsorptionPercentage;
        }

        public void HandleDamage(DamageRequest damageRequest)
        {
            damageRequest.Damage *= shieldAbsorptionPercentage;
            NextHandler?.HandleDamage(damageRequest);
        }
    }

    public class DamageAmplifier : IDamageHandler
    {
        public IDamageHandler NextHandler { get; set; }
        private float multiplier = 1f;

        public DamageAmplifier(float multiplier)
        {
            this.multiplier = multiplier;
        }

        public void HandleDamage(DamageRequest damageRequest)
        {
            damageRequest.Damage *= multiplier;
            NextHandler?.HandleDamage(damageRequest);
        }
    }

    public class DamageRequest
    {
        public float Damage { get; set; }

        public DamageRequest(float damage)
        {
            Damage = damage;
        }
    }
}
