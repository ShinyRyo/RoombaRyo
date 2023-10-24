using UnityEngine;

public class RandomWalker : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 360f;  // degrees per second
    public float wallDetectDistance = 1f;

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, wallDetectDistance))
        {
            float randomAngle = Random.Range(0f, 360f);
            transform.Rotate(0, randomAngle, 0);
        }
    }
}
