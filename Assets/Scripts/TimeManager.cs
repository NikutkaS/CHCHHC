using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    [Header("Настройки петли")]
    public float loopDuration = 10f;
    [SerializeField] private int maxLives = 3;

    [Header("UI (TMP)")]
    public TMP_Text timerText;
    [SerializeField] private Image[] lifeHearts;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Игрок и спавн")]
    public Transform player;
    public Transform spawnPoint;

    [Header("Эхо")]
    public GameObject echoPrefab;

    private float timer;
    private int lives;
    private bool gameOver;

    public float CurrentTime => timer;
    public int CurrentLives => lives;
    public bool IsGameOver => gameOver;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        ResetRunState();
    }

    private void Update()
    {
        if (gameOver)
            return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            HandleTimerExpired();
            return;
        }

        UpdateTimerUI();
    }

    // вызывается при выполнении действия (нажатие кнопки)
    public void ActionCompleted(Vector2 actionPosition)
    {
        if (gameOver)
            return;

        // создаём эхо на месте действия
        if (echoPrefab != null)
            Instantiate(echoPrefab, actionPosition, Quaternion.identity);

        ResetLoop();
    }

    public void BindSceneReferences(TMP_Text newTimerText, Image[] newLifeHearts, GameObject newGameOverPanel, Transform newPlayer, Transform newSpawnPoint)
    {
        timerText = newTimerText;
        lifeHearts = newLifeHearts;
        gameOverPanel = newGameOverPanel;
        player = newPlayer;
        spawnPoint = newSpawnPoint;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(gameOver);

        UpdateTimerUI();
        UpdateLivesUI();
    }

    private void HandleTimerExpired()
    {
        lives--;
        UpdateLivesUI();

        if (lives <= 0)
        {
            TriggerGameOver();
            return;
        }

        ResetLoop();
    }

    // общий сброс петли
    private void ResetLoop()
    {
        MathDoorPuzzle.CloseCurrentOpenPuzzle(true);

        timer = loopDuration;

        // телепорт игрока на старт
        if (player != null && spawnPoint != null)
        {
            player.position = spawnPoint.position;

            // сброс скорости, чтобы не ехал дальше
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = Vector2.zero;
        }

        UpdateTimerUI();
    }

    private void TriggerGameOver()
    {
        MathDoorPuzzle.CloseCurrentOpenPuzzle(true);

        gameOver = true;
        timer = 0f;
        UpdateTimerUI();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        ResetRunState();

        Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.name);
    }

    private void ResetRunState()
    {
        lives = maxLives;
        gameOver = false;
        timer = loopDuration;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        UpdateTimerUI();
        UpdateLivesUI();
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
            timerText.text = Mathf.Max(0f, timer).ToString("F1");
    }

    private void UpdateLivesUI()
    {
        if (lifeHearts == null)
            return;

        for (int i = 0; i < lifeHearts.Length; i++)
        {
            if (lifeHearts[i] != null)
                lifeHearts[i].enabled = i < lives;
        }
    }
}
