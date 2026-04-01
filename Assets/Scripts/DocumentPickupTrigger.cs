using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DocumentPickupTrigger : MonoBehaviour, IInteractableAction
{
    [Header("Visual")]
    [SerializeField] private GameObject documentVisual;

    [Header("Mobile UI")]
    [SerializeField] private GameObject mobileButton;

    [Header("Objective")]
    [SerializeField] private int requiredObjectiveStep = -1;
    [SerializeField] private int nextObjectiveStep = -1;

    private bool collected;

    public bool CanInteract
    {
        get
        {
            if (collected)
                return false;

            if (LevelObjectiveManager.Instance == null)
                return requiredObjectiveStep < 0;

            if (requiredObjectiveStep < 0)
                return true;

            return LevelObjectiveManager.Instance.CurrentStepIndex == requiredObjectiveStep;
        }
    }

    public void Activate()
    {
        if (!CanInteract)
            return;

        collected = true;

        if (documentVisual != null)
            documentVisual.SetActive(false);
        else
            gameObject.SetActive(false);

        HideMobileButton();

        if (LevelObjectiveManager.Instance != null)
        {
            if (nextObjectiveStep >= 0)
                LevelObjectiveManager.Instance.SetStep(nextObjectiveStep);
            else
                LevelObjectiveManager.Instance.CompleteCurrentObjective();
        }

        if (TimeManager.Instance != null)
            TimeManager.Instance.ActionCompleted(transform.position);
        else
            Debug.LogError("TimeManager.Instance is null! Добавь TimeManager на сцену.");
    }

    public void ShowMobileButton()
    {
        if (mobileButton != null && CanInteract)
            mobileButton.SetActive(true);
    }

    public void HideMobileButton()
    {
        if (mobileButton != null)
            mobileButton.SetActive(false);
    }
}
