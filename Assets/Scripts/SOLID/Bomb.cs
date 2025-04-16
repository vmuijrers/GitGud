using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// This is a Composite Bomb, all functionality is in one class, very inflexible
/// </summary>
public class Bomb : MonoBehaviour
{
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

    void Update()
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
        }
            
        Destroy(gameObject);
    }
}


public class BombSOLID : MonoBehaviour
{
    [SerializeField] private float radius = 5;
    [SerializeField] private List<MonoBehaviour> effects = new List<MonoBehaviour>();
    private List<IExplosionEffect> explosionEffects = new List<IExplosionEffect>();

    private void Start()
    {
        explosionEffects = effects.OfType<IExplosionEffect>().ToList();    
    }

    private void Explode()
    {
        foreach(var effect in explosionEffects)
        {
            effect.ApplyEffect(transform.position, radius);
        }
    }
}

public class DamageEffect : MonoBehaviour, IExplosionEffect
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
    public RefactoredBomb.Timer timer { get; set; }
    public event System.Action ExplosionEffect = null;
    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }

    private void Setup()
    {

    }

    public void Tick(float deltaTime)
    {
        timer.Tick(deltaTime);
    }

    public class BombBuilder
    {
        private float damage = 100;
        private float radius = 5;
        private LayerMask hitLayer;
        private System.Action explosionEffect = null;
        private RefactoredBomb.Timer timer = null;
        private Vector3 position;
        private Quaternion rotation;

        public BombBuilder SetPosition(Vector3 position)
        {
            this.position = position;
            return this;
        }

        public BombBuilder SetRotation(Quaternion rotation)
        {
            this.rotation = rotation;
            return this;
        }

        public BombBuilder SetDamage(float damage)
        {
            this.damage = damage;
            return this;
        }

        public BombBuilder SetRadius(float radius)
        {
            this.radius = radius;
            return this;
        }

        public BombBuilder SetHitLayer(LayerMask layerMask)
        {
            this.hitLayer = layerMask;
            return this;
        }

        public BombBuilder SetTimer(float duration)
        {
            this.timer = new RefactoredBomb.Timer(duration);
            return this;
        }

        public BombBuilder AddReactionToExplosion(System.Action effect)
        {
            this.explosionEffect += effect;
            return this;
        }

        public BombExtended Build()
        {
            BombExtended newBomb = new BombExtended();
            newBomb.Damage = damage;
            newBomb.Radius = radius;
            newBomb.HitLayer = hitLayer;
            newBomb.ExplosionEffect = explosionEffect;
            timer.SetEffectsOnDone(explosionEffect);
            newBomb.timer = timer;
            newBomb.Position = position;
            newBomb.Rotation = rotation;
            explosionEffect = null;
            return newBomb;
        }
    }

}