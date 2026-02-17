using UnityEngine;

namespace ClickerExample
{
    public class Player : BaseActor
    {
        public override void Setup()
        {
            Debug.Log("Setup Player!");
            var enemiesWithLowHealth = Registry<Enemy>.Filter((x) => x.Health < 5);
            Registry<IClickable>.Register(this);
        }

        private void OnDestroy()
        {
            Registry<IClickable>.UnRegister(this);
        }

        public override void Move()
        {
        }

        public override void OnClick()
        {
            Debug.Log("Player was Clicked");
        }
        
    }
}

