using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startpos;
<<<<<<< HEAD:Assets/Scripts/ParallaxLayer.cs
    private float startposY;
    public GameObject cam;
    
    [Header("Parallax AyarlarÄ±")]
    [Tooltip("1 = Arkaplan hiÃ§ kÄ±mÄ±ldamaz (GÃ¶kyÃ¼zÃ¼), 0 = Karakterle aynÄ± hÄ±zda gider")]
    public float parallaxEffect; 

    [Tooltip("Bu katman sonsuza kadar kendini tekrar etsin mi?")]
    public bool isInfinite; 
=======
    public GameObject cam;          // Ana Kamerayý buraya sürükleyeceðiz
    public float parallaxEffect;    // 0 = Hareket etmez, 1 = Kamera ile birebir gider (Tam arkaplan), 0.5 = Yarým hýz
>>>>>>> main:Assets/Scripts/Parallax.cs

    void Start()
    {
        startpos = transform.position.x;
<<<<<<< HEAD:Assets/Scripts/ParallaxLayer.cs
        startposY = transform.position.y;
        
        
        // Sprite'Ä±n uzunluÄŸunu otomatik alÄ±yoruz
        length = GetComponent<SpriteRenderer>().bounds.size.x;

        if (cam == null)
            cam = Camera.main.gameObject;
=======
        // Görselin geniþliðini otomatik alýr
        length = GetComponent<SpriteRenderer>().bounds.size.x;
>>>>>>> main:Assets/Scripts/Parallax.cs
    }

    void LateUpdate() // Kamera takibi iÃ§in LateUpdate en iyisidir, titremeyi Ã¶nler
    {
<<<<<<< HEAD:Assets/Scripts/ParallaxLayer.cs
        // KameranÄ±n ne kadar hareket ettiÄŸine gÃ¶re 'sanal' pozisyon (temp) ve 'gerÃ§ek' hareket (dist)
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);

        // Objeyi yeni pozisyona taÅŸÄ± (Y pozisyonunu koruyarak)
        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        // EÄŸer sonsuz dÃ¶ngÃ¼ aÃ§Ä±ksa resim bittiÄŸinde baÅŸa sar
        if (isInfinite)
        {
            if (temp > startpos + length) startpos += length;
            else if (temp < startpos - length) startpos -= length;
        }
=======
        // Kameranýn ne kadar ilerlediðini hesapla (Parallax efekti için)
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);

        // Arka planý yeni pozisyonuna taþý
        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        // Sonsuz döngü mantýðý: Eðer kamera görselin sýnýrýný aþtýysa, görseli ileri/geri ýþýnla
        if (temp > startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;
>>>>>>> main:Assets/Scripts/Parallax.cs
    }
}