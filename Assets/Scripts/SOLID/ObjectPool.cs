using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool<T> where T : IPoolable, IPoolableGameObject
{
    private List<T> activePool = new List<T>();
    private List<T> inactivePool = new List<T>();

    private GameObject prefab;

    public ObjectPool(GameObject prefab, int amount)
    {
        this.prefab = prefab;
        for (int i = 0; i < amount; i++)
        {
            T item = SpawnNewObject(prefab);
            inactivePool.Add(item);
        }
    }

    public void ReturnToPool(T item)
    {
        item.Active = false;
        item.OnDisableObject();
        activePool.Remove(item);
        inactivePool.Add(item);
    }

    public T RequestFromPool()
    {
        T item = default(T);
        if (inactivePool.Count > 0)
        {
            item = inactivePool.First();
            ActivateObject(item);
            return item;
        }
        item = SpawnNewObject(prefab);
        ActivateObject(item);
        return item;
    }

    private T SpawnNewObject(GameObject prefab)
    {
        T item = Activator.CreateInstance<T>();
        item.Active = false;
        var obj = MonoBehaviour.Instantiate(prefab);
        obj.SetActive(false);
        item.SetInstance(obj);
        return item;
    }

    private void ActivateObject(T item)
    {
        item.Active = true;
        item.OnEnableObject();
        activePool.Add(item);
        inactivePool.Remove(item);
    }
}

public interface IPoolableGameObject
{
    void SetInstance(GameObject prefab);
}