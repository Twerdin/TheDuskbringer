using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Gerekli Bileþenler")]
    public PlayerMovement playerController;
    private Animator anim;

    [Header("Genel Ayarlar")]
    public LayerMask enemyLayers; // Inspector'da 'Enemy' seçili olmalý!

    [Header("Skill Kilidi")]
    public bool isSpecialAttackUnlocked = false;

    [Header("Yer Saldýrýsý (J Tuþu)")]
    // Offset'i 0 yapma! 0.8 veya 1 gibi bir deðer ver ki karakterin önü olsun.
    public float groundAttackOffset = 1.0f;
    public float groundAttackRange = 1.0f;  // Menzili biraz artýrdým
    public int groundDamage = 20;
    public float comboResetTime = 1.0f;

    [Header("Özel Saldýrý (Q Tuþu)")]
    public KeyCode specialAttackInput = KeyCode.Q;
    public string specialAttackAnimName = "Special_Attack";
    public float specialAttackOffset = 1.2f; // Bunu da 0 yapma
    public float specialAttackRange = 1.5f;
    public int specialAttackDamage = 25;

    private int comboStep = 0;
    private float comboTimer = 0f;
    private float nextAttackTime = 0f;
    public float attackRate = 2f;

    void Start()
    {
        anim = GetComponent<Animator>();
        if (playerController == null)
            playerController = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        // Kombo zamanlayýcýsý
        if (comboStep > 0)
        {
            comboTimer += Time.deltaTime;
            if (comboTimer >= comboResetTime) ResetCombo();
        }

        if (Time.time >= nextAttackTime)
        {
            // ÖZEL SALDIRI
            if (Input.GetKeyDown(specialAttackInput) && isSpecialAttackUnlocked)
            {
                PerformSpecialAttack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
            // NORMAL SALDIRI
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

        // Animasyonlarý tetikle
        if (comboStep == 1) anim.SetTrigger("Attack1");
        else if (comboStep == 2) anim.SetTrigger("Attack2");
        else if (comboStep == 3) anim.SetTrigger("Attack3");

        // Hasar Ver
        DealDamage(groundDamage, groundAttackRange, groundAttackOffset);

        if (comboStep >= 3) comboStep = 0;
    }

    void PerformSpecialAttack()
    {
        anim.SetTrigger(specialAttackAnimName);
        DealDamage(specialAttackDamage, specialAttackRange, specialAttackOffset);
    }

    // --- DETAYLI HASAR FONKSÝYONU ---
    void DealDamage(int dmg, float range, float offset)
    {
        float direction = transform.localScale.x > 0 ? 1 : -1;
        Vector2 hitPos = new Vector2(transform.position.x + (direction * offset), transform.position.y);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(hitPos, range, enemyLayers);

        if (hitEnemies.Length == 0)
        {
            Debug.Log("Menzilde kimse yok.");
            return;
        }

        foreach (Collider2D enemy in hitEnemies)
        {
            // 1. Önce Standart Düþmaný Ara (EnemyController)
            EnemyController enemyScript = enemy.GetComponent<EnemyController>();
            if (enemyScript == null) enemyScript = enemy.GetComponentInParent<EnemyController>();

            if (enemyScript != null)
            {
                enemyScript.TakeDamage(dmg);
                Debug.Log("Vuruþ: Standart Düþman");
                continue; // Sýradaki düþmana geç
            }

            // 2. DEMON BOSS KONTROLÜ (YENÝ EKLEDÝÐÝMÝZ KISIM)
            // Senin Boss scriptinin adý neyse buraya onu yazmalýsýn!
            // Ben 'DemonBossController' olarak yazdým.
            DemonBossController bossScript = enemy.GetComponent<DemonBossController>();
            if (bossScript == null) bossScript = enemy.GetComponentInParent<DemonBossController>();

            if (bossScript != null)
            {
                bossScript.TakeDamage(dmg);
                Debug.Log("Vuruþ: DEMON BOSS CANI YANDI!");
                continue;
            }

            // 3. Eðer ikisi de yoksa eski sistemi dene (EnemyHealth)
            EnemyHealth oldHealth = enemy.GetComponent<EnemyHealth>();
            if (oldHealth != null)
            {
                oldHealth.TakeDamage(dmg);
                Debug.Log("Vuruþ: Eski Health Scripti");
            }
        }
    }

    void ResetCombo()
    {
        comboStep = 0;
        comboTimer = 0f;
    }

    // Menzili sahnede çiz
    void OnDrawGizmosSelected()
    {
        float direction = transform.localScale.x > 0 ? 1 : -1;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector2(transform.position.x + (direction * groundAttackOffset), transform.position.y), groundAttackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(new Vector2(transform.position.x + (direction * specialAttackOffset), transform.position.y), specialAttackRange);
    }
}