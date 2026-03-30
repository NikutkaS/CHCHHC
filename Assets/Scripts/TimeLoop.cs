using UnityEngine;
using UnityEngine.UI; 

public class TimeLoop : MonoBehaviour
{
    public float loopDuration = 10f;
    private float timer;

    public Text timerText; 

    void Start()
    {
        timer = loopDuration;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timerText != null)
            timerText.text = timer.ToString("F1");

        if (timer <= 0)
        {
            RestartLoop();
        }
    }

    void RestartLoop()
    {
        Debug.Log("⏳ Петля сброшена!");
        timer = loopDuration;
    }
}
