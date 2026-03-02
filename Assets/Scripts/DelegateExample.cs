using UnityEngine;
using System;
using Random = UnityEngine.Random;
using System.Collections.Generic;

namespace DelegateExample
{
    public class DelegateExample : MonoBehaviour
    {
        public delegate void SpawnDelegate(Vector3 spawnPosition);

        private SpawnDelegate wave;

        public Action DoSomething;
        public Func<Vector3, string> someFunc;
        public Predicate<Vector3> somePredicate;

        public event Action OnDeath;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            int num = Random.Range(3, 5);
            DoSomething += OnEnable;

            someFunc += PrintPosition;
        }

        private string PrintPosition(Vector3 position)
        {
            return position.ToString();
        }

        private void OnEnable()
        {
            wave += SpawnOrc;
            wave += SpawnOrc;
            wave += SpawnOrc;
            wave += SpawnGoblin;
        }
        private void OnDisable()
        {
            wave -= SpawnOrc;
            wave -= SpawnOrc;
            wave -= SpawnOrc;
            wave -= SpawnGoblin;
        }
        // Update is called once per frame
        void Update()
        {
            wave(new Vector3(1, 0, 1));
        }

        public void SpawnGoblin(Vector3 spawnPosition)
        {
            //Spawn a goblin
        }

        public void SpawnOrc(Vector3 spawnPosition)
        {
            //spwan an Orc
        }


        public void DoSomethingCool(GameObject target, float distance)
        {
            if(target == null) { return; }
            if(target == null) { return; }
            if(target == null) { return; }

            if(Vector3.Distance(transform.position, target.transform.position) < distance)
            { 
                //GetComponent<MeshRenderer>().material.SetVectorArray
            }
        }

    }

    public class SomeClass123
    {
        public DelegateExample example;

        public SomeClass123()
        {
            example.OnDeath += Example_OnDeath;
            //example.OnDeath.Invoke();
            //example.OnDeath = null;
        }

        private void Example_OnDeath()
        {
            throw new NotImplementedException();
        }
    }

    public class Spawner : MonoBehaviour
    {
        public Enemy goblinPrefab;
        private List<Enemy> enemies = new List<Enemy>();

        private void Start()
        {
            EventManager.Register(EventKeys.ON_GAME_STARTED, DoSomethingCool);
            EventManager.Register(EventKeys.ON_ENEMY_KILLED, HandleEnemyKilled);
            EventManager<Enemy>.Register(EventKeys.ON_ENEMY_KILLED, HandleEnemyDeath);
        }

        private void DoSomethingCool()
        {
        }

        private void OnDestroy()
        {
            EventManager.UnRegister(EventKeys.ON_GAME_STARTED, HandleEnemyKilled);
        }

        private void HandleEnemyKilled()
        {
            Debug.Log("Doing something cool!");
        }

        public void Update()
        {
            EventManager.RaiseEvent(EventKeys.ON_GAME_STARTED);
            EventManager<Enemy>.RaiseEvent(EventKeys.ON_GAME_STARTED, new Enemy());
        }

        public void Spawn(Enemy prefab, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Enemy enemy = Instantiate(prefab, new Vector3(0,0,0), Quaternion.identity);
                enemy.OnDeath += HandleEnemyDeath;
                enemies.Add(enemy);
            }
        }

        private void HandleEnemyDeath(Enemy enemy)
        {
            enemies.Remove(enemy);
            Destroy(enemy.gameObject);
        }
    }


    public enum EventKeys
    {
        ON_GAME_STARTED = 1,
        ON_ENEMY_KILLED = 2,
    }

    public static class EventManager
    {
        private static Dictionary<EventKeys, Action> allEvents = new Dictionary<EventKeys, Action>();

        public static void Register(EventKeys key, Action action)
        {
            if (!allEvents.ContainsKey(key))
            {
                allEvents.Add(key, null);
            }
            allEvents[key] += action;
        }

        public static void UnRegister(EventKeys key, Action action)
        {
            if (allEvents.ContainsKey(key))
            {
                allEvents[key] -= action;
            }
        }

        public static void RaiseEvent(EventKeys key)
        {
            if (allEvents.ContainsKey(key))
            {
                allEvents[key]?.Invoke();
            }
        }
    }

    public static class EventManager<T>
    {
        private static Dictionary<EventKeys, Action<T>> allEvents = new Dictionary<EventKeys, Action<T>>();

        public static void Register(EventKeys key, Action<T> action)
        {
            if (!allEvents.ContainsKey(key))
            {
                allEvents.Add(key, null);
            }
            allEvents[key] += action;
        }

        public static void UnRegister(EventKeys key, Action<T> action)
        {
            if (allEvents.ContainsKey(key))
            {
                allEvents[key] -= action;
            }
        }

        public static void RaiseEvent(EventKeys key, T input)
        {
            if (allEvents.ContainsKey(key))
            {
                allEvents[key]?.Invoke(input);
            }
        }
    }

    public static class EventManager<T, U>
    {
        private static Dictionary<EventKeys, Action<T, U>> allEvents = new Dictionary<EventKeys, Action<T, U>>();

        public static void Register(EventKeys key, Action<T, U> action)
        {
            if (!allEvents.ContainsKey(key))
            {
                allEvents.Add(key, null);
            }
            allEvents[key] += action;
        }

        public static void UnRegister(EventKeys key, Action<T, U> action)
        {
            if (allEvents.ContainsKey(key))
            {
                allEvents[key] -= action;
            }
        }

        public static void RaiseEvent(EventKeys key, T input, U input2)
        {
            if (allEvents.ContainsKey(key))
            {
                allEvents[key]?.Invoke(input, input2);
            }
        }
    }
}

