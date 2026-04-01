using UnityEngine;

public class ActionTrigger : MonoBehaviour, IInteractableAction
{
    [Header("Level1")]
    [SerializeField] private Door doorToOpen;

    [Header("Level2")]
    [SerializeField] private MathDoorPuzzle puzzleToOpen;

    [Header("Objective")]
    [SerializeField] private int requiredObjectiveStep = -1;
    [SerializeField] private int nextObjectiveStep = -1;
    [SerializeField] private bool completeObjectiveAutomatically = true;

    [Header("Mobile UI")]
    [SerializeField] private GameObject mobileButton;

    private bool activated;

    public bool CanInteract
    {
        get
        {
            if (activated)
                return false;

            if (LevelObjectiveManager.Instance == null)
                return requiredObjectiveStep < 0;

            if (requiredObjectiveStep < 0)
                return true;

            return LevelObjectiveManager.Instance.CurrentStepIndex == requiredObjectiveStep;
        }
    }

    public GameObject MobileButton => mobileButton;

    public void Activate()
    {
        if (activated || !CanInteract)
            return;

        if (puzzleToOpen != null)
        {
            activated = true;
            HideMobileButton();
            puzzleToOpen.OpenPuzzle(transform.position, this);
            return;
        }

        activated = true;
        HideMobileButton();

        if (doorToOpen != null)
            doorToOpen.Open();

        if (completeObjectiveAutomatically)
            AdvanceObjective();

        FinishAction();
    }

    public void ResetTrigger()
    {
        activated = false;
        gameObject.SetActive(true);
    }

    public void CompleteTrigger()
    {
        activated = true;
        HideMobileButton();

        if (completeObjectiveAutomatically)
            AdvanceObjective();

        FinishAction();
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

    private void AdvanceObjective()
    {
        if (LevelObjectiveManager.Instance == null)
            return;

        if (nextObjectiveStep >= 0)
            LevelObjectiveManager.Instance.SetStep(nextObjectiveStep);
        else
            LevelObjectiveManager.Instance.CompleteCurrentObjective();
    }

    private void FinishAction()
    {
        if (TimeManager.Instance != null)
            TimeManager.Instance.ActionCompleted(transform.position);
        else
            Debug.LogError("TimeManager.Instance is null! Добавь TimeManager на сцену.");

        gameObject.SetActive(false);
    }
}
