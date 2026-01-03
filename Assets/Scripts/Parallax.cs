using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startpos;
    public GameObject cam;          // Ana Kamerayý buraya sürükleyeceðiz
    public float parallaxEffect;    // 0 = Hareket etmez, 1 = Kamera ile birebir gider (Tam arkaplan), 0.5 = Yarým hýz

    void Start()
    {
        startpos = transform.position.x;
        // Görselin geniþliðini otomatik alýr
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        // Kameranýn ne kadar ilerlediðini hesapla (Parallax efekti için)
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);

        // Arka planý yeni pozisyonuna taþý
        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        // Sonsuz döngü mantýðý: Eðer kamera görselin sýnýrýný aþtýysa, görseli ileri/geri ýþýnla
        if (temp > startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;
    }
}