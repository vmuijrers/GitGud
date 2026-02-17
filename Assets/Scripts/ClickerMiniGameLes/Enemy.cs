using UnityEngine;

namespace ClickerExample
{
    public class Enemy : BaseActor
    {
        public override void Setup()
        {
            Debug.Log("Setup Enemy!");
            Registry<Enemy>.Register(this);
            Registry<IClickable>.Register(this);
        }

        public void OnDestroy()
        {
            Registry<Enemy>.UnRegister(this);
            Registry<IClickable>.UnRegister(this);
        }

        public override void Move()
        {

        }

        public override void OnClick()
        {
            // Take damage
            Debug.Log("Enemy was Clicked");

        }
    }
}

