using System.Collections;
using UnityEngine;

public class CrumblingPlatform : MonoBehaviour
{
    [Header("Ayarlar")]
    public float beklemeSuresi = 2.0f;
    public float maxTitremeSiddeti = 0.1f;
    public float geriGelmeSuresi = 3.0f;

    [Header("Görsel Efektler")]
    public GameObject tozEfektiPrefab; // Buraya toz/parçalanma efektini sürükle

    private Vector3 orijinalPozisyon;
    private bool kiriliyorMu = false;
    private BoxCollider2D col;
    private SpriteRenderer spr;

    void Start()
    {
        orijinalPozisyon = transform.position;
        col = GetComponent<BoxCollider2D>();
        spr = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !kiriliyorMu)
        {
            StartCoroutine(PlatformuKir());
        }
    }

    IEnumerator PlatformuKir()
    {
        kiriliyorMu = true;

        // --- 1. EFEKTÝ HEMEN BAÞLAT ---
        GameObject aktifEfekt = null; // Geçici bir kutu oluþturduk

        if (tozEfektiPrefab != null)
        {
            // Efekti yarat
            aktifEfekt = Instantiate(tozEfektiPrefab, transform.position, Quaternion.identity);

            // Efekti taþýn "çocuðu" yap ki taþ sallanýnca efekt de sallansýn
            aktifEfekt.transform.SetParent(transform);
        }

        // --- 2. TÝTREME SÜRECÝ ---
        float sayac = 0f;
        while (sayac < beklemeSuresi)
        {
            sayac += Time.deltaTime;
            float ilerleme = sayac / beklemeSuresi;
            float guncelSiddet = Mathf.Lerp(0f, maxTitremeSiddeti, ilerleme);

            // Taþ sallanýyor (Efekt de child olduðu için onunla sallanacak)
            transform.position = orijinalPozisyon + (Vector3)Random.insideUnitCircle * guncelSiddet;

            yield return null;
        }

        // --- 3. KIRILMA VE YOK ETME ---

        // Efekti sahnede yok et (Toz üretimi dursun ve obje silinsin)
        if (aktifEfekt != null)
        {
            // Efekti taþtan ayýr (yoksa taþ gizlenince o da gizlenir)
            aktifEfekt.transform.SetParent(null);

            // Toz üretimini durdur (Var olanlar süzülüp kaybolsun)
            var ps = aktifEfekt.GetComponent<ParticleSystem>();
            ps.Stop();

            // 2 saniye sonra tamamen yok et (Ram temizliði)
            Destroy(aktifEfekt, 2.0f);
        }
        // Platformu gizle
        PlatformuGizle();

        // Geri gelme mantýðý
        if (geriGelmeSuresi > 0)
        {
            yield return new WaitForSeconds(geriGelmeSuresi);
            PlatformuGeriGetir();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void PlatformuGizle()
    {
        spr.enabled = false;
        col.enabled = false;
    }

    void PlatformuGeriGetir()
    {
        transform.position = orijinalPozisyon;
        spr.enabled = true;
        col.enabled = true;
        kiriliyorMu = false;
    }
}