using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startpos;
    public GameObject cam;

    [Header("Parallax Ayarlarý")]
    [Tooltip("1 = Arkaplan hiç kýmýldamaz (Gökyüzü), 0 = Karakterle ayný hýzda gider")]
    public float parallaxEffect;

    [Tooltip("Bu katman sonsuza kadar kendini tekrar etsin mi?")]
    public bool isInfinite;

    void Start()
    {
        startpos = transform.position.x;

        // Sprite'ýn uzunluðunu otomatik alýyoruz
        length = GetComponent<SpriteRenderer>().bounds.size.x;

        if (cam == null)
            cam = Camera.main.gameObject;
    }

    void LateUpdate() // Kamera takibi için LateUpdate en iyisidir, titremeyi önler
    {
        // Kameranýn ne kadar hareket ettiðine göre 'sanal' pozisyon (temp) ve 'gerçek' hareket (dist)
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);

        // Objeyi yeni pozisyona taþý (Y pozisyonunu koruyarak)
        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        // Eðer sonsuz döngü açýksa resim bittiðinde baþa sar
        if (isInfinite)
        {
            if (temp > startpos + length) startpos += length;
            else if (temp < startpos - length) startpos -= length;
        }
    }
}