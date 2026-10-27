using System;

namespace Game.Application
{
    public class GameSessionService
    {
        public float ElapsedTime { get; private set; }
        public int FallsCount { get; private set; }
        public bool IsGameRunning { get; private set; }

        public event Action OnGameFinished;

        public void StartSession()
        {
            ElapsedTime = 0f;
            FallsCount = 0;
            IsGameRunning = true;
        }

        public void Tick(float deltaTime)
        {
            if (IsGameRunning) 
            {
                ElapsedTime += deltaTime;
            }
        }

        public void RegisterFall()
        {
            if (IsGameRunning) 
            {
                FallsCount++;
            }
        }

        public void FinishSession()
        {
            if (IsGameRunning) 
            {
                IsGameRunning = false;
                OnGameFinished?.Invoke();
            }
        }
    }
}