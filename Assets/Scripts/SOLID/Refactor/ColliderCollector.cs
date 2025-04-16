using UnityEngine;
using UnityEngine.Events;

namespace RefactoredBomb
{
    public class ColliderCollector : MonoBehaviour
    {
        public LayerMask layer;
        public float radius;

        public UnityEvent<GameObject, GameObject> colliderEvent; //sender, target

        public void RaiseEventForCollidersInRange()
        {
            var cols = Physics.OverlapSphere(transform.position, radius, layer);
            foreach(Collider c in cols)
            {
                colliderEvent?.Invoke(gameObject, c.gameObject);
            }
        }
    }
}