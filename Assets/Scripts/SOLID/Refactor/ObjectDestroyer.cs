using UnityEngine;

namespace RefactoredBomb
{
    public class ObjectDestroyer : MonoBehaviour
    {
        [SerializeField] private float delay = 1;

        public void DestroyObject(GameObject target, float delay)
        {
            GameObject.Destroy(target, delay);
        }

        public void DestroySelf()
        {
            Destroy(gameObject, delay);
        }

        public void OnDestroyReaction(GameObject sender, GameObject target)
        {
            DestroyObject(target, delay);
        }
    }
}