using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    private float length, startpos;
    public GameObject cam;
    public float parallaxEffect; // 0 ile 1 arasý deðer (1 = Hiç hareket etmez, 0 = Kamerayla ayný hýzda)

    void Start()
    {
        startpos = transform.position.x;
        // Resmin geniþliðini alýyoruz (SpriteRenderer olduðunu varsayarak)
        length = GetComponent<SpriteRenderer>().bounds.size.x;

        // Eðer kamerayý elle atamazsan otomatik bulsun
        if (cam == null)
            cam = GameObject.Find("Main Camera");
    }

    void Update()
    {
        // Kameranýn ne kadar ilerlediðini hesapla (Parallax hýzýna göre)
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);

        // Objeyi yeni pozisyona taþý
        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        // Sonsuz döngü kontrolü (Resim bitince baþa sarma)
        if (temp > startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;
    }
}