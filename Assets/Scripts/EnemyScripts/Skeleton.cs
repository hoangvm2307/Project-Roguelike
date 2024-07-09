using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : EnemyBase
{
    protected override IEnumerator MoveTowardsPlayer()
    {
        while (true)
        {
            List<Vector2Int> path = FindPathToPlayer();

            if (path != null && path.Count > 1)
            {
                Vector2Int nextCell = path[1]; // Next cell on the path to the player
                Vector2Int newCell = currentCell + (nextCell - currentCell);
                Vector3 newPosition = new Vector3((newCell.x + 0.5f) * gridSize, (newCell.y + 0.5f) * gridSize, 0f);

                if (!IsCellBlocked(newPosition) && nextCell != player.currentCell)
                {
                    MoveEnemy(nextCell - currentCell);
                }
                else if (nextCell != player.currentCell)
                {
                    // Player is unreachable, find a random walkable neighbor cell
                    Vector2Int randomNeighbor = GetRandomWalkableNeighbor(currentCell);
                    MoveEnemy(randomNeighbor - currentCell);
                }
                else
                {
                    AttackPlayerAtPosition(player.currentCell);
                }
            }


            yield return new WaitForSeconds(moveDelay);
        }
    }

}
