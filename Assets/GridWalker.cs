using UnityEngine;
using System.Collections.Generic;

public class GridWalker : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 360f;  // degrees per second
    public float wallDetectDistance = 1f;
    public Vector2 gridSize = new Vector2(10, 10);
    public float cellSize = 1f;

    private HashSet<Vector2Int> visitedCells = new HashSet<Vector2Int>();
    private Vector2Int currentCell;
    private float changeDirectionCooldown = 0f;
    public float changeDirectionDelay = 0.5f;  // 方向を変更するまでの遅延時間

    void Start()
    {
        Vector3 startPosition = transform.position;
        currentCell = WorldToCell(startPosition);
        visitedCells.Add(currentCell);
    }

void Update()
{
    if (changeDirectionCooldown > 0)
    {
        changeDirectionCooldown -= Time.deltaTime;
        return;
    }

    // 方向変更のロジックをループに入れる
    while (true)
    {
        Vector2Int[] neighbors = GetNeighbors(currentCell);
        bool allVisited = true;  // 初期値は全て訪問済みとする

        foreach (Vector2Int neighbor in neighbors)
        {
            if (!visitedCells.Contains(neighbor))
            {
                allVisited = false;  // 未訪問のセルを見つけたのでフラグを更新
                Vector3 targetPosition = CellToWorld(neighbor);
                Vector3 direction = (targetPosition - transform.position).normalized;
                RaycastHit hit;

                if (!Physics.Raycast(transform.position, direction, out hit, wallDetectDistance))
                {
                    if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
                    {
                        transform.position += direction * speed * Time.deltaTime;
                    }
                    else
                    {
                        currentCell = neighbor;
                        visitedCells.Add(neighbor);
                    }
                    return;  // 正しい方向を見つけたのでループを抜ける
                }
            }
        }

        if (allVisited)
        {
            break;  // 全ての隣接セルが訪問済みなのでループを抜ける
        }

        // 新しい方向を見つけるためにランダムな角度で回転
        float randomAngle = Random.Range(0f, 360f);
        transform.Rotate(0, randomAngle, 0);
        changeDirectionCooldown = changeDirectionDelay;  // 方向変更のクールダウンを設定
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
}
