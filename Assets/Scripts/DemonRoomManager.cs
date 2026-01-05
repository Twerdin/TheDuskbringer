using UnityEngine;
using System.Collections.Generic;

public class DemonRoomManager : MonoBehaviour
{
    [Header("Görsel Efektler")]
    public GameObject demonFilterUI; // Kýrmýzý ekran paneli

    [Header("Oyuncu Ayarlarý (YENÝ)")]
    public GameObject playerObj; // Buraya Player'ý sürükle
    public Color playerDemonColor = Color.red; // Oyuncu hangi renk olsun?

    [Header("Güçlenecek Düþmanlar")]
    public List<EnemyController> enemiesInRoom;

    [Header("Düþman Güç Çarpanlarý")]
    public float healthMultiplier = 2f;
    public float damageMultiplier = 2f;
    public float speedMultiplier = 1.5f;

    private bool hasTriggered = false;

    // --- TETÝKLEME KISMI ---
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Oyuncu kutuya girince çalýþtýr
        if (other.CompareTag("Player") && !hasTriggered)
        {
            ActivateDemonMode();
        }
    }

    public void ActivateDemonMode()
    {
        hasTriggered = true;
        Debug.Log("DEMON MODU! Her yer kan gölüne dönüyor...");

        // 1. Kýrmýzý Ekraný Aç
        if (demonFilterUI != null) demonFilterUI.SetActive(true);

        // 2. OYUNCUYU ETKÝLE (YENÝ KISIM)
        if (playerObj != null)
        {
            // A. Rengini Deðiþtir (Düþmanlar gibi kýrmýzý olsun)
            SpriteRenderer playerSprite = playerObj.GetComponent<SpriteRenderer>();
            if (playerSprite != null)
            {
                playerSprite.color = playerDemonColor;
            }

            // B. (Opsiyonel) Oyuncuyu Yavaþlatmak istersen bu kodu aç:
            // playerObj.GetComponent<PlayerMovement>().moveSpeed *= 0.8f; // %20 Yavaþlat

            // C. (Opsiyonel) Oyuncuyu Güçlendirmek (Rage Mode) istersen:
            // playerObj.GetComponent<PlayerCombat>().attackDamage *= 2; 
        }

        // 3. Düþmanlarý Güçlendir ve Boya
        foreach (EnemyController enemy in enemiesInRoom)
        {
            if (enemy != null && enemy.gameObject.activeInHierarchy)
            {
                // Can, Hasar ve Hýz Artýþý
                enemy.maxHealth = (int)(enemy.maxHealth * healthMultiplier);
                enemy.currentHealth = enemy.maxHealth;
                enemy.damageAmount = (int)(enemy.damageAmount * damageMultiplier);
                enemy.walkSpeed *= speedMultiplier;
                enemy.runSpeed *= speedMultiplier;

                // Görsel Deðiþim
                enemy.transform.localScale *= 1.2f; // Büyüt
                enemy.GetComponent<SpriteRenderer>().color = Color.red; // Kýzart
            }
        }

        // Kutuyu kapat
        GetComponent<Collider2D>().enabled = false;
    }
}