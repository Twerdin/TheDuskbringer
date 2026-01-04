using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Bileþenler")]
    private Animator anim;
    private Rigidbody2D rb;
    private PlayerHealth playerHealth;
    private Transform playerTarget;

    [Header("Can Ayarlarý")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Hareket Ayarlarý")]
    public float walkSpeed = 1.5f;
    public float runSpeed = 4f;

    [Header("Menzil & Yapay Zeka")]
    public float detectionRange = 6f;
    public float stopDistance = 1.2f;

    [Header("Devriye (Patrol) Ayarlarý")]
    public float patrolDuration = 2f;
    public float idleDuration = 2f;
    private float patrolTimer;
    private float idleTimer;
    private bool isMovingRight = true;

    [Header("Saldýrý")]
    public float attackRate = 1f;
    public int damageAmount = 20;
    private float nextAttackTime = 0f;

    // --- YENÝ EKLENEN KISIM: KOMBO SAYACI ---
    private int comboStep = 0;
    // ----------------------------------------

    private bool isDead = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;

        patrolTimer = patrolDuration;
        idleTimer = idleDuration;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTarget = playerObj.transform;
            playerHealth = playerObj.GetComponent<PlayerHealth>();
        }
    }

    void Update()
    {
        if (isDead) return;

        // Player var mý yok mu kontrol et (Hata almamak için)
        if (playerTarget == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTarget.position);

        if (distanceToPlayer < detectionRange)
        {
            EngagePlayer(distanceToPlayer);
        }
        else
        {
            PatrolRoutine();
        }
    }

    // --- DEVRÝYE MODU ---
    void PatrolRoutine()
    {
        anim.SetBool("isRunning", false);

        if (idleTimer > 0)
        {
            idleTimer -= Time.deltaTime;
            StopMovement();
            anim.SetBool("isWalking", false);
            return;
        }

        if (patrolTimer > 0)
        {
            patrolTimer -= Time.deltaTime;
            MovePatrol();
            anim.SetBool("isWalking", true);
        }
        else
        {
            idleTimer = idleDuration;
            patrolTimer = patrolDuration;
            FlipDirection();
        }
    }

    void MovePatrol()
    {
        float dir = isMovingRight ? 1 : -1;
        rb.linearVelocity = new Vector2(dir * walkSpeed, rb.linearVelocity.y);
        transform.localScale = new Vector3(dir, 1, 1);
    }

    void FlipDirection()
    {
        isMovingRight = !isMovingRight;
    }

    // --- SAVAÞ MODU ---
    void EngagePlayer(float distance)
    {
        anim.SetBool("isWalking", false);

        if (distance > stopDistance)
        {
            Vector2 direction = (playerTarget.position - transform.position).normalized;
            rb.linearVelocity = new Vector2(direction.x * runSpeed, rb.linearVelocity.y);

            // Düþmanýn yönünü oyuncuya çevir
            if (direction.x > 0) transform.localScale = new Vector3(1, 1, 1);
            else if (direction.x < 0) transform.localScale = new Vector3(-1, 1, 1);

            anim.SetBool("isRunning", true);
        }
        else
        {
            StopMovement();
            anim.SetBool("isRunning", false);

            if (Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void StopMovement()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    // --- YENÝLENMÝÞ SALDIRI FONKSÝYONU ---
    void Attack()
    {
        // 1. Sayacý artýr (1, 2, 3...)
        comboStep++;

        // 2. Eðer 3'ü geçtiyse baþa sar (1 yap)
        if (comboStep > 3)
        {
            comboStep = 1;
        }

        // 3. Animator'a hangi sýrada olduðumuzu bildir (AttackInt)
        anim.SetInteger("AttackInt", comboStep);

        // 4. Saldýrýyý tetikle (AttackTrig)
        anim.SetTrigger("AttackTrigger");


        // 5. Oyuncuya hasar ver
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageAmount);
        }
    }

    // --- HASAR ALMA VE ÖLÜM ---
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        // Hasar animasyonunu tetikle
        anim.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("Düþman Öldü!");

        anim.SetBool("IsDead", true); // "IsDead" parametresinin Animator'da olduðundan emin ol (yoksa hata verir)

        // Diðer animasyon parametrelerini temizle
        anim.SetBool("isRunning", false);
        anim.SetBool("isWalking", false);

        rb.linearVelocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;
        rb.bodyType = RigidbodyType2D.Kinematic;

        this.enabled = false;
    }
}