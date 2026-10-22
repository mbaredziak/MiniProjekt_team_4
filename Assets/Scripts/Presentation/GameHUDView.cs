using UnityEngine;
using TMPro;
using Game.Domain;
using Game.Composition;

namespace Game.Presentation
{
    public class GameHUDView : MonoBehaviour
    {
        [Header("Pola tekstowe (TextMeshPro)")]
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI fallsCountText;
        [SerializeField] private TextMeshProUGUI ballWeightText;

        [Header("Panel końca gry (opcjonalny)")]
        [SerializeField] private GameObject victoryPanel;
        [SerializeField] private TextMeshProUGUI finalStatsText;

        private GameBootstrap _bootstrap;

        private void Start()
        {
            _bootstrap = FindAnyObjectByType<GameBootstrap>();
            if (_bootstrap == null)
            {
                Debug.LogError("[GameHUDView] Nie znaleziono GameBootstrap na scenie!");
                return;
            }

            // Subskrypcja zmiany wagi z BallService
            if (_bootstrap.BallService != null)
            {
                _bootstrap.BallService.OnWeightChanged += HandleWeightChanged;
                UpdateWeightDisplay(_bootstrap.BallService.CurrentWeight);
            }

            // Subskrypcja ukończenia gry z serwisu GameSession
            if (_bootstrap.GameSession != null)
            {
                _bootstrap.GameSession.OnGameFinished += HandleGameFinished;
            }

            if (victoryPanel != null)
            {
                victoryPanel.SetActive(false);
            }
        }

        private void Update()
        {
            if (_bootstrap == null || _bootstrap.GameSession == null) return;

            // Pobieranie czasu i liczby upadków z serwisu GameSession
            if (_bootstrap.GameSession.IsGameRunning)
            {
                UpdateTimerDisplay(_bootstrap.GameSession.ElapsedTime);
                UpdateFallsDisplay(_bootstrap.GameSession.FallsCount);
            }
        }

        private void OnDestroy()
        {
            if (_bootstrap != null)
            {
                if (_bootstrap.BallService != null)
                {
                    _bootstrap.BallService.OnWeightChanged -= HandleWeightChanged;
                }

                if (_bootstrap.GameSession != null)
                {
                    _bootstrap.GameSession.OnGameFinished -= HandleGameFinished;
                }
            }
        }

        private void HandleWeightChanged(BallWeight newWeight)
        {
            UpdateWeightDisplay(newWeight);
        }

        private void UpdateWeightDisplay(BallWeight weight)
        {
            if (ballWeightText == null) return;

            string weightName = weight switch
            {
                BallWeight.Wood => "Drewno",
                BallWeight.Stone => "Kamień",
                BallWeight.Paper => "Papier",
                _ => weight.ToString()
            };

            ballWeightText.text = $"Waga: {weightName}";
        }

        private void UpdateTimerDisplay(float timeInSeconds)
        {
            if (timerText == null) return;

            int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
            int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
            int milliseconds = Mathf.FloorToInt((timeInSeconds * 100f) % 100f);

            timerText.text = $"Czas: {minutes:00}:{seconds:00}.{milliseconds:00}";
        }

        private void UpdateFallsDisplay(int falls)
        {
            if (fallsCountText == null) return;
            fallsCountText.text = $"Upadki: {falls}";
        }

        private void HandleGameFinished()
        {
            if (victoryPanel != null)
            {
                victoryPanel.SetActive(true);
            }

            if (finalStatsText != null && _bootstrap.GameSession != null)
            {
                float time = _bootstrap.GameSession.ElapsedTime;
                int falls = _bootstrap.GameSession.FallsCount;
                int minutes = Mathf.FloorToInt(time / 60f);
                int seconds = Mathf.FloorToInt(time % 60f);

                finalStatsText.text = $"Koniec gry!\nCzas: {minutes:00}:{seconds:00}\nUpadki: {falls}";
            }
        }
    }
}