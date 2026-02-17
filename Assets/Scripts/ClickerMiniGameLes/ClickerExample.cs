using System.Linq;
using UnityEngine;

namespace ClickerExample
{
    // Clicker Gamemanager
    public class ClickerExample : MonoBehaviour
    {
        [SerializeField] private GameObject EnemyPrefab;
        [SerializeField] private GameObject PlayerPrefab;
        [SerializeField] private Spawner spawner;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            spawner.Spawn(PlayerPrefab, 1, new Vector2Int(0, 1), DoSetup);
            spawner.Spawn(EnemyPrefab, 3, new Vector2Int(10, 10), DoSetup);

            MonoBehaviour[] monoBehaviours = GameObject.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
            
            foreach(ISetupable setupable in monoBehaviours.OfType<ISetupable>())
            {
                setupable.Setup();
            }
        }

        private void DoSetup(GameObject obj)
        {
            obj.GetComponent<ISetupable>()?.Setup();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var items = Registry<IClickable>.Filter((x) => Vector3.Distance(Input.mousePosition, x.transform.position) < 1);
                foreach(var item in items)
                {
                    item?.OnClick();
                }
            }
        }
    }

    public interface ISetupable
    {
        void Setup();
    }

    public interface IClickable : ITransform
    {
        void OnClick();
    }

    public interface ITransform
    {
        Transform transform { get; }
    }

    public interface IDamageable
    {
        void OnTakeDamage(int damage);
    }

}

