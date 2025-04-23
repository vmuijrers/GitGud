using UnityEngine.InputSystem;

namespace ExampleProject
{
    public class InputManager : CustomMonoBehaviour, InputSystem_Actions.IPlayerActions
    {
        public event System.Action SpaceKeyPressedEvent;
        //public InputSystem_Actions asset = new InputSystem_Actions();
        public InputActionReference inputAction;

        protected override void OnEnable()
        {
            base.OnEnable();
            //asset.Enable();
            inputAction.action.performed += Action_performed;
           
        }

        void Update()
        {
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                StartRebinding();
            }
        }

        private void StartRebinding()
        {
            UnityEngine.Debug.Log("Started Rebinding...");
            inputAction.action.Disable();
            inputAction.action.PerformInteractiveRebinding()
                .WithControlsExcluding("Mouse")
                .WithCancelingThrough("<Keyboard>/escape")
                .OnMatchWaitForAnother(0.1f)
                .OnComplete((x) => OnRebindComplete(x))
                .Start();
        }

        private void Action_performed(InputAction.CallbackContext obj)
        {
            UnityEngine.Debug.Log($"Attack button pressed: { obj.action.bindings[0].effectivePath}");
        }

        private void OnRebindComplete(InputActionRebindingExtensions.RebindingOperation rebindingOperation)
        {
            UnityEngine.Debug.Log("Rebinding Completed!");
            inputAction.action.Enable();
            rebindingOperation.Dispose();
        }

        public override void OnUpdate()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                SpaceKeyPressedEvent?.Invoke();
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnPrevious(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnNext(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }
    }

}
