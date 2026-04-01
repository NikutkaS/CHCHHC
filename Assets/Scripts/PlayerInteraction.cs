using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private IInteractableAction currentAction;

    public bool CanInteract => currentAction != null && currentAction.CanInteract;

    private void Update()
    {
        if (currentAction != null && !currentAction.CanInteract)
        {
            currentAction.HideMobileButton();
            currentAction = null;
        }

        if (Input.GetKeyDown(KeyCode.E))
            TryInteract();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        SetCurrentAction(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        SetCurrentAction(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        IInteractableAction action = GetActionFromCollider(other);
        if (action != null && action == currentAction)
        {
            currentAction.HideMobileButton();
            currentAction = null;
        }
    }

    private void SetCurrentAction(Collider2D other)
    {
        IInteractableAction action = GetActionFromCollider(other);
        if (action == null || !action.CanInteract)
            return;

        if (currentAction != null && currentAction != action)
            currentAction.HideMobileButton();

        currentAction = action;
        currentAction.ShowMobileButton();
    }

    private IInteractableAction GetActionFromCollider(Collider2D other)
    {
        ActionTrigger actionTrigger = other.GetComponentInParent<ActionTrigger>();
        if (actionTrigger != null)
            return actionTrigger;

        PuzzleTrigger puzzleTrigger = other.GetComponentInParent<PuzzleTrigger>();
        if (puzzleTrigger != null)
            return puzzleTrigger;

        DocumentPickupTrigger documentPickupTrigger = other.GetComponentInParent<DocumentPickupTrigger>();
        if (documentPickupTrigger != null)
            return documentPickupTrigger;

        return null;
    }

    public void TryInteract()
    {
        if (currentAction == null || !currentAction.CanInteract)
            return;

        IInteractableAction action = currentAction;
        action.HideMobileButton();
        currentAction = null;
        action.Activate();
    }
}
