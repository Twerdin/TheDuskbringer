using UnityEngine;

public class TimedPlatform : MonoBehaviour
{
    [Header("Bireysel Süre Ayarý")]
    [Tooltip("Bu platform tetiklendikten kaç saniye sonra düþsün?")]
    public float fallDelay = 5f; // Varsayýlan 5. Burayý Inspector'dan deðiþtireceksin.

    // Tetikleyici bu fonksiyonu çaðýracak
    public void StartCountdown()
    {
        // Her platform kendi 'fallDelay' süresini sayar
        Invoke("DropPlatform", fallDelay);
    }

    void DropPlatform()
    {
        // Platformu kapat (veya animasyon oynat)
        gameObject.SetActive(false);

        // Eðer geri gelmesini istersen ilerde buraya kod ekleriz
        // Debug.Log(gameObject.name + " düþtü!");
    }
}