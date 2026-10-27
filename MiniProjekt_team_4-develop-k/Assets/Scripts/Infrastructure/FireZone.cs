using System.Collections;
using UnityEngine;
using Game.Domain; // Dostęp do BallWeight

namespace Game.Infrastructure
{
    public class FireZone : MonoBehaviour
    {
        [Header("Punkt restartu po spaleniu")]
        public Transform CheckpointResetPoint;

        [Header("Zguba w ogniu")]
        public GameObject FireEffectPrefab;

        [Tooltip("Czas w sekundach na odtworzenie efektu przed teleportacją")]
        public float DelayBeforeReset = 1.0f;

        private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Rigidbody rb = other.GetComponentInParent<Rigidbody>();
        if (rb == null) rb = other.GetComponent<Rigidbody>();
        if (rb == null) return;

        // Sprawdzamy masę kamienia (np. >= 2.0f)
        bool isStone = (rb.mass >= 2.0f);

        if (isStone)
        {
            // KAMIEŃ: Nie dotykamy fizyki! Kula ma po prostu potoczyć się dalej po podłodze под ogniem.
            Debug.Log("Kamień bezpiecznie toczy się przez ogień.");
            return;
        }

        // PAPIER / DREWNO: Odpalamy sekwencję spalenia i restartu
        StartCoroutine(BurnSequence(other.gameObject, rb));
    }

        private IEnumerator BurnSequence(GameObject playerObj, Rigidbody rb)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;

            if (FireEffectPrefab != null)
            {
                GameObject effect = Instantiate(FireEffectPrefab, playerObj.transform.position, Quaternion.identity);
                Destroy(effect, 2f);
            }

            MeshRenderer renderer = playerObj.GetComponentInChildren<MeshRenderer>();
            if (renderer != null)
            {
                renderer.enabled = false;
            }

            yield return new WaitForSeconds(DelayBeforeReset);

            if (CheckpointResetPoint != null)
            {
                playerObj.transform.position = CheckpointResetPoint.position;
            }

            if (renderer != null)
            {
                renderer.enabled = true;
            }
            rb.isKinematic = false;
        }
    }
}