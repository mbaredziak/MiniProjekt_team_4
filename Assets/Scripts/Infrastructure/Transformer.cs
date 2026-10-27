using UnityEngine;
using Game.Application;
using Game.Domain;

namespace Game.Infrastructure
{
    public class Transformer : MonoBehaviour
    {
        public BallWeight TargetWeight;
        private BallService m_ballService;

        public void Init(BallService ballService)
        {
            m_ballService = ballService;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                m_ballService.ChangeWeight(TargetWeight);
            }
        }
    }
}