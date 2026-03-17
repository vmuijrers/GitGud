using UnityEngine;

namespace RefactoredBomb
{
    public class ObjectSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;

        public void SpawnObject(GameObject target)
        {
            Instantiate(target, transform.position, transform.rotation);
        }

        public void SpawnAssignedObject()
        {
            SpawnObject(prefab);
        }

        public void OnCreateObjectReaction(GameObject sender, GameObject target)
        {
            SpawnObject(target);
        }
    }
}

public class SoundTrigger
{
    public System.Action<float, float> OnValueChanged;

    private float intensity = 0;
    public float Intensity
    {
        get
        {
            return intensity;
        }
        set
        {
            OnValueChanged.Invoke(intensity, value);
            intensity = value;
        }
    }

    private void Start()
    {
        OnValueChanged += (old, newVal) => CheckForTrigger(old, newVal, 0.5f, () => DoSomething());
    }

    public void CheckForTrigger(float oldValue, float newValue, float threshold, System.Action callback = null)
    {
        if(oldValue < threshold && newValue > threshold)
        {
            callback?.Invoke();
        }
    }

    private void DoSomething()
    {
        //Do something
    }
}