using UnityEngine;

namespace ExampleProject
{
    public abstract class CustomMonoBehaviour : MonoBehaviour, IUpdate
    {
        [SerializeField] private UpdateFlags flags;
        public UpdateFlags Flags => flags;

        protected virtual void OnEnable()
        {
            UpdateManager.Subscribe(this);
        }

        protected virtual void OnDisable()
        {
            UpdateManager.UnSubscribe(this);
        }

        public virtual void OnSetup() { }
        public virtual void OnUpdate() { }
    }

}
