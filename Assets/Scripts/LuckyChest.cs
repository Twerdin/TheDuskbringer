using UnityEngine;

public class LuckyChest : MonoBehaviour
{
    [Header("Ayarlar")]
    public GameObject itemToDrop; // Ýksir Prefab'ý buraya

    private bool isOpen = false;
    private bool isPlayerInRange = false;

    private Animator anim;

    void Start()
    {
        // Animator'ü bul
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !isOpen)
        {
            OpenChest();
        }
    }

    void OpenChest()
    {
        isOpen = true;

        // --- DÜZELTME BURADA ---
        if (anim != null)
        {
            // Senin Animator ayarlarýnda isim "IsOpened" ve türü Bool (true/false)
            anim.SetBool("IsOpened", true);
        }

        // Ýtemi fýrlat
        if (itemToDrop != null)
        {
            Instantiate(itemToDrop, transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) isPlayerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) isPlayerInRange = false;
    }
}