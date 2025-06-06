﻿using UnityEngine;

public class Enemy : BaseActor, IClickable, IScoreable //, IDeathable
{
    [SerializeField] private Transform target;

    public event System.Action OnAddScore;
    public event System.Action<Enemy> OnDeath;

    public override void OnSetup()
    {
        Respawn();
    }

    public override void OnUpdate()
    {
        Move(); 
    }

    public override void OnTakeDamage()
    {
        Health -= 1;
        if (Health <= 0)
        {
            Debug.Log("Enemy Died!");
            OnDeath?.Invoke(this);
            OnAddScore?.Invoke();
            Respawn();
        }
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
        transform.position += (target.position - transform.position).normalized * Speed * Time.deltaTime;
    }

    public override void Respawn()
    {
        transform.position = NewRandomPosition();
        Health = baseHealth;
    }

    public Vector3 NewRandomPosition()
    {
        return new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
    }

    public void OnClicked()
    {
        Debug.Log("OnClicked!");
        OnTakeDamage();
    }

}
