using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    Transform cam;
    Vector3 camStartPos;

    GameObject[] backgrounds;
    Material[] materials;

    float distance;

    [Range(0f, 5f)]
    public float parallaxSpeed = 1f;

    [Tooltip("Her layer için manuel hýz (0 = sabit, 1 = kamera ile ayný)")]
    public float[] layerSpeeds;

    void Start()
    {
        cam = Camera.main.transform;
        camStartPos = cam.position;

        int count = transform.childCount;

        backgrounds = new GameObject[count];
        materials = new Material[count];

        // Inspector'dan girilmemiþse otomatik oluþtur
        if (layerSpeeds == null || layerSpeeds.Length != count)
        {
            layerSpeeds = new float[count];
            for (int i = 0; i < count; i++)
                layerSpeeds[i] = i * 0.15f; // default daðýlým
        }

        for (int i = 0; i < count; i++)
        {
            backgrounds[i] = transform.GetChild(i).gameObject;
            materials[i] = backgrounds[i].GetComponent<Renderer>().sharedMaterial;
        }
    }

    void LateUpdate()
    {
        distance = cam.position.x - camStartPos.x;

        for (int i = 0; i < backgrounds.Length; i++)
        {
            float offset = distance * layerSpeeds[i] * parallaxSpeed;

            // Cinemachine jitter azaltma
            offset = Mathf.Round(offset * 5000f) / 5000f;
            offset %= 1f;

            materials[i].SetTextureOffset("_MainTex", new Vector2(offset, 0));
        }
    }
}