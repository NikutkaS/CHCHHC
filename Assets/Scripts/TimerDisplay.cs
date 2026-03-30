using TMPro;
using UnityEngine;

public class TimerDisplay : MonoBehaviour
{
    public TimeManager timeManager;
    public TMP_Text timerText; // вместо Text

    void Update()
    {
        if (timeManager != null && timerText != null)
        {
            timerText.text = timeManager.CurrentTime.ToString("F1") + "s";
        }
    }
}
