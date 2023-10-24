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

    void ChangeDirection()
    {
        float randomAngle = Random.Range(0f, 360f);
        transform.Rotate(0, randomAngle, 0);
    }

    void MoveForward()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
