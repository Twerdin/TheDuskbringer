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

    [Header("Görsel Ayarlar (ÖNEMLÝ)")]
    // Eðer karakterin çizimi doðuþtan SOLA bakýyorsa bunu iþaretle!
    public bool spriteFaceLeft = false;

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

    [Tooltip("Kaç tane saldýrý animasyonun var? (Örn: 2 ise 1-2-1-2 yapar)")]
    public int maxComboCount = 2;
    private int comboStep = 0;

    // --- ANIMASYON ÝSÝMLERÝ (Inspector'dan Ayarlanabilir) ---
    [Header("Animasyon Ýsim Ayarlarý")]
    public string walkAnimName = "isWalking";
    public string runAnimName = "isRunning";
    public string attackTriggerName = "AttackTrigger"; // Trigger adý
    public string attackIntName = "AttackInt";         // Integer (Sayý) adý
    public string hurtTriggerName = "Hurt";
    public string deadBoolName = "IsDead";
    // ---------------------------------------------------------
    [Header("Ölüm Ayarlarý")]
    // Karakter ölünce ne kadar aþaðý/yukarý kaysýn? (Eksi deðer aþaðý indirir)
    public float deathYOffset = -0.15f;
    private bool isDead = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;

        patrolTimer = patrolDuration;
        idleTimer = idleDuration;

        // Oyuncuyu Bul
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

        // Player yoksa hata verme, sadece bekle
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
        anim.SetBool(runAnimName, false);

        if (idleTimer > 0)
        {
            idleTimer -= Time.deltaTime;
            StopMovement();
            anim.SetBool(walkAnimName, false);
            return;
        }

        if (patrolTimer > 0)
        {
            patrolTimer -= Time.deltaTime;
            MovePatrol();
            anim.SetBool(walkAnimName, true);
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
        // Not: Unity 6 kullanýyorsan linearVelocity, eskiyse velocity kullan.
        rb.linearVelocity = new Vector2(dir * walkSpeed, rb.linearVelocity.y);

        // --- TERS SPRITE DÜZELTME MANTIÐI ---
        float scaleMultiplier = spriteFaceLeft ? -1 : 1;
        transform.localScale = new Vector3(dir * scaleMultiplier, 1, 1);
    }

    void FlipDirection()
    {
        isMovingRight = !isMovingRight;
    }

    // --- SAVAÞ MODU (PLAYER KOVALAMA) ---
    void EngagePlayer(float distance)
    {
        anim.SetBool(walkAnimName, false);

        if (distance > stopDistance)
        {
            Vector2 direction = (playerTarget.position - transform.position).normalized;
            rb.linearVelocity = new Vector2(direction.x * runSpeed, rb.linearVelocity.y);

            // --- YÖN ÇEVÝRME (TERS SPRITE DESTEKLÝ) ---
            float scaleMultiplier = spriteFaceLeft ? -1 : 1;

            if (direction.x > 0)
                transform.localScale = new Vector3(1 * scaleMultiplier, 1, 1);
            else if (direction.x < 0)
                transform.localScale = new Vector3(-1 * scaleMultiplier, 1, 1);
            // ------------------------------------------

            anim.SetBool(runAnimName, true);
        }
        else
        {
            StopMovement();
            anim.SetBool(runAnimName, false);

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

    // --- SALDIRI SÝSTEMÝ ---
    void Attack()
    {
        comboStep++;

        // Belirlenen kombo sýnýrýný geçerse 1'e dön
        if (comboStep > maxComboCount)
        {
            comboStep = 1;
        }

        // Animasyon parametre isimlerini deðiþkenden al
        anim.SetInteger(attackIntName, comboStep);
        anim.SetTrigger(attackTriggerName);

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
        anim.SetTrigger(hurtTriggerName);

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

        anim.SetBool(deadBoolName, true);
        anim.SetBool(runAnimName, false);
        anim.SetBool(walkAnimName, false);

        // Fiziði ve Çarpýþmayý Kapat
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Collider2D>().enabled = false;

        // --- YENÝ KISIM: INSPECTOR'DAN AYARLANAN DEÐERÝ KULLAN ---
        // Mevcut Y pozisyonuna senin belirlediðin (deathYOffset) deðeri ekler.
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y + deathYOffset,
            transform.position.z
        );
        // ----------------------------------------------------------

        this.enabled = false;
    }
}