using UnityEngine;

public interface IClickable : ITransform
{
    void OnClicked();
}

public interface ITransform
{
    Transform transform { get; }
}

public interface IScoreable
{
    event System.Action OnAddScore;
}

public interface IDeathable
{
    event System.Action OnDeath;
}

public interface IPlayer : IDeathable, ITransform { }

public interface ISetupable
{
    void OnSetup() { }
}

public interface IUpdateable
{
    void OnUpdate() { }
}

public interface IDamageable
{
    int Health { get; }
    void OnTakeDamage();
}