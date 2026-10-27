using UnityEngine;
using Game.Application;
using Game.Domain;

namespace Game.Infrastructure
{
    public class Transformer : MonoBehaviour
    {
        public BallWeight TargetWeight;
        
        [Header("Grafika po transformacji")]
        [Tooltip("Wstaw tu swój materiał 'chomik_p'")]
        public Material TargetMaterial; // Dodana zmienna na nowy wygląd

        private BallService m_ballService;

        public void Init(BallService ballService)
        {
            m_ballService = ballService;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                // 1. Wywołujemy istniejącą logikę zmiany wagi
                m_ballService.ChangeWeight(TargetWeight);

                // 2. Szukamy MeshRenderera na kuli (lub jej dzieciach), aby zmienić materiał
                MeshRenderer ballRenderer = other.GetComponent<MeshRenderer>();
                
                // Jeśli renderer jest na obiekcie nadrzędnym lub dziecku, na wszelki wypadek:
                if (ballRenderer == null)
                {
                    ballRenderer = other.GetComponentInChildren<MeshRenderer>();
                }

                // 3. Jeśli znaleźliśmy i przypisano materiał w Inspektorze, zmieniamy go
                if (ballRenderer != null && TargetMaterial != null)
                {
                    ballRenderer.material = TargetMaterial;
                }
            }
        }
    }
}