using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance;

    public DungeonGenerationData dungeonGenerationData;
    private List<Vector2Int> dungeonRooms;
    public int currentFloor = 1;
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
        if (currentFloor == 1) return;
        PlayerController player = PlayerController.Instance;
        player.currentCell = new Vector2Int(0, 0);
        StartCoroutine(player.JumpToPosition(new Vector3(player.currentCell.x, player.currentCell.y)));
    }

    private void SpawnRooms(IEnumerable<Vector2Int> rooms)
    {
        int variant = Random.Range(1, 2);
        string startRoom = RoomName.GetRoomName(RoomName.Start, 1, 1);
        RoomController.instance.LoadRoom(startRoom, 0, 0);

        string[] roomTypes = { RoomName.Enemy, RoomName.Treasure };

        foreach (Vector2Int roomLocation in rooms)
        {
            string roomType = roomTypes[Random.Range(0, roomTypes.Length)];
            variant = Random.Range(1, 3);
            if (roomType == RoomName.Empty) variant = 1;
            string roomName = RoomName.GetRoomName(roomType, 1, variant);
            RoomController.instance.LoadRoom(roomName, roomLocation.x, roomLocation.y);
        }
    }
}
