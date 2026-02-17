using System.Collections;
using UnityEngine;

public class Enemy : BaseActor, IClickable, IScoreable, ITargetUser //, IDeathable
{
    [SerializeField] private IDamageable target;
    [SerializeField] private float hitDistance = 1;
    [SerializeField] private float sizeIncrease = .2f;
    [SerializeField] private float speedIncrease = .2f;
    [SerializeField] private int damage = 1;
    public event System.Action OnAddScore;
    public event System.Action<Enemy> OnDeath;

    private float size = 1f;

    public override void OnSetup()
    {
        Registry<IClickable>.Register(this);
        Registry<IScoreable>.Register(this);
        Registry<ITargetUser>.Register(this);
        Registry<IUpdateable>.Register(this);
        Registry<IDamageable>.Register(this);
        Respawn();
    }

    private void OnDestroy()
    {
        Registry<IClickable>.UnRegister(this);
        Registry<IScoreable>.UnRegister(this);
        Registry<ITargetUser>.UnRegister(this);
        Registry<IUpdateable>.UnRegister(this);
        Registry<IDamageable>.UnRegister(this);
    }

    public void SetTarget(IDamageable target)
    {
        this.target = target;
    }

    public override void OnUpdate()
    {
        Move();
        CheckForTarget();
    }

    private void CheckForTarget()
    {
        if(target == null) { return; }
        if(Vector3.Distance(target.transform.position, transform.position) < hitDistance)
        {
            if(target.transform.TryGetComponent<IDamageable>(out IDamageable damagable))
            {
                damagable.OnTakeDamage(damage);
                Die();
            }
        }
    }

    public override void OnTakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Debug.Log("Enemy Died!");
            OnAddScore?.Invoke();
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke(this);
        Respawn();
    }

    public Enemy Clone()
    {
        return this.MemberwiseClone() as Enemy;
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public override void Move()
    {
        transform.position += (target.transform.position - transform.position).normalized * Speed * Time.deltaTime;
    }

    public override void Respawn()
    {
        transform.position = NewRandomPosition();
        baseHealth += 1;
        Health = baseHealth;
        Speed += speedIncrease;
        size = baseHealth * sizeIncrease;
        transform.localScale = new Vector3(size, size, size);
    }

    public Vector3 NewRandomPosition()
    {
        if(target == null) { return transform.position; }
        return target.transform.position + Random.insideUnitSphere.WithXYZ(y: 0).normalized * 10;
    }

    public void OnClicked()
    {
        Debug.Log("OnClicked!");
        OnTakeDamage(1);
        StartCoroutine(BumpRoutine(size, size * 1.5f, 0.66f, 0.2f));
    }
    
}

