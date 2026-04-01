using UnityEngine;

public class PuzzleTrigger : MonoBehaviour, IInteractableAction
{
    [Header("Загадка Level2")]
    [SerializeField] private MathDoorPuzzle puzzleToOpen;

    [Header("Mobile UI")]
    [SerializeField] private GameObject mobileButton;

    [Header("Objective")]
    [SerializeField] private int requiredObjectiveStep = -1;
    [SerializeField] private int nextObjectiveStep = -1;

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

    public void Activate()
    {
        if (!CanInteract)
            return;

        activated = true;
        HideMobileButton();

        if (nextObjectiveStep >= 0 && LevelObjectiveManager.Instance != null)
            LevelObjectiveManager.Instance.SetStep(nextObjectiveStep);

        if (puzzleToOpen != null)
            puzzleToOpen.OpenPuzzle(transform.position);
        else
            Debug.LogError("PuzzleToOpen не назначен на PuzzleTrigger.");

        gameObject.SetActive(false);
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
