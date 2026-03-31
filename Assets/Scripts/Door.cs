using UnityEngine;

public class Door : MonoBehaviour
{
    private Collider2D mainCollider;
    private SpriteRenderer sr;

    [Header("Внешний вид двери")]
    [SerializeField] private Sprite openSprite;

    private Sprite closedSprite;

    public bool IsOpen { get; private set; }

    private void Awake()
    {
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            if (!collider.isTrigger)
            {
                mainCollider = collider;
                break;
            }
        }

        sr = GetComponent<SpriteRenderer>();

        if (sr != null)
            closedSprite = sr.sprite;

        if (mainCollider != null)
            mainCollider.enabled = true;
    }

    public void Open()
    {
        IsOpen = true;

        // Отключаем только основной коллайдер, но оставляем trigger-коллайдеры.
        if (mainCollider != null)
            mainCollider.enabled = false;

        if (sr != null && openSprite != null)
            sr.sprite = openSprite;
    }

    public void Close()
    {
        IsOpen = false;

        if (mainCollider != null)
            mainCollider.enabled = true;

        if (sr != null && closedSprite != null)
            sr.sprite = closedSprite;
    }
}
