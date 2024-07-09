using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager _instance;
    public static EnemyManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<EnemyManager>();
                if (_instance == null)
                {
                    GameObject singleton = new GameObject(typeof(EnemyManager).ToString());
                    _instance = singleton.AddComponent<EnemyManager>();
                }
            }
            return _instance;
        }
    }

    private HashSet<Vector2Int> occupiedPositions = new HashSet<Vector2Int>();

    public bool IsPositionOccupied(Vector2Int position)
    {
        return occupiedPositions.Contains(position);
    }

    public void OccupyPosition(Vector2Int position)
    {
        occupiedPositions.Add(position);
    }

    public void VacatePosition(Vector2Int position)
    {
        occupiedPositions.Remove(position);
    }
}
