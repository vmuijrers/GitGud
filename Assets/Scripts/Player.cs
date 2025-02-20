using UnityEngine;

public class Player : BaseActor, IClickable, IDamageable, IDeathable, IPlayer
{
    public event System.Action OnDeath;

    public override void OnUpdate()
    {
        Move();
    }

    public override void OnTakeDamage()
    {
        Debug.Log("Player Lost health");

        Health -= 1;
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
    }
}
