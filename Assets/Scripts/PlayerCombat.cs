using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Gerekli Bileþenler")]
    public PlayerMovement playerController;
    private Animator anim;

    [Header("Saldýrý Ayarlarý")]
    public float attackRate = 2f;
    private float nextAttackTime = 0f;

    [Header("Skill Kilidi (YENÝ EKLENEN KISIM)")]
    // Hata veren deðiþken buydu. Artýk burada tanýmlý.
    public bool isSpecialAttackUnlocked = false;

    [Header("Yer Saldýrýsý (J Tuþu)")]
    public float groundAttackOffset = 1.0f;
    public float groundAttackRange = 0.8f;
    public int groundDamage = 20;
    public float comboResetTime = 1.0f;

    [Header("Özel Saldýrý (Q Tuþu)")]
    public KeyCode specialAttackInput = KeyCode.Q;
    public string specialAttackAnimName = "Special_Attack"; // Animator ismin neyse onu yaz
    public float specialAttackOffset = 0.8f;
    public float specialAttackRange = 1.2f;
    public int specialAttackDamage = 25;

    [Header("Genel")]
    public LayerMask enemyLayers;

    private int comboStep = 0;
    private float comboTimer = 0f;

    void Start()
    {
        anim = GetComponent<Animator>();
        if (playerController == null)
            playerController = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        // Kombo süresi sayacý
        if (comboStep > 0)
        {
            comboTimer += Time.deltaTime;
            if (comboTimer >= comboResetTime) ResetCombo();
        }

        // --- SALDIRI KONTROLÜ ---
        if (Time.time >= nextAttackTime)
        {
            // 1. ÖZEL SALDIRI (Q)
            // ARTIK ÞART VAR: Hem Q'ya basýlacak HEM DE skill açýlmýþ (True) olacak.
            if (Input.GetKeyDown(specialAttackInput) && isSpecialAttackUnlocked)
            {
                PerformSpecialAttack();
                nextAttackTime = Time.time + 1f / attackRate;
            }

            // 2. NORMAL KOMBO (J)
            // Bu sadece YERDE çalýþýr.
            else if (Input.GetKeyDown(KeyCode.J) && playerController.isGrounded)
            {
                PerformComboAttack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void PerformComboAttack()
    {
        comboStep++;
        comboTimer = 0f;

        if (comboStep == 1) anim.SetTrigger("Attack1");
        else if (comboStep == 2) anim.SetTrigger("Attack2");
        else if (comboStep == 3) anim.SetTrigger("Attack3");

        DealDamage(groundDamage, groundAttackRange, groundAttackOffset);

        if (comboStep >= 3) comboStep = 0;
    }

    // --- ÖZEL SALDIRI ---
    void PerformSpecialAttack()
    {
        anim.SetTrigger(specialAttackAnimName);
        DealDamage(specialAttackDamage, specialAttackRange, specialAttackOffset);
    }

    // --- ORTAK HASAR FONKSÝYONU ---
    void DealDamage(int dmg, float range, float offset)
    {
        float direction = Mathf.Sign(transform.localScale.x);
        Vector2 hitPos = new Vector2(transform.position.x + (direction * offset), transform.position.y);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(hitPos, range, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyController enemyScript = enemy.GetComponent<EnemyController>();
            if (enemyScript != null) enemyScript.TakeDamage(dmg);
        }
    }

    void ResetCombo()
    {
        comboStep = 0;
        comboTimer = 0f;
    }

    void OnDrawGizmosSelected()
    {
        float direction = Mathf.Sign(transform.localScale.x);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector2(transform.position.x + (direction * groundAttackOffset), transform.position.y), groundAttackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(new Vector2(transform.position.x + (direction * specialAttackOffset), transform.position.y), specialAttackRange);
    }
}