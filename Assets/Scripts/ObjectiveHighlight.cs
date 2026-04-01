using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ObjectiveHighlight : MonoBehaviour
{
    [Header("Visual")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color highlightedColor = new Color(1f, 0.95f, 0.35f, 1f);
    [SerializeField] private Vector3 highlightedScale = new Vector3(1.08f, 1.08f, 1f);

    private SpriteRenderer spriteRenderer;
    private Vector3 baseScale;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        baseScale = transform.localScale;
    }

    private void Start()
    {
        SetHighlighted(false);
    }

    public void SetHighlighted(bool highlighted)
    {
        if (spriteRenderer == null)
            return;

        spriteRenderer.color = highlighted ? highlightedColor : normalColor;
        transform.localScale = highlighted ? highlightedScale : baseScale;
    }
}
