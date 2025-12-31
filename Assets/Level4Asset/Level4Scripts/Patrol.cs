using UnityEngine;

public class Patrol : MonoBehaviour
{
    public float startY = 0f;
    public float endY = 5f;
    public float speed = 2f;

    private Vector3 startPosition;
    private Vector3 endPosition;
    private float journeyLength;

    void Start()
    {
        startPosition = new Vector3(transform.position.x, startY, transform.position.z);
        endPosition = new Vector3(transform.position.x, endY, transform.position.z);
        journeyLength = Vector3.Distance(startPosition, endPosition);
    }

    void Update()
    {
        
        float distCovered = Mathf.PingPong(Time.time * speed, journeyLength);
        transform.position = Vector3.Lerp(startPosition, endPosition, distCovered / journeyLength);
    }
}
