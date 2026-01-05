using UnityEngine;
using System.Collections.Generic;

public class ExitDoorManager : MonoBehaviour
{
    [Header("Yönetilecek Kapý")]
    public GameObject teleportDoorObject; // Iþýnlanma Kapýsý

    [Header("Görsel Efektler (Temizlenecek)")]
    public GameObject demonFilterUI; // Odayý kýrmýzý yapan panel (Kapanacak)
    public GameObject playerObj;     // Oyuncu (Rengi düzelecek)

    [Header("Ölmesi Gereken Düþmanlar")]
    public List<GameObject> enemiesToKill;

    private bool isLevelCleared = false;

    void Start()
    {
        // 1. Kapýyý Gizle
        if (teleportDoorObject != null)
            teleportDoorObject.SetActive(false);
    }

    void Update()
    {
        // Level zaten bittiyse veya liste boþsa bakma
        if (isLevelCleared || enemiesToKill.Count == 0) return;

        // 2. Düþmanlarý kontrol et
        if (AreAllEnemiesDead())
        {
            FinishLevel();
        }
    }

    void FinishLevel()
    {
        isLevelCleared = true;
        Debug.Log("Oda Temizlendi! Demon Modu kapandý, Kapý açýldý.");

        // A. Kapýyý Aç
        if (teleportDoorObject != null)
            teleportDoorObject.SetActive(true);

        // B. Demon Efektini Kapat (Kýrmýzý Ekran Gitsin)
        if (demonFilterUI != null)
            demonFilterUI.SetActive(false);

        // C. Oyuncunun Rengini Normale Döndür (Beyaz)
        if (playerObj != null)
        {
            playerObj.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    bool AreAllEnemiesDead()
    {
        foreach (GameObject enemy in enemiesToKill)
        {
            // Düþman hala varsa "Bitmedi" de
            if (enemy != null && enemy.activeInHierarchy)
            {
                return false;
            }
        }
        return true;
    }
}