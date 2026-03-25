using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    
    private Vector2 movement;
    
    [Header("Jump Settings")]
    public float jumpForce = 12f;
    public Transform groundCheck;
    public Vector2 groundCheckSize = new (1, 1);
    public LayerMask groundLayer;
    public float fallMultiplier = 3f;
    
    private bool isGrounded = false;
    
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

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, groundLayer); //overlapbox가 더 좋음ㅇㅇ
        if(dash.isDashing) return;
        Move();
        ApplyBetterJump();
        CheckTunneling();
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
        if (value.isPressed && isGrounded)
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

    void ApplyBetterJump()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * (Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime);
        }
    }

    void CheckTunneling()
    {
        if(isGrounded || rb.linearVelocity.y < 0.01f) return;
        float moveDistance = rb.linearVelocity.magnitude * Time.fixedDeltaTime;
        Vector2 direction = rb.linearVelocity.normalized;
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, groundCheckSize, 0, direction, moveDistance, groundLayer);
        if (hit.collider != null)
        {
            transform.position = hit.point + (hit.normal * (groundCheckSize.y / 2));
            rb.linearVelocity = Vector2.zero;
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(isFacingRight ? 1 : -1, 1, 1);
    }
}
