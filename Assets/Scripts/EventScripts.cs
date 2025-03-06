using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using UnityEngine.Events;
using CustomEventManager;
using System.Linq;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;

public class EventScripts : MonoBehaviour
{
    public UnityEvent OnSomeEvent;

    public delegate void WaveSpawner();
    public WaveSpawner waveSpawner;

    public System.Action myACtion;
    public System.Action<float> myFloatAction;

    public event System.Action ExplodeEvent;

    void Start()
    {
        HandleCommand("DoSomething", 3, "jpo", 3.0f, true);
        waveSpawner += SpawnOrcs;
        waveSpawner += SpawnOrcs;
        waveSpawner += SpawnOrcs;

        myACtion += SpawnOrcs;

        myFloatAction += TakeDamage;
    }

    private void OnDestroy()
    {
        waveSpawner = null;
        ExplodeEvent = null;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            waveSpawner?.Invoke();
            myACtion?.Invoke();
            ExplodeEvent?.Invoke();
            EventManager.RaiseEvent(EventTypeCustom.EVENT_ON_GAME_STARTED);
        }
    }

    public void TakeDamage(float damage)
    {

    }

    /// <summary>
    /// Command Handler voor Tim
    /// </summary>
    /// <param name="command"></param>
    /// <param name="args"></param>
    /// <exception cref="MissingMethodException"></exception>
    public void HandleCommand(string command, params object[] args)
    {
        Type[] paramTypes = args.Select(p => p?.GetType()).ToArray();

        MethodInfo method = GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
            .FirstOrDefault(m => m.Name == command && ParametersMatch(m.GetParameters(), paramTypes));

        if (method == null)
        {
            throw new MissingMethodException($"No matching method '{command}' found in class '{GetType().Name}'.");
        }

        object instance = method.IsStatic ? null : Activator.CreateInstance(GetType());
        method?.Invoke(instance, args);
    }

    private bool ParametersMatch(ParameterInfo[] methodParams, Type[] providedTypes)
    {
        if (methodParams.Length != providedTypes.Length)
        {
            return false;
        }

        for (int i = 0; i < methodParams.Length; i++)
        {
            if (providedTypes[i] != null && !methodParams[i].ParameterType.IsAssignableFrom(providedTypes[i]))
            {
                return false;
            }
        }

        return true;
    }

    public void SpawnOrcs()
    {

    }
}

public class SomeClass3 : MonoBehaviour, IDisposable
{
    public EventScripts eventReference;

    private void Start()
    {
        EventManager.AddListener(EventTypeCustom.EVENT_ON_GAME_STARTED, OnExplode);
        //eventReference.ExplodeEvent.Invoke();
        //eventReference.ExplodeEvent = null;
    }

    void OnEnable()
    {
        eventReference.ExplodeEvent += OnExplode;
    }

    void OnDisable()
    {
        EventManager.RemoveListener(EventTypeCustom.EVENT_ON_GAME_STARTED, OnExplode);
        EventManager<float>.AddListener(EventTypeCustom.EVENT_ON_GAME_STARTED, DoSomehtingCool);
        //EventManager<Unit>.AddListener(EventType.EVENT_ON_GAME_STARTED, DoSomehtingCool);
        eventReference.ExplodeEvent -= OnExplode;
    }

    private void OnDestroy()
    {

    }

    private void OnExplode()
    {

    }

    public void DoSomehtingCool(float coolness)
    {

    }

    public void Dispose()
    {
        eventReference.ExplodeEvent -= OnExplode;
    }
}


namespace CustomEventManager
{
    public enum EventTypeCustom
    {
        EVENT_ON_GAME_STARTED = 0, //0
    }
    public static class EventManager
    {
        private static Dictionary<EventTypeCustom, System.Action> eventDictionary = new();

        public static void RaiseEvent(EventTypeCustom eventType)
        {
            if (eventDictionary.ContainsKey(eventType))
            {
                eventDictionary[eventType]?.Invoke();
            }
        }

        public static void AddListener(EventTypeCustom eventType, System.Action listener)
        {
            if (!eventDictionary.ContainsKey(eventType))
            {
                eventDictionary.Add(eventType, null);
            }
            eventDictionary[eventType] += listener;
        }

        public static void RemoveListener(EventTypeCustom eventType, System.Action listener)
        {
            if (eventDictionary.ContainsKey(eventType))
            {
                eventDictionary[eventType] -= listener;
            }
        }
    }

    public static class EventManager<T>
    {
        private static Dictionary<EventTypeCustom, System.Action<T>> eventDictionary = new();

        public static void RaiseEvent(EventTypeCustom eventType, T arg)
        {
            if (eventDictionary.ContainsKey(eventType))
            {
                eventDictionary[eventType]?.Invoke(arg);
            }
        }

        public static void AddListener(EventTypeCustom eventType, System.Action<T> listener)
        {
            if (!eventDictionary.ContainsKey(eventType))
            {
                eventDictionary.Add(eventType, null);
            }
            eventDictionary[eventType] += listener;
        }

        public static void RemoveListener(EventTypeCustom eventType, System.Action<T> listener)
        {
            if (eventDictionary.ContainsKey(eventType))
            {
                eventDictionary[eventType] -= listener;
            }
        }
    }
}

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    private List<Enemy> enemies = new List<Enemy>();

    public void Spawn()
    {
        Enemy enemy = Instantiate(enemyPrefab);
        enemy.OnDeath += HandleEnemyDeath;
        enemies.Add(enemy);
    }

    private void HandleEnemyDeath(Enemy enemy)
    {
        enemy.OnDeath -= HandleEnemyDeath;
        enemies.Remove(enemy);
        //Destroy(enemy);
        //Return to objectpool
    }
}


