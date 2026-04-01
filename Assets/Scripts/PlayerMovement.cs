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

    private Vector2 mobileInput;
    private bool hasMobileInput;

    public Vector2 CurrentMovement => movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;
    }

    public void SetMobileInput(Vector2 input)
    {
        mobileInput = input;
        hasMobileInput = input.sqrMagnitude > 0.001f;
    }

    public void ClearMobileInput()
    {
        mobileInput = Vector2.zero;
        hasMobileInput = false;
    }

    void Update()
    {
        if (hasMobileInput)
        {
            movement = SnapToCardinal(mobileInput);
            return;
        }

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

    private Vector2 SnapToCardinal(Vector2 input)
    {
        if (input.sqrMagnitude < 0.001f)
            return Vector2.zero;

        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            return input.x > 0f ? Vector2.right : Vector2.left;

        return input.y > 0f ? Vector2.up : Vector2.down;
    }

    private MoveDirection ResolveDirection(bool upHeld, bool downHeld, bool leftHeld, bool rightHeld)
    {
        if (lockedDirection != MoveDirection.None && IsHeld(lockedDirection, upHeld, downHeld, leftHeld, rightHeld))
            return lockedDirection;

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
