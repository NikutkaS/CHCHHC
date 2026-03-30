using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ActionTrigger action = other.GetComponent<ActionTrigger>();
            if (action != null)
                action.Activate();
        }
    }
}
