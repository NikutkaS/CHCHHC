using UnityEngine;

public class MobileInteractButton : MonoBehaviour
{
    [SerializeField] private PlayerInteraction playerInteraction;

    public void PressInteract()
    {
        if (playerInteraction != null)
            playerInteraction.TryInteract();
    }
}
