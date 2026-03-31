using UnityEngine;

public class PuzzleTrigger : MonoBehaviour
{
    [Header("Загадка Level2")]
    [SerializeField] private MathDoorPuzzle puzzleToOpen;

    private bool activated;

    public void Activate()
    {
        if (activated) return;
        activated = true;

        if (puzzleToOpen != null)
            puzzleToOpen.OpenPuzzle(transform.position);
        else
            Debug.LogError("PuzzleToOpen не назначен на PuzzleTrigger.");

        gameObject.SetActive(false);
    }
}
