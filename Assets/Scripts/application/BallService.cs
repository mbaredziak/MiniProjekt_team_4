using System;
using Game.Domain;

namespace Game.Application
{
    public class BallService
    {
        public BallWeight CurrentWeight { get; private set; } = BallWeight.Wood;
        public event Action<BallWeight> OnWeightChanged;

        public void ChangeWeight(BallWeight newWeight)
        {
            if (CurrentWeight != newWeight) 
            {
                CurrentWeight = newWeight;
                OnWeightChanged?.Invoke(newWeight);
            }
        }
    }
}