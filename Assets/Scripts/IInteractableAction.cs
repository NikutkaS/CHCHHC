using UnityEngine;

public interface IInteractableAction
{
    bool CanInteract { get; }
    void Activate();
    void ShowMobileButton();
    void HideMobileButton();
}
