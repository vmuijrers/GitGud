using System.Collections.Generic;
using UnityEngine;

namespace ClickerExample
{
    public class Spawner : MonoBehaviour
    {
        private List<GameObject> spawnedObjects = new List<GameObject>();

        public void Spawn(GameObject prefab, int amount, Vector2Int spawnArea, System.Action<GameObject> SetupFunction = null) 
        {
            for (int i = 0; i < amount; i++)
            {
                var instance = Instantiate(prefab, new Vector3(
                    Random.Range(-spawnArea.x, spawnArea.x),
                    0,
                    Random.Range(-spawnArea.y, spawnArea.y)),
                    Quaternion.identity);
                spawnedObjects.Add(instance);

                SetupFunction?.Invoke(instance);
            }
        }
    }
}

