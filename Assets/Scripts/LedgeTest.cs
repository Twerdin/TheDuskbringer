using UnityEngine;

public class LedgeTest : MonoBehaviour
{
    public Transform ledgeCheck;
    public LayerMask ground;

    private Rigidbody2D rb;
    private bool grabbing;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        bool touchingWall = Physics2D.OverlapCircle(ledgeCheck.position, 0.1f, ground);

        if (touchingWall && !grabbing)
        {
            grabbing = true;
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0;
        }

        if (grabbing && Input.GetKeyDown(KeyCode.W))
        {
            transform.position = new Vector3(
    ledgeCheck.position.x,
    ledgeCheck.position.y + 1f,
    transform.position.z
);

        }
    }
}

