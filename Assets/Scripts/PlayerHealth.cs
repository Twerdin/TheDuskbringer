using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Can Ayarlarý")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Ýksir Ayarlarý (YENÝ)")]
    public bool hasPotion = false; // Cebinde iksir var mý?
    public int potionHealAmount = 30; // Ýksir kaç can doldursun?

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        // YENÝ KISIM: F Tuþuna basýnca Ýksir Ýçme
        if (Input.GetKeyDown(KeyCode.F))
        {
            UsePotion();
        }
    }

    void UsePotion()
    {
        // Eðer iksirimiz yoksa veya canýmýz zaten full ise içme
        if (!hasPotion)
        {
            Debug.Log("Ýksirin yok!");
            return;
        }

        if (currentHealth >= maxHealth)
        {
            Debug.Log("Canýn zaten dolu!");
            return;
        }

        // Ýksiri Ýç:
        Heal(potionHealAmount);
        hasPotion = false; // Ýksiri tükettik
        Debug.Log("Gluk gluk... Ýksir içildi!");
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        // Buraya UI can barý güncelleme kodunu da ekleyebilirsin
        // healthBar.SetHealth(currentHealth);
    }

    // (Senin mevcut TakeDamage fonksiyonun burada durabilir)
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        // ... senin ölüm kodlarýn ...
    }
}