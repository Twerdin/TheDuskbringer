using UnityEngine;

public class MagicElevator : MonoBehaviour
{
    [Header("Ayarlar")]
    public Transform targetPoint;
    public float speed = 3f;

    [Header("Kilit Sistemi")]
    public bool isLocked = true; // BAÞLANGIÇTA KÝLÝTLÝ OLSUN

    private bool isMoving = false;

    void Update()
    {
        // Hareket baþladýysa yukarý çýk
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
        }
    }

    // --- IÞIK SÝSTEMÝ BURAYI AÇACAK ---
    public void UnlockElevator()
    {
        isLocked = false; // Kilit açýldý, artýk binilebilir!
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Oyuncu çarparsa VE KÝLÝT AÇIKSA (isLocked == false)
        if (collision.gameObject.CompareTag("Player") && !isLocked)
        {
            isMoving = true;
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}