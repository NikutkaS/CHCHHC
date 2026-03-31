using UnityEngine;

public class GameOverMenuUI : MonoBehaviour
{
    public void RestartGame()
    {
        if (TimeManager.Instance != null)
            TimeManager.Instance.RestartGame();
    }
}
