using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float gridSize = 1f;
    private Vector2Int currentCell;
    public LayerMask collisionMask;
    public BoxCollider2D boxCollider;
    public Transform playerTransform;
    public float moveDelay = 1f; // Time between each move

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        currentCell = new Vector2Int(Mathf.RoundToInt(transform.position.x / gridSize), Mathf.RoundToInt(transform.position.y / gridSize));
        StartCoroutine(MoveTowardsPlayer());
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void OnEnable()
    {
        PlayerController.onPlayerMove += UpdatePlayerPosition;
    }
    private void OnDisable()
    {
        PlayerController.onPlayerMove -= UpdatePlayerPosition;
    }
    private IEnumerator MoveTowardsPlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(moveDelay);
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            if (playerTransform)
            {
                List<Vector2Int> path = FindPathToPlayer();
                if (path != null && path.Count > 1)
                {
                    Vector2Int nextCell = path[1];
                    // Check if the next cell is the player's current cell
                    Vector2Int playerCell = new Vector2Int(Mathf.RoundToInt(playerTransform.position.x / gridSize), Mathf.RoundToInt(playerTransform.position.y / gridSize));
                    if (nextCell != playerTransform.GetComponent<PlayerController>().currentCell)
                    {
                        MoveEnemy(nextCell - currentCell);
                    }
                }
            }

        }


    }

    private List<Vector2Int> FindPathToPlayer()
    {
        //Vector2Int playerCell = new Vector2Int(Mathf.RoundToInt(playerTransform.position.x / gridSize), Mathf.RoundToInt(playerTransform.position.y / gridSize));
        if (!playerTransform) return null;
        return AStarPathfinding(currentCell, playerTransform.GetComponent<PlayerController>().currentCell);
    }

    private void MoveEnemy(Vector2Int direction)
    {
        Vector2Int newCell = currentCell + direction;
        Vector3 newPosition = new Vector3((newCell.x + 0.5f) * gridSize, (newCell.y + 0.5f) * gridSize, 0f);

        if (!IsCellBlocked(newPosition))
        {
            StartCoroutine(JumpToPosition(newPosition));
            currentCell = newCell;
        }
    }

    private bool IsCellBlocked(Vector3 newPosition)
    {
        Vector2 origin = new Vector2(transform.position.x, transform.position.y);
        Vector2 direction = new Vector2(newPosition.x - origin.x, newPosition.y - origin.y);

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, direction.magnitude, collisionMask);
        return hit.collider != null;
    }

    private IEnumerator JumpToPosition(Vector3 endPosition)
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

            // Quadratic Bezier curve for smooth jump
            Vector3 currentPosition = Vector3.Lerp(Vector3.Lerp(startPos, midPoint, t), Vector3.Lerp(midPoint, endPosition, t), t);
            transform.position = currentPosition;

            yield return null;
        }
        boxCollider.enabled = true;
        transform.position = endPosition;
    }

    private List<Vector2Int> AStarPathfinding(Vector2Int start, Vector2Int goal)
    {
        List<Vector2Int> openSet = new List<Vector2Int> { start };
        HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        Dictionary<Vector2Int, float> gScore = new Dictionary<Vector2Int, float> { [start] = 0 };
        Dictionary<Vector2Int, float> fScore = new Dictionary<Vector2Int, float> { [start] = HeuristicCostEstimate(start, goal) };

        while (openSet.Count > 0)
        {
            Vector2Int current = GetLowestFScoreNode(openSet, fScore);

            if (current == goal)
            {
                return ReconstructPath(cameFrom, current);
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (Vector2Int neighbor in GetNeighbors(current))
            {
                if (closedSet.Contains(neighbor))
                {
                    continue;
                }

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

    private List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
    {
        List<Vector2Int> totalPath = new List<Vector2Int> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Insert(0, current);
        }
        return totalPath;
    }

    private Vector2Int GetLowestFScoreNode(List<Vector2Int> openSet, Dictionary<Vector2Int, float> fScore)
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

    private float HeuristicCostEstimate(Vector2Int a, Vector2Int b)
    {
        // Using Manhattan distance
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    private List<Vector2Int> GetNeighbors(Vector2Int cell)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>
        {
            cell + Vector2Int.right,
            cell + Vector2Int.left,
            cell + Vector2Int.up,
            cell + Vector2Int.down
        };

        // Remove blocked neighbors
        neighbors.RemoveAll(neighbor => IsCellBlocked(new Vector3((neighbor.x + 0.5f) * gridSize, (neighbor.y + 0.5f) * gridSize, 0f)));

        return neighbors;
    }
    private Vector2Int SnapToGrid(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x / gridSize);
        int y = Mathf.RoundToInt(position.y / gridSize);
        return new Vector2Int(x, y);
    }
    public void UpdatePlayerPosition()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        //StartCoroutine(MoveTowardsPlayer());
    }
}
