using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Panels")]
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private GameObject winPanel;

    [Header("HUD References")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI deathCounterText;

    [Header("Win Screen References")]
    [SerializeField] private TextMeshProUGUI winStatsText;

    [Header("Gameplay State")]
    private bool isGameActive = false;
    private bool isLevelCompleted = false;
    private float elapsedTime = 0f;
    private int deathCount = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        Time.timeScale = 0f;
        isGameActive = false;
        isLevelCompleted = false;

        if (controlsPanel != null) controlsPanel.SetActive(true);
        if (winPanel != null) winPanel.SetActive(false);

        UpdateTimerUI();
        UpdateDeathUI();
    }

    private void Update()
    {
        if (!isGameActive && !isLevelCompleted)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartGame();
            }
            return;
        }

        if (isGameActive && !isLevelCompleted)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerUI();
        }
    }

    private void StartGame()
    {
        isGameActive = true;
        Time.timeScale = 1f;
        if (controlsPanel != null) controlsPanel.SetActive(false);
    }

    public void AddDeath()
    {
        if (isLevelCompleted) return;

        deathCount++;
        UpdateDeathUI();
    }

    public void CompleteLevel()
    {
        if (isLevelCompleted) return;

        isLevelCompleted = true;
        isGameActive = false;
        Time.timeScale = 0f;

        if (winStatsText != null)
        {
            int minutes = Mathf.FloorToInt(elapsedTime / 60F);
            int seconds = Mathf.FloorToInt(elapsedTime % 60F);
            winStatsText.text = $"Twój czas: {minutes:00}:{seconds:00}\nŚmierci: {deathCount}";
        }

        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void UpdateTimerUI()
    {
        if (timerText == null) return;
        int minutes = Mathf.FloorToInt(elapsedTime / 60F);
        int seconds = Mathf.FloorToInt(elapsedTime % 60F);
        timerText.text = $"Czas: {minutes:00}:{seconds:00}";
    }

    private void UpdateDeathUI()
    {
        if (deathCounterText != null)
        {
            deathCounterText.text = $"Śmierci: {deathCount}";
        }
    }
}