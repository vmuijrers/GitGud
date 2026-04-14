using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SOLID
{
    //SOLID
    public class Examples : MonoBehaviour
    {
        List<Enemy> enemyList;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Enemy enemy1 = new Enemy(new Bow());

            enemyList = new List<Enemy>();
            enemyList.Add(new Goblin(new Bow()));
            enemyList.Add(new Orc(new Sword()));

            foreach(var enemy in enemyList)
            {
                enemy.Attack();
                if(enemy is Goblin)
                {
                    ((Goblin)enemy).Scream();
                }
                if (enemy is Orc)
                {
                    ((Orc)enemy).Scream();
                }
            }

            foreach (IScream screamingEnemies in enemyList.OfType<IScream>())
            {
                screamingEnemies.Scream();
            }

            foreach (IAttack attackingEnemies in enemyList.OfType<IAttack>())
            {
                attackingEnemies.Attack();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

    public class Enemy
    {
        public IItem item;

        public Enemy(IItem _item)
        {
            item = _item;
        }

        public virtual void Attack()
        {

        }
    }

    public interface IItem
    {
        void Use();
    }

    public class Apple : IItem
    {
        public void Use()
        {
        }
    }

    public abstract class Weapon : IItem
    {
        public int Damage { get; protected set; }
        public abstract void Attack();
        public abstract void Equip();

        public void Use()
        {
            Equip();
        }
    }

    public class Bow : Weapon
    {
        public override void Attack()
        {
        }

        public override void Equip()
        {
        }
    }

    public class Sword : Weapon
    {
        public override void Attack()
        {
        }

        public override void Equip()
        {
        }
    }

    public class Goblin : Enemy, IAttack, IScream
    {
        public Goblin(IItem _item) : base(_item)
        {
        }

        public override void Attack()
        {

        }

        public void Scream()
        {

        }
    }
    public class Orc : Enemy, IScream, IAttack
    {
        public Orc(IItem _item) : base(_item)
        {
        }

        public override void Attack()
        {

        }

        public void Scream()
        {

        }
    }

    public class Wyvern : Enemy, IAttack
    {
        public Wyvern(IItem _item) : base(_item)
        {
        }

        public override void Attack()
        {

        }
    }

    public interface IScream
    {
        void Scream();
    }

    public interface ITransform
    {
        Transform transform { get; }
    }

    public interface IEnemy : IScream, IAttack
    {
    }

    public interface IAttack
    {
        void Attack();
    }
}

