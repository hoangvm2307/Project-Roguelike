using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance;

    public DungeonGenerationData dungeonGenerationData;
    private List<Vector2Int> dungeonRooms;
    private int currentFloor = 1;
    private bool isTransitioning = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GenerateNewFloor();
    }

    public void GoToNextFloor()
    {
        if (!isTransitioning)
        {
            StartCoroutine(TransitionToNextFloor());
        }
    }

    private IEnumerator TransitionToNextFloor()
    {
        isTransitioning = true;

        // Clear current rooms
        RoomController.instance.ClearRooms();

        // Optional: Add a delay or transition effect here if needed
        yield return new WaitForSeconds(1f);

        currentFloor++;
        GenerateNewFloor();

        isTransitioning = false;
    }

    private void GenerateNewFloor()
    {
        dungeonRooms = DungeonCrawlerController.GenerateDungeon(dungeonGenerationData);
        SpawnRooms(dungeonRooms);
    }

    private void SpawnRooms(IEnumerable<Vector2Int> rooms)
    {
        RoomController.instance.LoadRoom(RoomName.Start + "_" + currentFloor, 0, 0);
        foreach (Vector2Int roomLocation in rooms)
        {
            RoomController.instance.LoadRoom(RoomName.Empty + "_" + currentFloor, roomLocation.x, roomLocation.y);
        }
    }
}
