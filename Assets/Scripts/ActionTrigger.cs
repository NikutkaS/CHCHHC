using UnityEngine;

public class ActionTrigger : MonoBehaviour
{
    [Header("Level1")]
    [SerializeField] private Door doorToOpen;

    [Header("Level2")]
    [SerializeField] private MathDoorPuzzle puzzleToOpen;

    private bool activated;

    public void Activate()
    {
        if (activated)
            return;

        if (puzzleToOpen != null)
        {
            activated = true;
            puzzleToOpen.OpenPuzzle(transform.position, this);
            return;
        }

        activated = true;

        if (doorToOpen != null)
            doorToOpen.Open();

        if (TimeManager.Instance != null)
            TimeManager.Instance.ActionCompleted(transform.position);
        else
            Debug.LogError("TimeManager.Instance is null! Добавь TimeManager на сцену.");

        gameObject.SetActive(false);
    }

    public void ResetTrigger()
    {
        activated = false;
        gameObject.SetActive(true);
    }

    public void CompleteTrigger()
    {
        activated = true;
        gameObject.SetActive(false);
    }
}
