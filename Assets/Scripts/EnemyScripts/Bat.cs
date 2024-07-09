using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : EnemyBase
{
    protected override IEnumerator MoveTowardsPlayer()
    {
        while (player.transform)
        {
            yield return new WaitForSeconds(moveDelay);

            if (player.transform)
            {
                List<Vector2Int> path = FindPathToPlayer();
                if (path != null && path.Count > 1)
                {
                    Vector2Int nextCell = path[1];
                    if (nextCell != player.currentCell)
                    {
                        MoveEnemy(nextCell - currentCell);
                    }
                    else
                    {
                        AttackPlayerAtPosition(player.currentCell);
                    }
                }
            }
        }
    }

    protected override bool IsCellBlocked(Vector3 newPosition)
    {
        return false;
    }

}
