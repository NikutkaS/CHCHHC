using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleMobileJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("References")]
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform handle;
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Settings")]
    [SerializeField] private float handleRange = 60f;

    private Vector2 inputVector;

    private void Awake()
    {
        if (background == null)
            background = transform as RectTransform;
    }

    private void Start()
    {
        if (playerMovement == null)
            playerMovement = FindObjectOfType<PlayerMovement>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (background == null || handle == null)
            return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint);

        Vector2 normalized = localPoint / handleRange;
        inputVector = Vector2.ClampMagnitude(normalized, 1f);

        handle.anchoredPosition = inputVector * handleRange;

        if (playerMovement != null)
            playerMovement.SetMobileInput(inputVector);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;

        if (handle != null)
            handle.anchoredPosition = Vector2.zero;

        if (playerMovement != null)
            playerMovement.ClearMobileInput();
    }
}
