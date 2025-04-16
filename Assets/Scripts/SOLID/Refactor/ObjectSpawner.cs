using UnityEngine;

namespace RefactoredBomb
{
    public class ObjectSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;

        public void SpawnObject(GameObject target)
        {
            Instantiate(target, transform.position, transform.rotation);
        }

        public void SpawnAssignedObject()
        {
            SpawnObject(prefab);
        }

        public void OnCreateObjectReaction(GameObject sender, GameObject target)
        {
            SpawnObject(target);
        }
    }
}