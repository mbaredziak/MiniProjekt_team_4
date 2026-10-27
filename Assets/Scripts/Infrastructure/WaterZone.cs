using UnityEngine;
using Game.Domain;

namespace Game.Infrastructure
{
    public class WaterZone : MonoBehaviour
    {
        [Header("Ustawienia wyporu wody")]
        [Tooltip("Siła, z jaką woda wypycha papierową kulę w górę")]
        public float BuoyancyForce = 15f; 

        [Tooltip("Wysokość powierzchni wody (pozycja Y)")]
        public float WaterLevelY = 0.31f;

        private void OnTriggerStay(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb == null) return;

            // Sprawdzamy, czy to papierowa kula (masa <= 0.5f)
            bool isPaper = rb.mass <= 0.5f;

            if (isPaper)
            {
                // Jeśli kula jest poniżej poziomu wody, wypychamy ją w górę
                if (other.transform.position.y < WaterLevelY)
                {
                    // Zerujemy opadanie i dodajemy siłę wyporu w górę (Vector3.up)
                    Vector3 force = Vector3.up * BuoyancyForce;
                    rb.AddForce(force, ForceMode.Acceleration);
                    
                    // Lekki opór wody, żeby nie skakała jak szalona
                    rb.linearDamping = 2f; 
                }
            }
            else
            {
                // Kamienna i zwykła kula toną bez przeszkód
                rb.linearDamping = 0.5f;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Rigidbody rb = other.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // Przywracamy opór powietrza po wyjściu z wody
                    rb.linearDamping = 0f;
                }
            }
        }
    }
}