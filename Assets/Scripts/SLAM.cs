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

    public MapDisplay mapDisplay;  // MapDisplayの参照を保持

    private Dictionary<Vector2Int, bool> exploredMap = new Dictionary<Vector2Int, bool>();


    void Update()
    {
        UpdateCooldown();
        SimulateSensors();
        // 現在のルンバの位置を取得
        Vector2Int currentPosition = new Vector2Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.z));
        // 探索済みのマップデータを更新
        exploredMap[currentPosition] = true;
        // MapDisplayスクリプトを使用してマップを更新
        mapDisplay.UpdateMap(exploredMap);
        Debug.Log("Updated map at position: " + currentPosition);  // ログステートメントを追加
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
        exploredMap[new Vector2Int(Mathf.FloorToInt(hitPoint.x), Mathf.FloorToInt(hitPoint.z))] = true;
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
