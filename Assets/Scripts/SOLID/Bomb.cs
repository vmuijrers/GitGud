using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

/// <summary>
/// This is a Composite Bomb, all functionality is in one class, very inflexible
/// </summary>
public class Bomb : MonoBehaviour
{
    public float SomeFLOAT { get; set; }

    [SerializeField] private LayerMask hitLayer;
    [SerializeField] private float knockbackForce = 300; 
    [SerializeField] private int damage = 100;
    [SerializeField] private float hitRadius = 5f;
    [SerializeField] private float initialTimer = 10;

    private float timeLeft;

    void Start()
    {
        timeLeft = initialTimer;
    }

    private void Update()
    {
        if(timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
        }
        else
        {
            Explode();
        }
    }
    
    public void Explode()
    {
        Debug.Log("Boom!");
        var cols = Physics.OverlapSphere(transform.position, hitRadius, hitLayer);
        foreach(Collider c in cols)
        {
            if(c.TryGetComponent(out Rigidbody rb))
            {
                rb.AddExplosionForce(knockbackForce, transform.position, hitRadius);
            }
            if (c.TryGetComponent(out Health health))
            {
                health.TakeDamage(damage);
            }

            //sound?
        }

        //Destroy(gameObject);
    }
}


public class BombSOLID : MonoBehaviour
{
    [SerializeField] private float radius = 5;
    [SerializeField] private List<MonoBehaviour> effects = new List<MonoBehaviour>();

    private RefactoredBomb.Timer bombTimer;

    private void Start()
    {
        bombTimer = new RefactoredBomb.Timer(3, true, Explode);
    }

    private void Update()
    {
        bombTimer.Tick(Time.deltaTime);
    }

    private void Explode()
    {
        foreach(var effect in effects.OfType<IExplosionEffect>())
        {
            effect.ApplyEffect(transform.position, radius);
        }
    }
}

public class AreaDamageEffect : MonoBehaviour, IExplosionEffect
{
    [SerializeField] private float damage = 100;
    [SerializeField] private LayerMask hitLayer;

    public void ApplyEffect(Vector3 explosionPosition, float radius)
    {
        var cols = Physics.OverlapSphere(explosionPosition, radius, hitLayer);
        foreach(var col in cols)
        {
            if(col.TryGetComponent(out RefactoredBomb.IDamageable dam))
            {
                dam.TakeDamage(damage);
            }
        }
    }
}


public interface IExplosionEffect
{
    void ApplyEffect(Vector3 explosionPosition, float radius);
}


public class BombExtended
{
    public float Damage { get; set; }
    public float Radius { get; set; }
    public LayerMask HitLayer { get; set; }
    public RefactoredBomb.Timer Timer { get; set; }
    public event System.Action ExplosionEffect = null;
    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }

    private void Setup()
    {
        BombExtended bomb = new BombExtended()
        {
            Damage = 100,
            Radius = 10
        };
    }

    public void Tick(float deltaTime)
    {
        Timer.Tick(deltaTime);
    }

    public class BombBuilder
    {
        //private float damage = 100;
        //private float radius = 5;
        //private LayerMask hitLayer;
        //private System.Action explosionEffect = null;
        //private RefactoredBomb.Timer timer = null;
        //private Vector3 position;
        //private Quaternion rotation;

        private BombExtended bomb = new BombExtended();

        public BombBuilder SetPosition(Vector3 position)
        {
            bomb.Position = position;
            return this;
        }

        public BombBuilder SetRotation(Quaternion rotation)
        {
            bomb.Rotation = rotation;
            return this;
        }

        public BombBuilder SetDamage(float damage)
        {
            bomb.Damage = damage;
            return this;
        }

        public BombBuilder SetRadius(float radius)
        {
            bomb.Radius = radius;
            return this;
        }

        public BombBuilder SetHitLayer(LayerMask layerMask)
        {
            bomb.HitLayer = layerMask;
            return this;
        }

        public BombBuilder SetTimer(float duration)
        {
            bomb.Timer = new RefactoredBomb.Timer(duration);
            return this;
        }

        public BombBuilder AddReactionToExplosion(System.Action effect)
        {
            bomb.ExplosionEffect += effect;
            return this;
        }

        public BombExtended Build()
        {
            bomb.Setup();
            return bomb;
        }
    }

}