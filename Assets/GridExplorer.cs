using UnityEngine;
using System.Collections.Generic;

public class GridExplorer : MonoBehaviour
{
    public Vector2 gridSize = new Vector2(10, 10);
    public float cellSize = 1f;
    public float speed = 1f;
    private HashSet<Vector2Int> visitedCells = new HashSet<Vector2Int>();
    private Vector2Int currentCell;
    public float obstacleDetectDistance = 0.5f;  // 障害物を検出する距離

    void Start()
    {
        Vector3 startPosition = transform.position;
        currentCell = WorldToCell(startPosition);
        visitedCells.Add(currentCell);
    }

    void Update()
    {
        Vector2Int[] neighbors = GetNeighbors(currentCell);
        foreach (Vector2Int neighbor in neighbors)
        {
            if (!visitedCells.Contains(neighbor))
            {
                Vector3 targetPosition = CellToWorld(neighbor);
                Vector3 direction = (targetPosition - transform.position).normalized;
                if (IsObstacleInDirection(direction))
                {
                    direction = GetNewDirection();  // 障害物がある場合、新しい方向を計算
                }
                if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
                {
                    transform.position += direction * speed * Time.deltaTime;
                }
                else
                {
                    currentCell = neighbor;
                    visitedCells.Add(neighbor);
                }
                break;
            }
        }
    }

    Vector2Int[] GetNeighbors(Vector2Int cell)
    {
        return new Vector2Int[]
        {
            new Vector2Int(cell.x + 1, cell.y),
            new Vector2Int(cell.x - 1, cell.y),
            new Vector2Int(cell.x, cell.y + 1),
            new Vector2Int(cell.x, cell.y - 1)
        };
    }

    Vector2Int WorldToCell(Vector3 worldPosition)
    {
        return new Vector2Int(Mathf.FloorToInt(worldPosition.x / cellSize), Mathf.FloorToInt(worldPosition.z / cellSize));
    }

    Vector3 CellToWorld(Vector2Int cell)
    {
        return new Vector3(cell.x * cellSize, transform.position.y, cell.y * cellSize);
    }

    bool IsObstacleInDirection(Vector3 direction)
    {
        // 指定された方向で障害物を検出
        return Physics.Raycast(transform.position, direction, obstacleDetectDistance);
    }

    Vector3 GetNewDirection()
    {
        // 障害物を避けるための新しい方向を計算 (この例ではランダムに選択)
        float randomAngle = Random.Range(0f, 360f);
        return new Vector3(Mathf.Cos(randomAngle * Mathf.Deg2Rad), 0, Mathf.Sin(randomAngle * Mathf.Deg2Rad));
    }
}
