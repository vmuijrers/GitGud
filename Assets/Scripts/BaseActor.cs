using UnityEngine;

public abstract class BaseActor : MonoBehaviour, IDamageable, IUpdateable, ISetupable
{
    [SerializeField] private float speed = 3;
    public float Speed { get => speed; protected set => speed = value; }

    private int health;
    public int Health { get => health; set => health = value; }

    [SerializeField] protected int baseHealth = 5;

    public abstract void Respawn();
    public abstract void OnTakeDamage();
    public abstract void Move();

    public virtual void OnSetup() { }
    public virtual void OnUpdate() { }
}
