using UnityEngine;
using UnityEngine.Events;

namespace RefactoredBomb
{
    public class ColliderCollector : MonoBehaviour
    {
        [SerializeField] private LayerMask layer;
        [SerializeField] private float radius;

        public UnityEvent<GameObject, GameObject> colliderEvent; //sender, target

        /// <summary>
        /// Is Called via Inspector
        /// </summary>
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