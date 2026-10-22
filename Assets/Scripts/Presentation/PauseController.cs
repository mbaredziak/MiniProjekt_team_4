using UnityEngine;

namespace Game.Presentation
{
    public class PauseController : MonoBehaviour
    {
        [Header("UI Pauzy")]
        [Tooltip("Obiekt Panelu w Canvasie, który ma się pokazywać po zatrzymaniu gry")]
        [SerializeField] private GameObject pausePanel;

        private bool _isPaused = false;

        private void Start()
        {
            if (pausePanel != null)
            {
                pausePanel.SetActive(false);
            }
            Time.timeScale = 1f;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }
        }

        public void TogglePause()
        {
            _isPaused = !_isPaused;

            // Zatrzymanie lub wznowienie upływu czasu fizyki w grze
            Time.timeScale = _isPaused ? 0f : 1f;

            if (pausePanel != null)
            {
                pausePanel.SetActive(_isPaused);
            }
        }

        public void ResumeGame()
        {
            _isPaused = false;
            Time.timeScale = 1f;

            if (pausePanel != null)
            {
                pausePanel.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            // Przywracamy normalną prędkość gry przy przeładowaniu sceny
            Time.timeScale = 1f;
        }
    }
}