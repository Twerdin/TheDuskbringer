using UnityEngine;

public class LedgeGrab : MonoBehaviour
{
    public Transform ledgeCheck;
    public Transform groundCheck;
    public LayerMask whatIsGround;

    public float checkRadius = 0.1f;
    public float climbXOffset = 0.5f;
    public float climbYOffset = 1f;

    private Rigidbody2D rb;
    private bool isGrabbing = false;
    private Vector2 ledgePos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        bool wall = Physics2D.OverlapCircle(ledgeCheck.position, checkRadius, whatIsGround);
        bool top = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        if (wall && !top && !isGrabbing)
        {
            isGrabbing = true;
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0;

            ledgePos = transform.position;
        }

        if (isGrabbing)
        {
            // Tırmanma tuşu (W veya ↑)
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                Climb();
            }
        }
    }

    void Climb()
    {
        transform.position = new Vector2(
            ledgePos.x + climbXOffset,
            ledgePos.y + climbYOffset
        );

        rb.gravityScale = 3; // kendi gravity’in neyse onu yaz
        isGrabbing = false;
    }
}

