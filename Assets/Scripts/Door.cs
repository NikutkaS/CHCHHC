using UnityEngine;

public class Door : MonoBehaviour
{
    private Collider2D col;
    private SpriteRenderer sr;

    [Header("Внешний вид двери")]
    [SerializeField] private Sprite openSprite;

    private Sprite closedSprite;

    public bool IsOpen { get; private set; }

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();

        if (sr != null)
            closedSprite = sr.sprite;

        if (col != null)
            col.isTrigger = false;
    }

    public void Open()
    {
        IsOpen = true;

        // Открытая дверь больше не блокирует игрока, но остаётся видимой
        // с отдельным спрайтом и работает как вход на следующий уровень.
        if (col != null)
            col.isTrigger = true;

        if (sr != null && openSprite != null)
            sr.sprite = openSprite;
    }

    public void Close()
    {
        IsOpen = false;

        if (col != null)
            col.isTrigger = false;

        if (sr != null && closedSprite != null)
            sr.sprite = closedSprite;
    }
}
