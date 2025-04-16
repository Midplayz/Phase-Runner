using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;

    [Header("Phase Settings")]
    [SerializeField] private float phaseDuration = 2f;
    [SerializeField] private float phaseCooldown = 4f;
    [SerializeField] private KeyCode phaseKey = KeyCode.LeftShift;

    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Transparency Settings")]
    [SerializeField] private float phasedAlpha = 0.5f;

    // Components
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    // State
    private bool isGrounded;
    private bool isPhasing = false;
    private float phaseTimer = 0f;
    private float cooldownTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandlePhase();
        CheckGrounded();
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    private void HandleJump()
    {
        if (!isPhasing && Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private void HandlePhase()
    {
        cooldownTimer -= Time.deltaTime;

        if (Input.GetKeyDown(phaseKey) && cooldownTimer <= 0 && !isPhasing)
        {
            StartPhase();
        }

        if (isPhasing)
        {
            phaseTimer -= Time.deltaTime;

            if (phaseTimer <= 0)
            {
                EndPhase();
            }
        }
    }

    private void StartPhase()
    {
        isPhasing = true;
        phaseTimer = phaseDuration;
        cooldownTimer = phaseCooldown;

        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, phasedAlpha);
        gameObject.layer = LayerMask.NameToLayer("Phased"); 
    }

    private void EndPhase()
    {
        isPhasing = false;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    private void CheckGrounded()
    {
        Collider2D hit = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);
        isGrounded = hit != null;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SolidObstacle"))
        {
            TriggerGameOver("Hit Solid Obstacle");
        }

        if (other.CompareTag("PhaseableObstacle"))
        {
            if (!isPhasing)
            {
                TriggerGameOver("Should have phased but jumped!");
            }
        }
    }

    private void TriggerGameOver(string reason)
    {
        Debug.Log("Game Over: " + reason);
        Time.timeScale = 0;
    }


}
