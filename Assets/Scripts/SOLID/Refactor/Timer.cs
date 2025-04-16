namespace RefactoredBomb
{
    public class Timer
    {
        private float initialTimer = 10;
        private float timeLeft = 0;
        private event System.Action OnTimerDone = null;
        private bool isActive = false;

        public Timer(float initialTimer, bool startImmediately = true, System.Action OnTimerDone = null)
        {
            this.initialTimer = initialTimer;
            this.OnTimerDone = OnTimerDone;
            if (startImmediately)
            {
                ResetTimer();
            }
        }

        public void SetEffectsOnDone(System.Action OnTimerDone)
        {
            this.OnTimerDone = OnTimerDone;
        }

        public void ResetTimer()
        {
            timeLeft = initialTimer;
            isActive = true;
        }

        public void Tick(float deltaTime)
        {
            if (!isActive) return;
            if(timeLeft > 0)
            {
                timeLeft -= deltaTime;
            }
            else
            {
                OnTimerDone?.Invoke();
                isActive = false;
            }
        }
    }

}