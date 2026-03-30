using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    [Header("Настройки петли")]
    public float loopDuration = 10f;

    [Header("UI (TMP)")]
    public TMP_Text timerText;

    [Header("Игрок и спавн")]
    public Transform player;
    public Transform spawnPoint;

    [Header("Эхо")]
    public GameObject echoPrefab;

    private float timer;
    public float CurrentTime => timer;

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
        timer = loopDuration;
        UpdateTimerUI();
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            ResetLoop(false);
            return;
        }

        UpdateTimerUI();
    }

    // вызывается при выполнении действия (нажатие кнопки)
    public void ActionCompleted(Vector2 actionPosition)
    {
        // создаём эхо на месте действия
        if (echoPrefab != null)
            Instantiate(echoPrefab, actionPosition, Quaternion.identity);

        ResetLoop(true);
    }

    // общий сброс петли
    private void ResetLoop(bool byAction)
    {
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

    private void UpdateTimerUI()
    {
        if (timerText != null)
            timerText.text = Mathf.Max(0f, timer).ToString("F1");
    }
}
