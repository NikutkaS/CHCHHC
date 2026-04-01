using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SceneInteractButton : MonoBehaviour
{
    [SerializeField] private PlayerInteraction playerInteraction;
    [SerializeField] private bool hideWhenUsed = true;

    private void Awake()
    {
        // Если кнопку ставишь рядом с объектом, она может быть скрыта по умолчанию.
        gameObject.SetActive(gameObject.activeSelf);
    }

    private void OnMouseDown()
    {
        if (playerInteraction == null)
            return;

        if (!playerInteraction.CanInteract)
            return;

        playerInteraction.TryInteract();

        if (hideWhenUsed)
            gameObject.SetActive(false);
    }

    public void SetPlayerInteraction(PlayerInteraction interaction)
    {
        playerInteraction = interaction;
    }
}
