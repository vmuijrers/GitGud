using UnityEngine.InputSystem;

namespace ExampleProject
{
    public class InputManager : CustomMonoBehaviour
    {
        public event System.Action SpaceKeyPressedEvent;

        public override void OnUpdate()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                SpaceKeyPressedEvent?.Invoke();
            }
        }
    }

}
