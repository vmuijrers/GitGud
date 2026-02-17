using UnityEngine;

public class Player : BaseActor, IClickable, IDamageable, IDeathable, IPlayer
{
    public event System.Action OnDeath;

    public override void OnSetup()
    {
        Registry<IPlayer>.Register(this);
        Registry<IClickable>.Register(this);
        Registry<IDamageable>.Register(this);
        Registry<IDeathable>.Register(this);
        Registry<IUpdateable>.Register(this);
    }

    private void OnDestroy()
    {
        Registry<IPlayer>.UnRegister(this);
        Registry<IClickable>.UnRegister(this);
        Registry<IDamageable>.UnRegister(this);
        Registry<IDeathable>.UnRegister(this);
        Registry<IUpdateable>.UnRegister(this);
    }

    public override void OnUpdate()
    {
        Move();
    }

    public override void OnTakeDamage(int damage)
    {
        Debug.Log("Player Lost health");

        Health -= damage;
        if (Health <= 0)
        {
            Debug.Log("Player Died!");
            OnDeath?.Invoke();
        }
    }

    public override void Respawn()
    {
        transform.position = Vector3.zero;
        Health = baseHealth;
    }

    public override void Move()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        transform.position += new Vector3(input.x, 0, input.y) * Speed * Time.deltaTime;
    }

    public void OnClicked()
    {
        Health += 1;
        Debug.Log($"Player Clicked, health: {Health}");
        StartCoroutine(BumpRoutine(1f, 1.5f, 1f, 0.2f));
    }
}
