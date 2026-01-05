using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering; // Volume (Post Processing) için gerekli
using System.Collections;    // Coroutine (Zamanlayıcı) için gerekli

public class PlayerHealth : MonoBehaviour
{
    [Header("Can Ayarları")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI Bağlantıları")]
    public Image healthBarImg;

    [Header("Ölüm Efekti ve Paneli")]
    public Volume deathVolume;       // Sahnedeki 'DeathEffetcs' objesi buraya
    public GameObject gameOverPanel; // Canvas içindeki 'Game Over' paneli buraya
    public float deathDuration = 5f; // Efekt kaç saniye sürsün?

    [Header("İksir Ayarları")]
    public bool hasPotion = false;
    public int potionHealAmount = 30;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();

        // Başlangıçta ekran temiz olsun (Efekt kapalı)
        if (deathVolume != null) deathVolume.weight = 0;

        // Game Over panelini gizle
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    void Update()
    {
        // Öldüysek iksir içemeyelim
        if (Input.GetKeyDown(KeyCode.F) && !isDead)
        {
            UsePotion();
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (isDead) return;
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        UpdateHealthUI();
    }

    void UsePotion()
    {
        if (!hasPotion || currentHealth >= maxHealth) return;
        Heal(potionHealAmount);
        hasPotion = false;
    }

    void UpdateHealthUI()
    {
        if (healthBarImg != null)
        {
            healthBarImg.fillAmount = (float)currentHealth / maxHealth;
        }
    }

    // --- ÖLÜM SENARYOSU ---
    void Die()
    {
        isDead = true;
        Debug.Log("Karakter öldü. Efekt başlıyor...");

        // 1. KARAKTERİ DONDUR (SİLME YOK)
        // Karakterin fiziğini kapatıyoruz ki aşağı düşmesin veya itilmesin
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; // Hızı sıfırla
            rb.simulated = false;       // Fizik hesaplamasını kapat
        }

        // Eğer karakterin hareket scripti varsa onu da buradan kapatmalısın
        // Örneğin: GetComponent<PlayerMovement>().enabled = false;

        // Animasyon varsa 'Dead' animasyonunu tetikleyebilirsin
        // GetComponent<Animator>().SetTrigger("Die");

        // 2. Efekt ve Panel sürecini başlat
        StartCoroutine(DeathSequence());
    }

    // 5 Saniyelik Sinematik Geçiş
    IEnumerator DeathSequence()
    {
        float timer = 0f;

        // Belirlenen süre boyunca (5 sn) döngü çalışır
        while (timer < deathDuration)
        {
            timer += Time.deltaTime;

            // 0'dan 1'e doğru artan oran
            float progress = timer / deathDuration;

            // Volume değerini yavaşça artır
            if (deathVolume != null)
            {
                deathVolume.weight = progress;
            }

            yield return null; // Bir sonraki frame'i bekle
        }

        // Döngü bitti, emin olmak için weight'i tam 1 yap
        if (deathVolume != null) deathVolume.weight = 1f;

        // 3. VE FİNAL: GAME OVER PANELİNİ AÇ
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }
}