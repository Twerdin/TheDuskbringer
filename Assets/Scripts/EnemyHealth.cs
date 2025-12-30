using UnityEngine;
using UnityEngine.UI; // UI kullanacaðýmýz için þart

public class EnemyHealth : MonoBehaviour
{
    [Header("Can Ayarlarý")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Bileþenler")]
    public Slider healthSlider; // Kafasýnýn üzerindeki bar
    private EnemyController enemyController; // Hareket koduna eriþmek için

    void Start()
    {
        currentHealth = maxHealth;
        enemyController = GetComponent<EnemyController>();

        // Slider ayarlarý
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damage;

        // UI Güncelle
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        // Can bittiyse ÖL, bitmediyse HURT animasyonu oynat
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // EnemyController içindeki TakeDamage fonksiyonunu çaðýr (Hurt animasyonu için)
            if (enemyController != null)
                enemyController.TakeDamage();
        }
    }

    void Die()
    {
        Debug.Log("Düþman Öldü!");

        // EnemyController içindeki Die fonksiyonunu çaðýr (Hareketi kesmek için)
        if (enemyController != null)
            enemyController.Die();

        // Can barýný gizle
        if (healthSlider != null)
            healthSlider.gameObject.SetActive(false);

        // Collider'ý kapat ki kýlýç içinden geçsin
        GetComponent<Collider2D>().enabled = false;

        // 5 Saniye sonra cesedi yok et (Ýstersen kapatabilirsin)
        Destroy(gameObject, 5f);
    }
}