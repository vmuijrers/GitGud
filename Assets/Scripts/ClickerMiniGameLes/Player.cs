using UnityEngine;

namespace ClickerExample
{
    public class Player : BaseActor
    {
        public override void Setup()
        {
            Debug.Log("Setup Player!");
            var enemiesWithLowHealth = Registry<Enemy>.Filter((x) => x.Health < 5);
        }

        public override void Move()
        {
        }

        public override void OnClick()
        {
        }
        
    }
}

