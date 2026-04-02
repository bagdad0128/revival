using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Dash Settings")] public float dashSpeed = 20f;
    public float dashTime = 0.2f;
    public float dashCooldown = 0.2f;

    private bool canDash = true;
    public bool isDashing { get; private set; }

    private void Awake() => rb = GetComponent<Rigidbody2D>();

    void OnDash(InputValue value)
    {
        if (value.isPressed && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        
        float dashDirection = transform.localScale.x;
        rb.linearVelocity = new Vector2(dashDirection * dashSpeed, 0f);
        
        yield return new WaitForSeconds(dashTime);
        
        rb.gravityScale = originalGravity;
        isDashing = false;
        
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
