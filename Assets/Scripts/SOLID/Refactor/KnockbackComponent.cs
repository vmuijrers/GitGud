using UnityEngine;
namespace RefactoredBomb
{
    public class KnockbackComponent : MonoBehaviour
    {
        [SerializeField] private float force = 1000;

        public void AddForce(Rigidbody rb, Vector3 direction, float force)
        {
            rb.AddForce(direction * force);
        }

        public void AddForceToObject(GameObject sender, GameObject target)
        {
            if(target.TryGetComponent(out Rigidbody rb))
            {
                AddForce(rb, (target.transform.position - sender.transform.position).normalized, force);
            }
        }
    }

}