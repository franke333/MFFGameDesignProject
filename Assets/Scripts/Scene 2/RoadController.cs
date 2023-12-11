using UnityEngine;

public class RoadController : MonoBehaviour
{
    private const float speed = 600.0f;
    private const float respawnTime = 4.0f;

    private Vector2 startPos;
    private float accumulator = 1.0f;
    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float directionAngle = transform.localRotation.eulerAngles.z - 90;
        Vector2 direction = new(Mathf.Cos(Mathf.Deg2Rad * directionAngle), Mathf.Sin(Mathf.Deg2Rad * directionAngle));

        transform.position += (Vector3)(speed * Time.deltaTime * direction);
        
        accumulator += Time.deltaTime;
        if (accumulator >= respawnTime)
        {
            accumulator = 0.0f;
            transform.position = startPos;
        }

    }
}
