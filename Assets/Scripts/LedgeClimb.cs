using UnityEngine;

public class LedgeClimb : MonoBehaviour
{
    
    public Transform ledgeCheck; 
    public float ledgeCheckRadius = 0.2f;
    public LayerMask whatIsGround; 
    public Vector2 ledgeOffset; 
    public Vector2 climbOffset; 

    
    private Rigidbody2D rb;
    private Animator anim;
    private PlayerMovement playerMovement; 
    private bool isTouchingLedge;
    private bool isHoldingLedge;
    private bool isClimbing; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>(); 
    }

    void Update()
    {
        
        if (!isClimbing)
        {
            
            if (isHoldingLedge)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    isHoldingLedge = false;
                    isClimbing = true; 
                    anim.SetTrigger("Climbing"); 
                }
            }

            
            isTouchingLedge = Physics2D.OverlapCircle(ledgeCheck.position, ledgeCheckRadius, whatIsGround);

           
            if (isTouchingLedge && !isHoldingLedge && rb.linearVelocity.y < 0)
            {
                HoldLedge();
            }
        }
    }

    void HoldLedge()
    {
        isHoldingLedge = true;

        
        anim.SetBool("holdEdge", true);

        
        if (playerMovement != null) playerMovement.enabled = false;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;

        
        transform.position = new Vector2(ledgeCheck.position.x - ledgeOffset.x, ledgeCheck.position.y - ledgeOffset.y);
    }

    
    public void FinishClimb()
    {
        
        transform.position = new Vector2(transform.position.x + climbOffset.x, transform.position.y + climbOffset.y);

        
        rb.gravityScale = 1f; 
        if (playerMovement != null) playerMovement.enabled = true;
        
        
        anim.SetBool("holdEdge", false);
        isClimbing = false;
    }

    
    private void OnDrawGizmos()
    {
        if (ledgeCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(ledgeCheck.position, ledgeCheckRadius);
        }
    }
}