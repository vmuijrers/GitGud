using UnityEngine;

namespace ExampleProject
{
    public class GameContext : CustomMonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private InputManager inputManager;

        public override void OnSetup()
        {
            //Jump when the space is pressed
            inputManager.SpaceKeyPressedEvent += player.Jump;
            player.JumpEvent += () => { Debug.Log("Jump!"); };
        }

        private void OnDestroy()
        {
            inputManager.SpaceKeyPressedEvent -= player.Jump;
        }
    }

}
