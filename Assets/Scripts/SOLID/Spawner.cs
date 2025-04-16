using UnityEngine;
using UnityEngine.InputSystem;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnForce = 100;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            SpawnObject(prefab, spawnPoint);
        }    
    }

    private void SpawnObject(GameObject prefab, Transform spawnPoint)
    {
        var obj = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        obj.GetComponent<Rigidbody>().AddForce(obj.transform.forward * spawnForce);
    }
}


public class BombBuilderPattern
{
    private BombExtended bomb;
    private LayerMask layerMask;
    private float radius = 5;
    private float damage = 10;
    private float delay = 1;

    private BombBuilderPattern()
    {

    }

    public void SpawnBomb(Vector3 position, Quaternion rotation)
    {
        bomb = new BombExtended.BombBuilder()
            .SetDamage(damage)
            .SetTimer(delay)
            .SetHitLayer(layerMask)
            .SetRadius(radius)
            .AddReactionToExplosion(null)
            .SetPosition(position)
            .SetRotation(rotation)
            .Build();
    }
}