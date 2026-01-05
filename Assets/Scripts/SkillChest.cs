using UnityEngine;
using System.Collections.Generic; // Listeleri kullanmak için þart

public class SkillChest : MonoBehaviour
{
    [Header("Baðlantýlar")]
    public GameObject rewardPanel; // Az önce yaptýðýn Panel
    public PlayerCombat playerScript; // Player'ýn kodu (Skilli açmak için)

    [Header("Düþman Kontrolü")]
    // Odayý temizlemeden sandýk açýlmasýn diye düþmanlarý buraya koyacaðýz
    public List<GameObject> enemiesToKill;

    private bool isOpened = false;
    private bool playerIsClose = false;

    void Update()
    {
        // Eðer oyuncu yakýndaysa VE E tuþuna bastýysa VE sandýk daha önce açýlmadýysa
        if (playerIsClose && Input.GetKeyDown(KeyCode.E) && !isOpened)
        {
            CheckEnemiesAndOpen();
        }
    }

    void CheckEnemiesAndOpen()
    {
        // 1. Önce listedeki düþmanlar ölmüþ mü diye bakýyoruz
        foreach (GameObject enemy in enemiesToKill)
        {
            if (enemy != null && enemy.activeInHierarchy)
            {
                Debug.Log("Hala yaþayan düþmanlar var! Sandýk açýlmaz.");
                return; // Biri bile yaþýyorsa iþlemi iptal et
            }
        }

        // 2. Eðer herkes öldüyse sandýðý aç
        OpenTheChest();
    }

    void OpenTheChest()
    {
        isOpened = true;

        // A. Player'ýn Q yeteneðini aktif et
        playerScript.isSpecialAttackUnlocked = true;

        // B. Ekrana o paneli getir
        rewardPanel.SetActive(true);

        // C. Oyunu durdur (Arka planda oyun akmasýn)
        Time.timeScale = 0f;
    }

    // --- BUTON ÝÇÝN FONKSÝYON ---
    // Paneldeki "Kapat" butonuna bunu baðlayacaðýz
    public void CloseButtonAction()
    {
        rewardPanel.SetActive(false); // Paneli kapat
        Time.timeScale = 1f; // Oyunu tekrar baþlat
        this.enabled = false; // Bu scripti kapat (Bir daha açýlmasýn)
    }

    // Oyuncu sandýðýn yanýna geldi mi?
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerIsClose = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerIsClose = false;
    }
}