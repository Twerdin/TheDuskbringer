using UnityEngine;

/// <summary>
/// Bu script, karakterin yatay hareket girdisine bağlı olarak Collider2D'sinin PhysicsMaterial2D'sini yönetir.
/// Karakter hareket ederken kaygan bir materyal, durduğunda ise yapışkan bir materyal atayarak
/// sürtünmeden kaynaklanan küçük zıplama/takılma sorunlarını önler.
/// Bu script'i Collider2D bileşeni olan oynatıcı GameObject'ine ekleyin.
/// </summary>
public class KarakterFizikMateryalYoneticisi : MonoBehaviour
{
    [Tooltip("Karakter yatayda hareket ederken kullanılacak PhysicsMaterial2D (örn: Friction = 0).")]
    public PhysicsMaterial2D kayganMateryal; // Kaygan materyal

    [Tooltip("Karakter yatayda hareket etmezken kullanılacak PhysicsMaterial2D (örn: Friction = 1).")]
    public PhysicsMaterial2D durmaMateryali; // Durma materyali

    [Tooltip("Karakterin 'hareket ediyor' sayılması için gerekli minimum mutlak yatay girdi değeri.")]
    [Range(0.01f, 0.5f)]
    public float yatayGirdiEsigi = 0.1f; // Yatay girdi eşiği

    private Collider2D karakterCollider; // Karakterin Collider2D bileşenine referans

    void Awake() // Collider'ın diğer script'ler tarafından kullanılmadan önce bulunmasını sağlar
    {
        karakterCollider = GetComponent<Collider2D>();

        if (karakterCollider == null)
        {
            Debug.LogError("KarakterFizikMateryalYoneticisi: Bu GameObject üzerinde Collider2D bileşeni bulunamadı. Script devre dışı bırakılıyor.");
            enabled = false; // Collider bulunamazsa script'i devre dışı bırak
        }
    }

    void Update()
    {
        // Ham yatay girdiyi al (GetAxis'in yumuşatmasını engeller, anlık tepki için Raw iyidir)
        float yatayGirdi = Input.GetAxisRaw("Horizontal");

        // Karakterin önemli bir yatay girdi alıp almadığını kontrol et
        if (Mathf.Abs(yatayGirdi) > yatayGirdiEsigi)
        {
            // Karakter hareket ediyor: Kaygan materyali kullan
            if (kayganMateryal != null && karakterCollider.sharedMaterial != kayganMateryal)
            {
                karakterCollider.sharedMaterial = kayganMateryal;
            }
        }
        else
        {
            // Karakter durdu: Yapışkan materyali kullan
            if (durmaMateryali != null && karakterCollider.sharedMaterial != durmaMateryali)
            {
                karakterCollider.sharedMaterial = durmaMateryali;
            }
        }
    }
}                         