using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float attackRange = 1.5f;
    public int damage = 10;
    public Vector2Int currentCell;
    public LayerMask collisionMask;

    private PlayerController player;
    private List<Vector2Int> path;
    private int pathIndex;

    private void Start()
    {
        player = PlayerController.Instance;
        currentCell = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        StartCoroutine(FindPlayer());
    }

    private IEnumerator FindPlayer()
    {
        while (true)
        {
            Vector2Int playerCell = player.currentCell;
            path = Pathfinding.Instance.FindPath(currentCell, playerCell);
            pathIndex = 0;
            yield return new WaitForSeconds(1f); // Update pathfinding every 1 second
        }
    }

    private void Update()
    {
        if (path != null && pathIndex < path.Count)
        {
            MoveAlongPath();
        }
    }

    private void MoveAlongPath()
    {
        Vector2Int targetCell = path[pathIndex];
        Vector3 targetPosition = new Vector3(targetCell.x, targetCell.y, 0);

        if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
        else
        {
            currentCell = targetCell;
            pathIndex++;
            if (pathIndex >= path.Count)
            {
                path = null;
                if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
                {
                    AttackPlayer();
                }
            }
        }
    }

    private void AttackPlayer()
    {
        HealthUIManager.Instance.TakeDamage(damage);
    }
}
