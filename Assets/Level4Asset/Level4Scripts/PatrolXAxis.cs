using UnityEngine;

public class PatrolXAxis : MonoBehaviour
{
    public float startX = -5f; 
    public float endX = 5f;    
    public float speed = 2f;

    private Vector3 startPosition;
    private Vector3 endPosition;
    private float journeyLength;

    void Start()
    {
        
        startPosition = new Vector3(startX, transform.position.y, transform.position.z);
        endPosition = new Vector3(endX, transform.position.y, transform.position.z);
        journeyLength = Vector3.Distance(startPosition, endPosition);
    }

    void Update()
    {
        
        float distCovered = Mathf.PingPong(Time.time * speed, journeyLength);
        transform.position = Vector3.Lerp(startPosition, endPosition, distCovered / journeyLength);
    }
}
