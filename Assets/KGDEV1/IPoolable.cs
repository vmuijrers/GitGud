using UnityEngine;

public interface IPoolable
{
    bool Active { get; set; }
    void OnEnableObject();
    void OnDisableObject();
}

public abstract class GameObjectEntity : IPoolable, ITransform, IPoolableGameObject
{
    public GameObject GameObjectInstance { get; private set; }
    public GameObject GameObjectPrefab { get; private set; }
    public bool Active { get; set; }

    public Transform transform => GameObjectInstance.transform;

    public GameObjectEntity() { }

    public GameObjectEntity(GameObject gameObjectInstance)
    {
        this.GameObjectInstance = gameObjectInstance;
    }

    public void SetInstance(GameObject instance)
    {
        GameObjectInstance = instance;
    }

    public abstract void OnEnableObject();
    public abstract void OnDisableObject();
}

public class BulletEntity : GameObjectEntity
{
    public BulletEntity() { }

    public BulletEntity(GameObject gameObjectInstance) : base(gameObjectInstance) { }

    public override void OnDisableObject()
    {
        GameObjectInstance.SetActive(false);
    }

    public override void OnEnableObject()
    { 
        if(GameObjectInstance != null)
        {
            GameObjectInstance.SetActive(true);
        }
    }
}

[System.Flags]
public enum Effect // (32 bits)
{
    Nothing     = 0,
    Fire        = 1 << 0, //1
    Poison      = 1 << 1, //2
    Ice         = 1 << 2, //4
    Earth       = 1 << 3, //8
}

// 0011
