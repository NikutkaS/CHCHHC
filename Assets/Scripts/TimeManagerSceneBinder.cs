using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeManagerSceneBinder : MonoBehaviour
{
    [Header("UI сцены")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private Image[] lifeHearts;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Игрок сцены")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform spawnPoint;

    private void Start()
    {
        if (TimeManager.Instance != null)
            TimeManager.Instance.BindSceneReferences(timerText, lifeHearts, gameOverPanel, player, spawnPoint);
    }
}
