using UnityEngine;
using Game.Application;
using Game.Composition;

namespace Game.Infrastructure
{
    public class FallZone : MonoBehaviour
    {
        private CheckpointService m_checkpointService;
        private BallService m_ballService;
        private Rigidbody m_rigidBody;

        public void Init(CheckpointService checkpointService, BallService ballService, Rigidbody rigidBody)
        {
            m_checkpointService = checkpointService;
            m_ballService = ballService;
            m_rigidBody = rigidBody;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                // Dopisane: zliczanie upadku w GameSessionService
                var bootstrap = FindAnyObjectByType<GameBootstrap>();
                if (bootstrap != null && bootstrap.GameSession != null)
                {
                    bootstrap.GameSession.RegisterFall();
                }

                var data = m_checkpointService.CurrentCheckpoint;
                
                m_rigidBody.linearVelocity = Vector3.zero;
                m_rigidBody.angularVelocity = Vector3.zero;
                m_rigidBody.transform.position = new Vector3(data.X, data.Y, data.Z);
                
                m_ballService.ChangeWeight(data.SavedWeight);
            }
        }
    }
}