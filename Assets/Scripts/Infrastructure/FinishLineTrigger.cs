using UnityEngine;
using Game.Composition;

namespace Game.Infrastructure
{
    [RequireComponent(typeof(BoxCollider))]
    public class FinishLineTrigger : MonoBehaviour
    {
        private bool _isFinished = false;

        private void Awake()
        {
            GetComponent<BoxCollider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isFinished) return;

            if (other.CompareTag("Player"))
            {
                var bootstrap = FindAnyObjectByType<GameBootstrap>();
                if (bootstrap != null && bootstrap.GameSession != null)
                {
                    bootstrap.GameSession.FinishSession();
                    _isFinished = true;
                    Debug.Log("[FinishLineTrigger] Poziom ukończony!");
                }
            }
        }
    }
}