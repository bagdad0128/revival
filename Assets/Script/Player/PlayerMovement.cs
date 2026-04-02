using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    
    private Vector2 movement;
    
    [Header("Jump Settings")]
    public float jumpForce = 12f;
    public LayerMask groundLayer;
    public float fallMultiplier = 3f;
    
    [Header("Attack Settings")]
    public bool isFacingRight = true;
    public Transform firePoint;
    public Vector2 offsetRight = new Vector2(1, 0);
    public Vector2 offsetUp = new Vector2(0, 1);
    public Vector2 offsetDown = new Vector2(0, 0);
    
    private Rigidbody2D rb;
    private PlayerDash dash;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        dash = GetComponent<PlayerDash>();
    }

    private void Update()
    {
        if(dash is not null && dash.isDashing) return;
        Move();
    }

    void FixedUpdate()
    {
        ApplyBetterJump();
    }
    
    void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>();
        if(movement.x > 0 && !isFacingRight) { Flip(); }
        else if(movement.x < 0 && isFacingRight) { Flip(); }
        if(movement.y > 0.5) firePoint.localPosition = offsetUp;
        else if(movement.y < -0.5) firePoint.localPosition = offsetDown;
        else firePoint.localPosition = offsetRight;
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed && IsGrounded())
        {
            Jump();
        }
    }

    void Move()
    {
        rb.linearVelocity = new Vector2(movement.x * speed, rb.linearVelocity.y);
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    bool IsGrounded()
    {
        float rayDistance = 0.6f;
        float sideOffset = 0.5f;

        Vector2 centerOrigin = transform.position;
        Vector2 leftOrigin = transform.position + (Vector3.left * sideOffset);
        Vector2 rightOrigin = transform.position + (Vector3.right * sideOffset);

        bool hitCenter = Physics2D.Raycast(centerOrigin, Vector2.down, rayDistance, groundLayer);
        bool hitLeft = Physics2D.Raycast(leftOrigin, Vector2.down, rayDistance, groundLayer);
        bool hitRight = Physics2D.Raycast(rightOrigin, Vector2.down, rayDistance, groundLayer);
        
        return hitCenter || hitLeft || hitRight;
    }

    void ApplyBetterJump()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * (Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime);
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(isFacingRight ? 1 : -1, 1, 1);
    }
}
