using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    [Header("Smoothing")]
    public float smoothTime = 0.3f; // чем больше — тем более "ленивая" камера

    [Header("Offset")]
    public Vector3 offset;

    [Header("Dead Zone")]
    public float deadZone = 1.5f; // радиус зоны, где камера не двигается

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition = target.position + offset;
        Vector3 currentPosition = transform.position;

        Vector3 delta = targetPosition - currentPosition;

        // Если игрок вышел за пределы dead zone
        if (delta.magnitude > deadZone)
        {
            // Камера догоняет, но не до конца (оставляет зону)
            Vector3 desiredPosition = currentPosition + (delta - delta.normalized * deadZone);

            Vector3 smoothedPosition = Vector3.SmoothDamp(
                currentPosition,
                desiredPosition,
                ref velocity,
                smoothTime
            );

            transform.position = new Vector3(
                smoothedPosition.x,
                smoothedPosition.y,
                -10f
            );
        }
        else
        {
            // Просто фиксируем Z (на случай если что-то сдвинуло)
            transform.position = new Vector3(
                currentPosition.x,
                currentPosition.y,
                -10f
            );
        }
    }
}
