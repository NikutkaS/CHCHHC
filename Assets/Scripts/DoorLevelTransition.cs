using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class DoorLevelTransition : MonoBehaviour
{
    [Header("Переход на следующий уровень")]
    [SerializeField] private string nextSceneName = "Level2";
    [SerializeField] private bool loadOnlyOnce = true;

    private Door door;
    private bool loaded;

    private void Awake()
    {
        door = GetComponentInParent<Door>();

        Collider2D[] colliders = GetComponents<Collider2D>();
        Collider2D sourceCollider = null;
        BoxCollider2D triggerCollider = null;

        foreach (Collider2D col in colliders)
        {
            if (!col.isTrigger)
                sourceCollider = col;
            else if (triggerCollider == null && col is BoxCollider2D box)
                triggerCollider = box;
        }

        if (triggerCollider == null)
        {
            triggerCollider = gameObject.AddComponent<BoxCollider2D>();
            triggerCollider.isTrigger = true;
        }

        if (sourceCollider is BoxCollider2D sourceBox)
        {
            triggerCollider.size = sourceBox.size;
            triggerCollider.offset = sourceBox.offset;
        }

        triggerCollider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (loaded && loadOnlyOnce)
            return;

        if (door != null && !door.IsOpen)
            return;

        if (!IsPlayer(other))
            return;

        loaded = true;
        SceneManager.LoadScene(nextSceneName);
    }

    private bool IsPlayer(Collider2D other)
    {
        if (other.CompareTag("Player"))
            return true;

        return other.GetComponentInParent<PlayerMovement>() != null;
    }
}
