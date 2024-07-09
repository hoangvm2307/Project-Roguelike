using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public float gridSize = 1f;
    protected Vector2Int currentCell;
    public LayerMask collisionMask;
    public BoxCollider2D boxCollider;
    public PlayerController player;
    public float moveDelay = 1f; // Time between each move
    public GameObject slash;
    public int damage;
    void Start()
    {
        player = PlayerController.Instance;
        boxCollider = GetComponent<BoxCollider2D>();
        currentCell = new Vector2Int(Mathf.RoundToInt(transform.position.x / gridSize), Mathf.RoundToInt(transform.position.y / gridSize));
        EnemyManager.Instance.OccupyPosition(currentCell);
        StartCoroutine(MoveTowardsPlayer());
    }

    protected void OnEnable()
    {

    }

    protected void OnDisable()
    {
        EnemyManager.Instance.VacatePosition(currentCell);
    }

    protected abstract IEnumerator MoveTowardsPlayer();

    public void AttackPlayerAtPosition(Vector2Int position)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector3(position.x + 0.5f, position.y + 0.5f), 0.1f);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag(TagName.Player))
            {
                GameObject player = collider.gameObject;
                HealthUIManager.Instance.TakeDamage(damage);
                if(CameraShake.Instance != null)
                {
                    StartCoroutine(CameraShake.Instance.Shake()); 
                }
                GameObject _slash = Instantiate(slash, new Vector3(position.x + 0.5f, position.y + 0.5f), Quaternion.identity);
                break;
            }
        }
    }
    protected Vector2Int GetRandomWalkableNeighbor(Vector2Int cell)
    {
        List<Vector2Int> neighbors = GetShuffledNeighbors(cell);
        foreach (Vector2Int neighbor in neighbors)
        {
            Vector3 newPosition = new Vector3((neighbor.x + 0.5f) * gridSize, (neighbor.y + 0.5f) * gridSize, 0f);
            if (!IsCellBlocked(newPosition) && !EnemyManager.Instance.IsPositionOccupied(neighbor))
            {
                return neighbor;
            }
        }
        return cell; // Return current cell if no valid neighbor found (shouldn't normally happen)
    }

    protected List<Vector2Int> GetShuffledNeighbors(Vector2Int cell)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>
    {
        cell + Vector2Int.right,
        cell + Vector2Int.left,
        cell + Vector2Int.up,
        cell + Vector2Int.down
    };

        for (int i = 0; i < neighbors.Count; i++)
        {
            int randomIndex = Random.Range(i, neighbors.Count);
            Vector2Int temp = neighbors[randomIndex];
            neighbors[randomIndex] = neighbors[i];
            neighbors[i] = temp;
        }

        return neighbors;
    }
    protected List<Vector2Int> FindPathToPlayer()
    {
        if (!player) return null;
        return AStarPathfinding(currentCell, player.currentCell);
    }

    protected void MoveEnemy(Vector2Int direction)
    {
        Vector2Int newCell = currentCell + direction;
        Vector3 newPosition = new Vector3((newCell.x + 0.5f) * gridSize, (newCell.y + 0.5f) * gridSize, 0f);

        if (!EnemyManager.Instance.IsPositionOccupied(newCell))
        {
            StartCoroutine(JumpToPosition(newPosition));
            EnemyManager.Instance.VacatePosition(currentCell);
            currentCell = newCell;
            EnemyManager.Instance.OccupyPosition(currentCell);
        }
    }

    protected virtual bool IsCellBlocked(Vector3 newPosition)
    {
        Vector2 origin = new Vector2(transform.position.x, transform.position.y);
        Vector2 direction = newPosition - (Vector3)origin;
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, direction.magnitude, collisionMask);
        return hit.collider != null;
    }

    protected IEnumerator JumpToPosition(Vector3 endPosition)
    {
        boxCollider.enabled = false;
        Vector3 startPos = transform.position;
        float duration = 0.075f;
        float elapsedTime = 0f;
        Vector3 midPoint = (startPos + endPosition) / 2f + Vector3.up * 0.5f; // Adjust the height of the jump here

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            Vector3 currentPosition = Vector3.Lerp(Vector3.Lerp(startPos, midPoint, t), Vector3.Lerp(midPoint, endPosition, t), t);
            transform.position = currentPosition;
            yield return null;
        }

        boxCollider.enabled = true;
        transform.position = endPosition;
    }

    #region PathFinding
    protected List<Vector2Int> AStarPathfinding(Vector2Int start, Vector2Int goal)
    {
        List<Vector2Int> openSet = new List<Vector2Int> { start };
        HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        Dictionary<Vector2Int, float> gScore = new Dictionary<Vector2Int, float> { [start] = 0 };
        Dictionary<Vector2Int, float> fScore = new Dictionary<Vector2Int, float> { [start] = HeuristicCostEstimate(start, goal) };

        while (openSet.Count > 0)
        {
            Vector2Int current = GetLowestFScoreNode(openSet, fScore);
            if (current == goal) return ReconstructPath(cameFrom, current);
            openSet.Remove(current);
            closedSet.Add(current);

            foreach (Vector2Int neighbor in GetNeighbors(current))
            {
                if (closedSet.Contains(neighbor)) continue;
                float tentativeGScore = gScore[current] + HeuristicCostEstimate(current, neighbor);
                if (!openSet.Contains(neighbor))
                {
                    openSet.Add(neighbor);
                }
                else if (tentativeGScore >= gScore[neighbor])
                {
                    continue;
                }
                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeGScore;
                fScore[neighbor] = gScore[neighbor] + HeuristicCostEstimate(neighbor, goal);
            }
        }
        return null; // No path found
    }

    protected List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
    {
        List<Vector2Int> totalPath = new List<Vector2Int> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Insert(0, current);
        }
        return totalPath;
    }

    protected Vector2Int GetLowestFScoreNode(List<Vector2Int> openSet, Dictionary<Vector2Int, float> fScore)
    {
        Vector2Int lowest = openSet[0];
        foreach (Vector2Int node in openSet)
        {
            if (fScore.ContainsKey(node) && fScore[node] < fScore[lowest])
            {
                lowest = node;
            }
        }
        return lowest;
    }

    protected float HeuristicCostEstimate(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y); // Using Manhattan distance
    }

    protected List<Vector2Int> GetNeighbors(Vector2Int cell)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>
        {
            cell + Vector2Int.right,
            cell + Vector2Int.left,
            cell + Vector2Int.up,
            cell + Vector2Int.down
        };
        return neighbors;
    }

 
    private Vector3 debugCellWorldPosition;
    #endregion
    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(debugCellWorldPosition, new Vector3(gridSize, gridSize, 0));
        }
    }
}
