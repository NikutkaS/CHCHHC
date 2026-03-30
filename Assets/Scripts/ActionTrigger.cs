using UnityEngine;

public class ActionTrigger : MonoBehaviour
{
    [Header("Связанные объекты")]
    [SerializeField] private Door doorToOpen;

    private bool activated;

    public void Activate()
    {
        if (activated) return;
        activated = true;

        if (TimeManager.Instance != null)
            TimeManager.Instance.ActionCompleted(transform.position);
        else
            Debug.LogError("TimeManager.Instance is null! Добавь TimeManager на сцену.");

        if (doorToOpen != null)
            doorToOpen.Open();

        gameObject.SetActive(false);
    }
}
