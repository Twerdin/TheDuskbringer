using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    private float length, startpos;
    public GameObject cam;
    public float parallaxEffect; // 0 ile 1 aras� de�er (1 = Hi� hareket etmez, 0 = Kamerayla ayn� h�zda)

    void Start()
    {
        startpos = transform.position.x;
        // Resmin geni�li�ini al�yoruz (SpriteRenderer oldu�unu varsayarak)
        length = GetComponent<SpriteRenderer>().bounds.size.x;

        // E�er kameray� elle atamazsan otomatik bulsun
        if (cam == null)
            cam = GameObject.Find("Main Camera");
    }

    void Update()
    {
        // Kameran�n ne kadar ilerledi�ini hesapla (Parallax h�z�na g�re)
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);

        // Objeyi yeni pozisyona ta��
        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        // Sonsuz d�ng� kontrol� (Resim bitince ba�a sarma)
        if (temp > startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;
    }
} 