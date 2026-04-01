using TMPro;
using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private TMP_Text objectiveText;

    private bool isPaused;
    private bool isSubscribed;

    private void Awake()
    {
        TrySubscribe();
    }

    private void OnEnable()
    {
        TrySubscribe();
        RefreshObjectiveText();
    }

    private void OnDisable()
    {
        TryUnsubscribe();
    }

    private void Start()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);

        TrySubscribe();
        RefreshObjectiveText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    public void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (pausePanel != null)
            pausePanel.SetActive(true);

        TrySubscribe();
        RefreshObjectiveText();
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    public void OnContinueButtonPressed()
    {
        ResumeGame();
    }

    private void HandleObjectiveChanged(string newObjective)
    {
        if (objectiveText != null)
            objectiveText.text = newObjective;
    }

    private void TrySubscribe()
    {
        if (isSubscribed)
            return;

        if (LevelObjectiveManager.Instance == null)
            return;

        LevelObjectiveManager.Instance.OnObjectiveChanged += HandleObjectiveChanged;
        isSubscribed = true;
    }

    private void TryUnsubscribe()
    {
        if (!isSubscribed)
            return;

        if (LevelObjectiveManager.Instance != null)
            LevelObjectiveManager.Instance.OnObjectiveChanged -= HandleObjectiveChanged;

        isSubscribed = false;
    }

    private void RefreshObjectiveText()
    {
        if (objectiveText == null)
            return;

        if (LevelObjectiveManager.Instance != null)
            objectiveText.text = LevelObjectiveManager.Instance.CurrentObjective;
        else
            objectiveText.text = string.Empty;
    }
}
