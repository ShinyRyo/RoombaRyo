using UnityEngine;

public class SLAM : MonoBehaviour
{
    public float speed = 5f;
    public float roombaWidth = 0.5f;  // ルンバの幅
    public float rotationSpeed = 360f;
    public float sensorRange = 1f;
    public float sensorOffset = 1f;
    public float changeDirectionDelay = 1f;
    private float changeDirectionCooldown = 0.1f;
    public Color paintColor = Color.white;  // ペンキの色。デフォルトは白
    public Material paintMaterial;  // 新しい public Material 変数

<<<<<<< HEAD:Assets/SLAM.cs
    private TrailRenderer trailRenderer;
    void Start()
    {
        trailRenderer = gameObject.GetComponent<TrailRenderer>();
        if(trailRenderer == null) 
        {
            trailRenderer = gameObject.AddComponent<TrailRenderer>();
        }
        trailRenderer.material = paintMaterial;  // トレイルレンダラーのマテリアルを設定
        trailRenderer.widthMultiplier = roombaWidth;
        trailRenderer.time = Mathf.Infinity;
    }

    void Update()
    {
        HandleMovement();
        // TrailRenderer の色を更新
        UpdateTrailColor();
=======
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
>>>>>>> origin/main:Assets/Scripts/SLAM.cs
    }
    void UpdateTrailColor()
    {
        if(trailRenderer != null)
        {
            trailRenderer.startColor = paintColor;
            trailRenderer.endColor = paintColor;
        }
    }
    void HandleMovement()
    {
        if (IsObstacleDetected())
        {
            ChangeDirection();
        }
        else
        {
            MoveForward();
        }
    }

    bool IsObstacleDetected()
    {
        RaycastHit hit;
        Vector3 leftSensorPosition = transform.position + transform.right * -sensorOffset;
        Vector3 rightSensorPosition = transform.position + transform.right * sensorOffset;
        Vector3 centerSensorPosition = transform.position;

        float adjustedSensorRange = sensorRange / Mathf.Cos(sensorOffset * Mathf.Deg2Rad);

        return Physics.Raycast(leftSensorPosition, transform.forward, out hit, adjustedSensorRange) ||
               Physics.Raycast(rightSensorPosition, transform.forward, out hit, adjustedSensorRange) ||
               Physics.Raycast(centerSensorPosition, transform.forward, out hit, sensorRange);
    }

<<<<<<< HEAD:Assets/SLAM.cs
    void ChangeDirection()
=======
    void UpdateMap(Vector3 hitPoint)// 検出された障害物の位置でマップを更新
    {
        exploredMap[new Vector2Int(Mathf.FloorToInt(hitPoint.x), Mathf.FloorToInt(hitPoint.z))] = true;
    }


    void ChangeDirection()// ロボットの方向をランダムに変更
>>>>>>> origin/main:Assets/Scripts/SLAM.cs
    {
        float randomAngle = Random.Range(0f, 360f);
        transform.Rotate(0, randomAngle, 0);
    }

    void MoveForward()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
