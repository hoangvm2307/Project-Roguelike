using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding Instance;
    private void Awake()
    {
        Instance = this;
    }

    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> openList = new List<Vector2Int>();
        HashSet<Vector2Int> closedList = new HashSet<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        Dictionary<Vector2Int, float> gCost = new Dictionary<Vector2Int, float>();
        Dictionary<Vector2Int, float> fCost = new Dictionary<Vector2Int, float>();

        openList.Add(start);
        gCost[start] = 0;
        fCost[start] = Heuristic(start, end);

        while (openList.Count > 0)
        {
            Vector2Int current = GetLowestFCostNode(openList, fCost);
            if (current == end)
            {
                return RetracePath(cameFrom, current);
            }

            openList.Remove(current);
            closedList.Add(current);

            foreach (Vector2Int neighbor in GetNeighbors(current))
            {
                if (closedList.Contains(neighbor))
                {
                    continue;
                }

                float tentativeGCost = gCost[current] + Heuristic(current, neighbor);
                if (!openList.Contains(neighbor) || tentativeGCost < gCost[neighbor])
                {
                    gCost[neighbor] = tentativeGCost;
                    fCost[neighbor] = gCost[neighbor] + Heuristic(neighbor, end);
                    cameFrom[neighbor] = current;

                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }
            }
        }

        return null; // No path found
    }

    private List<Vector2Int> RetracePath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        while (cameFrom.ContainsKey(current))
        {
            path.Add(current);
            current = cameFrom[current];
        }
        path.Reverse();
        return path;
    }

    private Vector2Int GetLowestFCostNode(List<Vector2Int> openList, Dictionary<Vector2Int, float> fCost)
    {
        Vector2Int lowest = openList[0];
        foreach (Vector2Int node in openList)
        {
            if (fCost[node] < fCost[lowest])
            {
                lowest = node;
            }
        }
        return lowest;
    }

    private float Heuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y); // Manhattan distance
    }

    private List<Vector2Int> GetNeighbors(Vector2Int node)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        neighbors.Add(new Vector2Int(node.x + 1, node.y));
        neighbors.Add(new Vector2Int(node.x - 1, node.y));
        neighbors.Add(new Vector2Int(node.x, node.y + 1));
        neighbors.Add(new Vector2Int(node.x, node.y - 1));
        return neighbors;
    }
}
