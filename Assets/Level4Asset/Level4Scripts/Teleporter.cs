using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [Header("Işınlanma Hedefi")]
    // Karakterin ışınlanacağı hedef konumu belirten bir Transform.
    public Transform hedefKonum;

    [Header("Ayarlar")]
    public string oyuncuTagi = "Player";
    
    [Tooltip("Işınlandıktan sonra tekrar ışınlanabilmek için geçmesi gereken süre (saniye).")]
    public float sogumaSuresi = 1.0f; 
    
    [Tooltip("Işınlanınca oyuncunun hızını sıfırla (Rigidbody varsa).")]
    public bool hiziSifirla = true;

    // Statik değişken: Tüm Teleporter'lar ortak bir zamanlayıcı kullanır.
    // Bu sayede A'dan B'ye gidince, B'nin içindeyken hemen geri ışınlanmazsın.
    private static float sonIsinlanmaZamani; 

    void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Oyuncu kontrolü
        // 2. Hedef kontrolü
        // 3. Soğuma süresi kontrolü (Şu anki zaman > Son ışınlanma + bekleme süresi)
        if (other.CompareTag(oyuncuTagi) && hedefKonum != null && Time.time > sonIsinlanmaZamani + sogumaSuresi)
        {
            Isinla(other);
        }
        else if (hedefKonum == null && other.CompareTag(oyuncuTagi))
        {
            Debug.LogWarning("Hedef konum atanmamış! Lütfen Inspector'dan atama yapın.");
        }
    }

    void Isinla(Collider2D player)
    {
        // Pozisyonu güncelle
        player.transform.position = hedefKonum.position;

        // Son ışınlanma zamanını kaydet
        sonIsinlanmaZamani = Time.time;

        // Rigidbody varsa hızını sıfırla (İsteğe bağlı)
        if (hiziSifirla)
        {
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }

        Debug.Log($"{player} başarıyla {hedefKonum.name} konumuna ışınlandı.");
    }

    // Editör görselleri
    void OnDrawGizmos()
    {
        // Giriş noktasını çiz
        Gizmos.color = Color.blue;
        
        // HATA DÜZELTMESİ: Collider yoksa varsayılan bir boyut kullan (hata vermemesi için)
        var col = GetComponent<Collider2D>();
        Vector3 size = col != null ? col.bounds.size : new Vector3(1, 1, 1);
        
        Gizmos.DrawWireCube(transform.position, size);

        // Hedef ve Çizgi
        if (hedefKonum != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(hedefKonum.position, new Vector3(0.5f, 0.5f, 0.5f));
            Gizmos.DrawLine(transform.position, hedefKonum.position);
        }
    }
}
