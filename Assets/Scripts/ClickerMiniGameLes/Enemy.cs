using UnityEngine;

namespace ClickerExample
{
    public class Enemy : BaseActor
    {
        public override void Setup()
        {
            Debug.Log("Setup Enemy!");
            Registry<Enemy>.Register(this);
        }

        public void OnDestroy()
        {
            Registry<Enemy>.UnRegister(this);
        }

        public override void Move()
        {

        }

        public override void OnClick()
        {
            // Take damage
        }
    }
}

