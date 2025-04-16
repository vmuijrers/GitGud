using UnityEngine;
namespace RefactoredBomb
{
    public class HealthComponent : MonoBehaviour, IDamageable
    {
        [SerializeField] private float health = 100;
        public float Health => health;

        public void TakeDamage(float damage)
        {
            health -= damage;
        }
    }

}