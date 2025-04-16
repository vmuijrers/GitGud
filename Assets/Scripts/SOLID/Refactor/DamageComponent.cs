using UnityEngine;
namespace RefactoredBomb
{
    public class DamageComponent : MonoBehaviour
    {
        [SerializeField] private float damage = 10;

        public void DealDamage(IDamageable damageable, float damage)
        {
            damageable.TakeDamage(damage);
        }

        public void DealDamage(GameObject sender, GameObject damageableObject)
        {
            if(damageableObject.TryGetComponent(out IDamageable damageable))
            {
                DealDamage(damageable, damage);
            }
        }
    }

}