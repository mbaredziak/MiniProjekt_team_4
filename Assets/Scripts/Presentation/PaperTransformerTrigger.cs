using UnityEngine;

namespace Game.Presentation
{
    public class PaperTransformerTrigger : MonoBehaviour
    {
        [Header("Materiał do nałożenia")]
        [SerializeField] private Material paperMaterial;

        private void OnTriggerEnter(Collider other)
        {
            // Sprawdzamy, czy w strefę wjechał gracz (kula)
            if (other.CompareTag("Player") || other.GetComponent<MeshRenderer>() != null)
            {
                MeshRenderer playerRenderer = other.GetComponent<MeshRenderer>();
                
                if (playerRenderer != null && paperMaterial != null)
                {
                    // Zmieniamy materiał kuli na papierowy
                    playerRenderer.material = paperMaterial;
                    
                    // (Opcjonalnie) Zmniejszamy masę kuli, żeby była lekka jak papier
                    Rigidbody rb = other.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.mass = 0.5f; // Domyślna masa to np. 1.0f
                    }
                }
            }
        }
    }
}