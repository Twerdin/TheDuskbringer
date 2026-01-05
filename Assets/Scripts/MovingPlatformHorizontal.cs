using UnityEngine;

public class MovingPlatformHorizontal : MonoBehaviour
{
    public float moveDistance = 3f; // Ne kadar sağ-sol gidecek
    public float speed = 2f;        // Hareket hızı

    private Vector3 startPos;
    private int direction = 1;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.Translate(Vector3.right * direction * speed * Time.deltaTime);

        if (Mathf.Abs(transform.position.x - startPos.x) >= moveDistance)
        {
            direction *= -1; // yön değiştir
        }
    }

    // Player platformun üstündeyken birlikte hareket etsin
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
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
