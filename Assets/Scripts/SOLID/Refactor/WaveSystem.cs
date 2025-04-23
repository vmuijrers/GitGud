using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Builder pattern
/// </summary>
public class WaveSystem
{
    [SerializeField] private GameObject orcPrefab;
    [SerializeField] private GameObject goblinPrefab;
    [SerializeField] private Ability speedBuff;
    [SerializeField] private Ability enragedBuff;
    [SerializeField] private int numOrcs = 10;
    [SerializeField] private int numGoblins = 20;
    [SerializeField] private Vector3 spawnPosition;

    private Wave wave;

    public WaveSystem()
    {
        wave = new WaveBuilder()
            .AddEnemies(orcPrefab, numOrcs, enragedBuff)
            .AddEnemies(goblinPrefab, numGoblins, speedBuff)
            .SetPosition(spawnPosition)
            .SetWaveClearedCallBack((wave) => WaveEnded())
            .Build();

        wave.Spawn();
    }

    public void WaveEnded()
    {
        //Handle level change or something
    }

    public void SpawnWave()
    {
        wave.Spawn();
    }
    
    public class WaveBuilder
    {
        private Wave wave;

        public WaveBuilder()
        {
            wave = new Wave();
        }

        public WaveBuilder AddEnemies(GameObject prefab, int number, params Ability[] abilities)
        {
            wave.WaveData.Add(new WaveData(prefab, number, abilities));
            return this;
        }

        public WaveBuilder SetPosition(Vector3 position)
        {
            wave.SetPosition(position);
            return this;
        }

        public WaveBuilder SetWaveClearedCallBack(System.Action<Wave> WaveCleardCallback = null)
        {
            wave.WaveClearedEvent += WaveCleardCallback;
            return this;
        }

        public Wave Build()
        {
            wave.Setup(); //Do Setup here or anything you need to initialize the wave
            return wave;
        }
    }


    public class Wave
    {
        public List<WaveData> WaveData { get; private set; } = new List<WaveData>();
        public event System.Action<Wave> WaveClearedEvent;
        public Vector3 Position { get; private set; }

        private List<IUnit> spawnedUnits = new List<IUnit>();

        public void Setup()
        {

        }

        public void Spawn()
        {
            foreach(WaveData data in WaveData)
            {
                for(int i = 0; i < data.Amount; i++)
                {
                    GameObject obj = MonoBehaviour.Instantiate(data.Prefab, Position, Quaternion.identity);
                    if(obj.TryGetComponent<IUnit>(out IUnit spawnedUnit))
                    {
                        spawnedUnit.DeadEvent += OnUnitDied;
                        spawnedUnits.Add(spawnedUnit);
                    }
                }
            }
        }

        public void OnUnitDied(IUnit deadUnit)
        {
            spawnedUnits.Remove(deadUnit);
            if(spawnedUnits.Count == 0)
            {
                WaveClearedEvent?.Invoke(this);
            }
        }

        public void SetPosition(Vector3 position)
        {
            Position = position;
        }
    }

    public class WaveData
    {
        public GameObject Prefab { get; private set; }
        public int Amount { get; private set; }
        public Ability[] Abilities { get; private set; }

        public WaveData(GameObject prefab, int amount, Ability[] abilities)
        {
            this.Prefab = prefab;
            this.Amount = amount;
            this.Abilities = abilities;
        }
    }

}

public interface IUnit
{
    event System.Action<IUnit> DeadEvent;
}


/// <summary>
/// Prototype pattern
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IClonable<T>
{
    T Clone();
}

public class TestClass : IClonable<TestClass>
{
    private float hp;
    private List<TestClass> testClasses = new List<TestClass>();

    public TestClass Clone()
    {
        var test = new TestClass();
        test.hp = this.hp;
        test.MemberwiseClone();
        test.testClasses = this.testClasses.ToList();
        return test;
    }
}