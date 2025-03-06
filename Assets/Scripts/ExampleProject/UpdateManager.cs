using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace ExampleProject
{

    public class UpdateManager : MonoBehaviour
    {
        [SerializeField] private UpdateFlags updateFlags;
        private static List<IUpdate> updateList = new List<IUpdate>();

        public static void Subscribe(IUpdate updateListener)
        {
            updateList.Add(updateListener);
        }

        public static void UnSubscribe(IUpdate updateListener)
        {
            updateList.Remove(updateListener);
        }

        private void Start()
        {
            var objs = FindObjectsByType<CustomMonoBehaviour>(FindObjectsSortMode.None);
            foreach (var obj in objs)
            {
                obj.OnSetup();
            }
        }

        private void Update()
        {
            updateList.ForEach(x =>
            {
                if ((x.Flags & updateFlags) != 0)
                {
                    x.OnUpdate();
                }
            }
            );
        }
    }

}
