using UnityEngine;

public class ExampleClass : MonoBehaviour
{
    public float rayDistance = 1f;  // Raycastの距離
    public float rotationAngle = 30f;  // 回転する角度
    public float speed = 5f;  // 物体の速度

    void FixedUpdate()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        // Raycastを使用して前方の障害物を検出
        if (Physics.Raycast(transform.position, fwd, out hit, rayDistance))
        {
            // 障害物を検出した場合、物体を回転させる
            transform.Rotate(0, rotationAngle, 0);

            // 障害物の情報をコンソールに表示
            Debug.Log("Obstacle detected! Obstacle name: " + hit.collider.gameObject.name + ", Distance to obstacle: " + hit.distance);
        }

        // 物体を前方に移動させる
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
