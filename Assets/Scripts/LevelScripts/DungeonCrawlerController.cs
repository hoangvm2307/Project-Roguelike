using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    up = 0,
    left = 1,
    down = 2,
    right = 3,
};
public class DungeonCrawlerController : MonoBehaviour
{
    // Define 4 directions
    private static Dictionary<Direction, Vector2Int> directionMovementMap = new Dictionary<Direction, Vector2Int>
    {
        {Direction.up, Vector2Int.up },
        {Direction.left, Vector2Int.left},
        {Direction.down, Vector2Int.down},
        {Direction.right, Vector2Int.right },
    };
 
    // Generate dungeon by return list of positions
    public static List<Vector2Int> GenerateDungeon(DungeonGenerationData dungeonData)
    {
        List<DungeonCrawler> dungeonCrawlers = new List<DungeonCrawler>();
        List<Vector2Int> posVisited = new List<Vector2Int>();

        // Adding all dungeon crawlers from scriptable object
        for (int i = 0; i < dungeonData.numberOfCrawlers; i++)
        {
            DungeonCrawler dungeonCrawler = new DungeonCrawler(Vector2Int.zero);
            dungeonCrawlers.Add(dungeonCrawler);
        }

        int iterations = Random.Range(dungeonData.iterationMin, dungeonData.iterationMax);

        for (int i = 0; i < iterations; i++)
        {
            foreach (DungeonCrawler dungeonCrawler in dungeonCrawlers)
            {
                // Get position by moving crawler
                Vector2Int newPos = dungeonCrawler.Move(directionMovementMap);

                // If position is not visited, add to list
                if (!posVisited.Contains(newPos)) posVisited.Add(newPos);
            }
        }
        return posVisited;
    }

}
