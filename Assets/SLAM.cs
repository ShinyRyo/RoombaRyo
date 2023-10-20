using UnityEngine;
using System.Collections.Generic;

public class SLAM : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 360f;
    public float sensorRange = 1f;  // センサーの範囲を更新
    public float sensorOffset = 1f;
    public float changeDirectionDelay = 1f;
    private float changeDirectionCooldown = 0.1f;

    private Dictionary<Vector3, bool> map = new Dictionary<Vector3, bool>();

    void Update()
    {
        UpdateCooldown();
        SimulateSensors();
    }

    void UpdateCooldown()// 方向変更のクールダウンタイマーを更新
    {
        if (changeDirectionCooldown > 0)
        {
            changeDirectionCooldown -= Time.deltaTime;
        }
    }

    void SimulateSensors()// 3つのセンサーをシミュレートする、1つはロボットの中央、残りの2つはロボットの両側
    {
        RaycastHit hit;
        Vector3 leftSensorPosition = transform.position + transform.right * -sensorOffset;
        Vector3 rightSensorPosition = transform.position + transform.right * sensorOffset;
        Vector3 centerSensorPosition = transform.position;

        // サイドセンサーのセンサー範囲をオフセットに基づいて調整
        float adjustedSensorRange = sensorRange / Mathf.Cos(sensorOffset * Mathf.Deg2Rad);

        if (DetectObstacle(leftSensorPosition, transform.forward, out hit, adjustedSensorRange) ||
            DetectObstacle(rightSensorPosition, transform.forward, out hit, adjustedSensorRange) ||
            DetectObstacle(centerSensorPosition, transform.forward, out hit, sensorRange))
        {
            UpdateMap(hit.point);
            ChangeDirection();
        }
        else
        {
            MoveForward();
        }
    }

    bool DetectObstacle(Vector3 position, Vector3 direction, out RaycastHit hit, float range)// デバッグラインを描画
    {
        Debug.DrawLine(position, position + direction * range, Color.red, 0.1f);  
        return Physics.Raycast(position, direction, out hit, range);
    }

    void UpdateMap(Vector3 hitPoint)// 検出された障害物の位置でマップを更新
    {
        map[hitPoint] = true;
    }

    void ChangeDirection()// ロボットの方向をランダムに変更
    {
        float randomAngle = Random.Range(0f, 360f);
        transform.Rotate(0, randomAngle, 0);
        changeDirectionCooldown = changeDirectionDelay;
    }

    void MoveForward()// ロボットを前進させる
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
