using UnityEngine;
using Game.Domain;
using Game.Composition;

namespace Game.Infrastructure
{
    [RequireComponent(typeof(BoxCollider))]
    public class CheckpointTrigger : MonoBehaviour
    {
        [Header("Konfiguracja")]
        [Tooltip("Czy checkpoint może być aktywowany tylko raz?")]
        [SerializeField] private bool triggerOnlyOnce = true;

        [Header("Opcjonalny punkt respawnu")]
        [SerializeField] private Transform customSpawnPoint;

        private BoxCollider _boxCollider;
        private bool _isActivated = false;

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider>();
            _boxCollider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isActivated && triggerOnlyOnce) return;

            if (other.CompareTag("Player"))
            {
                TriggerCheckpoint(other.gameObject);
            }
        }

        private void TriggerCheckpoint(GameObject playerObject)
        {
            GameBootstrap bootstrap = FindAnyObjectByType<GameBootstrap>();
            if (bootstrap == null)
            {
                Debug.LogError("[CheckpointTrigger] Brak GameBootstrap na scenie!");
                return;
            }

            if (bootstrap.BallService == null || bootstrap.CheckpointService == null)
            {
                Debug.LogError("[CheckpointTrigger] Serwisy w GameBootstrap nie zostały jeszcze zainicjalizowane!");
                return;
            }

            Vector3 spawnPos = customSpawnPoint != null ? customSpawnPoint.position : playerObject.transform.position;
            BallWeight currentWeight = bootstrap.BallService.CurrentWeight;

            CheckpointData checkpointData = new CheckpointData
            {
                X = spawnPos.x,
                Y = spawnPos.y,
                Z = spawnPos.z,
                SavedWeight = currentWeight
            };

            bootstrap.CheckpointService.SaveCheckpoint(checkpointData);
            _isActivated = true;

            Debug.Log($"[CheckpointTrigger] Checkpoint zapisany: ({checkpointData.X:F2}, {checkpointData.Y:F2}, {checkpointData.Z:F2}), Waga: {checkpointData.SavedWeight}");
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = _isActivated ? new Color(0f, 1f, 0f, 0.35f) : new Color(0f, 0.7f, 1f, 0.35f);
            BoxCollider col = GetComponent<BoxCollider>();
            if (col != null)
            {
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.DrawCube(col.center, col.size);
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireCube(col.center, col.size);
            }
        }
    }
}