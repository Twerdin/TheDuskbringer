using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    private float length, startpos;
    private float startposY;
    public GameObject cam;
    
    [Header("Parallax Ayarları")]
    [Tooltip("1 = Arkaplan hiç kımıldamaz (Gökyüzü), 0 = Karakterle aynı hızda gider")]
    public float parallaxEffect; 

    [Tooltip("Bu katman sonsuza kadar kendini tekrar etsin mi?")]
    public bool isInfinite; 

    void Start()
    {
        startpos = transform.position.x;
        startposY = transform.position.y;
        
        
        // Sprite'ın uzunluğunu otomatik alıyoruz
        length = GetComponent<SpriteRenderer>().bounds.size.x;

        if (cam == null)
            cam = Camera.main.gameObject;
    }

    void LateUpdate() // Kamera takibi için LateUpdate en iyisidir, titremeyi önler
    {
        // Kameranın ne kadar hareket ettiğine göre 'sanal' pozisyon (temp) ve 'gerçek' hareket (dist)
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);

        // Objeyi yeni pozisyona taşı (Y pozisyonunu koruyarak)
        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        // Eğer sonsuz döngü açıksa resim bittiğinde başa sar
        if (isInfinite)
        {
            if (temp > startpos + length) startpos += length;
            else if (temp < startpos - length) startpos -= length;
        }
    }
}