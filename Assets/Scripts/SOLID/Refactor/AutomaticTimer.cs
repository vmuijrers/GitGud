using UnityEngine;
using UnityEngine.Events;

namespace RefactoredBomb
{
    public class AutomaticTimer : MonoBehaviour
    {
        [SerializeField] private float initialTime = 5f;
        private Timer timer;
        public UnityEvent TimerDoneEvent;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            timer = new Timer(initialTime, true, delegate { TimerDoneEvent?.Invoke(); });
        }

        // Update is called once per frame
        void Update()
        {
            timer.Tick(Time.deltaTime);
        }

        public void ResetTimer()
        {
            timer.ResetTimer();
        }
    }
}