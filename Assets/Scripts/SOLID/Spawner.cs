using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
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

namespace Factory
{
    public class GM : MonoBehaviour
    {
        [SerializeField] private Enemy prefabOrc;
        [SerializeField] private Enemy prefabGoblin;

        private void Setup()
        {
            var enemyfactory = EnemyFactory.Instance;
            enemyfactory.SetPrefabGoblin(prefabGoblin);
            enemyfactory.SetPrefabOrc(prefabOrc);
            enemyfactory.CreateGoblin(transform.position);
            enemyfactory.CreateOrc(transform.position);

            var goblinSpawner = new Spawner<Enemy>(new GoblinFactory());
            goblinSpawner.SpawnAtPosition(transform.position); //Spawn a goblin

            var orcSpawner = new Spawner<Enemy>(new GoblinFactory());
            orcSpawner.SpawnAtPosition(transform.position); //Spawn an orc
        }
    }

    public class Spawner<T>
    {
        private IFactory<T> factory;
        public Spawner(IFactory<T> factory)
        {
            this.factory =  factory;
        }

        public T SpawnAtPosition(Vector3 position)
        {
            return factory.CreateAtPosition(position);
        }
    }

    public interface IFactory<T>
    {
        T CreateAtPosition(Vector3 position);
    }

    public class Singleton<T>
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Activator.CreateInstance<T>();
                }
                else
                {
                    Debug.LogError("Instance already exists!");
                }
                return instance;
            }
        }

    }

    public class EnemyFactory : Singleton<EnemyFactory>
    {
        private Enemy prefabOrcPrototype;
        private Enemy prefabGoblinPrototype;

        public void SetPrefabOrc(Enemy prefabOrc)
        {
            this.prefabOrcPrototype = prefabOrc;
        }

        public void SetPrefabGoblin(Enemy prefabGoblin)
        {
            this.prefabGoblinPrototype = prefabGoblin;
        }

        public Enemy CreateOrc(Vector3 position)
        {
            Enemy enemy = prefabOrcPrototype.Clone() as Enemy;
            enemy.SetPosition(position);
            return enemy;
        }

        public Enemy CreateGoblin(Vector3 position)
        {
            Enemy enemy = prefabGoblinPrototype.Clone() as Enemy;
            enemy.SetPosition(position);
            return enemy;
        }
    }

    public class OrcFactory : IFactory<Enemy>
    {
        public class Orc : Enemy
        {
            public Orc()
            {

            }
            public void GrowlOnSpawn()
            {

            }
        }

        public Enemy CreateAtPosition(Vector3 position)
        {
            var orc = new Orc();
            orc.SetPosition(position);
            //Do Specific Orc stuff here!
            orc.GrowlOnSpawn();
            return orc;
        }
    }

    //Specific Goblin Factory
    public class GoblinFactory : IFactory<Enemy>
    {
        public class Goblin : Enemy
        {
            public Goblin()
            {

            }

            public void Shriek()
            {

            }
        }

        public Enemy CreateAtPosition(Vector3 position)
        {
            var goblin = new Goblin();
            goblin.SetPosition(position);
            //Do specific Goblin stuff here!
            goblin.Shriek();
            return goblin;
        }
    }

    //Generic Enemy factory
    public class EnemyFactory1 : IFactory<Enemy>
    {
        private Enemy enemyTemplate;

        public EnemyFactory1(Enemy enemyTemplate)
        {
            this.enemyTemplate = enemyTemplate;
        }

        public Enemy CreateAtPosition(Vector3 position)
        {
            var item = enemyTemplate.Clone();
            item.SetPosition(position);
            return item;
        }
    }

    public interface IFactoryString<T>
    {
        T CreateAtPosition(string enemyType, Vector3 position);
    }

    public class EnemyFactoryString : IFactoryString<Enemy>
    {
        public Enemy CreateByName(string enemyType)
        {
            switch(enemyType)
            {
                case "Orc": return new OrcFactory.Orc();
                case "Goblin": return new GoblinFactory.Goblin();
            }
            return null;
        }

        public Enemy CreateAtPosition(string enemyType, Vector3 position)
        {
            var result = CreateByName(enemyType);
            if(result != null)
            {
                result.SetPosition(position);
            }
            return result;
        }
    }
}

