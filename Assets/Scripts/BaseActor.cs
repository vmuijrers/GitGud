using System.Linq;
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

//public class SomeClass1
//{
//    public int someInt = 3;

//    public void Update()
//    {
//        if (!enabled) return;
//        Actions.DoJump(GameObject, height, speed);

//    }

//    public bool IsItTrue()
//    {
//        return true;
//    }

//    public void DoSomething(int _someInt)
//    {
//        if (_someInt < 0
//            && IsItTrue()) { return; }
//        if (_someInt > 100) return;

//        someInt = _someInt;

//        for (int i = 0; i < 10; i++)
//        {
//            for (int j = 0; j < 10; j++)
//            {
//                Grid[i, j].Update();
//                if (i > 5)
//                {
//                    continue;
//                }

//                return;
//            }
//            break;
//        }
//    }
//}

//public static class Actions
//{
//    public static void DoJump(GameObject go, float height, float speed)
//    {
//        Do the jump
//    }
//}

//public interface IHittable
//{
//    void OnHit();
//}

//public class Player1
//{
//    private IHittable[] hittable;

//    public Player1(IHittable[] hittable)
//    {
//        this.hittable = hittable;

//        foreach (var hit in hittable)
//        {
//            hit.OnHit();
//        }
//    }
//}

//public class GameManager
//{
//    public Animal[] kipjes;
//    void Main()
//    {
//        Player1 = new Player1(new Shield(), new Gun());
//        foreach (IMovable c in kipjes.OfType<IMovable>())
//        {
//            c.Move();
//        }
//    }

//    public void LoadLevel()
//    {

//    }
//}

//public class Shield : IHittable
//{
//    public void OnHit()
//    {
//        throw new System.NotImplementedException();
//    }
//}

//public class Gun : IHittable
//{
//    public void OnHit()
//    {
//        throw new System.NotImplementedException();
//    }
//}

//public abstract class Animal : IHittable
//{
//    public abstract void OnHit();
//}

//public interface IMovable
//{
//    void Move();
//}

//public class Cow : Animal, IMovable
//{
//    public override void OnHit()
//    {

//    }

//    public void Move() { }
//}


//public class Bomb : MonoBehaviour
//{
//    public float maxTimer;
//    private float currentTimer;

//    private bool isActive = false;
//    [SerializeField] private bool activeAtStart = true;

//    / <summary>
//    / 
//    / </summary>
//    public void Start()
//    {
//        SetActive(activeAtStart);
//    }

//    / <summary>
//    / 
//    / </summary>
//    / <param name = "value" ></ param >
//    public void SetActive(bool value)
//    {
//        isActive = value;
//        if (isActive)
//        {

//        }
//    }

//    / <summary>
//    / 
//    / </summary>
//    public void ResetTimer()
//    {
//        currentTimer = maxTimer;
//    }

//    / <summary>
//    / 
//    / </summary>
//    public void Update()
//    {
//        if (isActive)
//        {
//            currentTimer -= Time.deltaTime;
//            if (currentTimer <= 0)
//            {
//                Explode();
//            }
//        }
//    }

//    / <summary>
//    / 
//    / </summary>
//    public void Explode()
//    {
//        Debug.Log("Boom!");
//        Destroy(gameObject);
//    }
//}

//public class Enemy : MonoBehaviour
//{
//    public FSM fsm;
//    public AttackComponent attack;
//    public HealthComponent health;



//    public void Setup()
//    {
//        fsm = new FSM();
//        IdleState state = new IdleState(this);
//    }

//}