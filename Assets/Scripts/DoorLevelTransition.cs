using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Door))]
public class DoorLevelTransition : MonoBehaviour
{
    [Header("Переход на следующий уровень")]
    [SerializeField] private string nextSceneName = "Level2";
    [SerializeField] private bool loadOnlyOnce = true;

    private Door door;
    private bool loaded;

    private void Awake()
    {
        door = GetComponent<Door>();
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
