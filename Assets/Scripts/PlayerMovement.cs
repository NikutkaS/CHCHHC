using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;

    private enum MoveDirection
    {
        None,
        Up,
        Down,
        Left,
        Right
    }

    private MoveDirection lockedDirection = MoveDirection.None;

    private float upPressedAt = -1f;
    private float downPressedAt = -1f;
    private float leftPressedAt = -1f;
    private float rightPressedAt = -1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            upPressedAt = Time.time;

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            downPressedAt = Time.time;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            leftPressedAt = Time.time;

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            rightPressedAt = Time.time;

        bool upHeld = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        bool downHeld = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        bool leftHeld = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        bool rightHeld = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);

        lockedDirection = ResolveDirection(upHeld, downHeld, leftHeld, rightHeld);
        movement = DirectionToVector(lockedDirection);
    }

    private MoveDirection ResolveDirection(bool upHeld, bool downHeld, bool leftHeld, bool rightHeld)
    {
        // Если текущая выбранная кнопка ещё зажата — продолжаем двигаться в эту сторону.
        if (lockedDirection != MoveDirection.None && IsHeld(lockedDirection, upHeld, downHeld, leftHeld, rightHeld))
            return lockedDirection;

        // Иначе выбираем самую первую из зажатых кнопок.
        MoveDirection bestDirection = MoveDirection.None;
        float bestTime = float.MaxValue;

        TryPickDirection(MoveDirection.Up, upHeld, upPressedAt, ref bestDirection, ref bestTime);
        TryPickDirection(MoveDirection.Down, downHeld, downPressedAt, ref bestDirection, ref bestTime);
        TryPickDirection(MoveDirection.Left, leftHeld, leftPressedAt, ref bestDirection, ref bestTime);
        TryPickDirection(MoveDirection.Right, rightHeld, rightPressedAt, ref bestDirection, ref bestTime);

        return bestDirection;
    }

    private void TryPickDirection(MoveDirection direction, bool isHeld, float pressedAt, ref MoveDirection bestDirection, ref float bestTime)
    {
        if (!isHeld)
            return;

        if (pressedAt < 0f)
            pressedAt = Time.time;

        if (pressedAt < bestTime)
        {
            bestTime = pressedAt;
            bestDirection = direction;
        }
    }

    private bool IsHeld(MoveDirection direction, bool upHeld, bool downHeld, bool leftHeld, bool rightHeld)
    {
        switch (direction)
        {
            case MoveDirection.Up:
                return upHeld;
            case MoveDirection.Down:
                return downHeld;
            case MoveDirection.Left:
                return leftHeld;
            case MoveDirection.Right:
                return rightHeld;
            default:
                return false;
        }
    }

    private Vector2 DirectionToVector(MoveDirection direction)
    {
        switch (direction)
        {
            case MoveDirection.Up:
                return Vector2.up;
            case MoveDirection.Down:
                return Vector2.down;
            case MoveDirection.Left:
                return Vector2.left;
            case MoveDirection.Right:
                return Vector2.right;
            default:
                return Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
