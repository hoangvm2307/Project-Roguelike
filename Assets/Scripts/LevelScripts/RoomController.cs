using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
public class RoomInfo
{
    public string name;
    public int x;
    public int y;
}
public class RoomController : MonoBehaviour
{
    public static RoomController instance;

    string currentWorldName = "Basement";

    RoomInfo currentLoadRoomData;

    Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();

    public List<Room> loadedRooms = new List<Room>();
    

    bool isLoadingRoom = false;
    bool spawnedBossRoom = false;
    bool updatedRooms = false;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        UpdateRoomQueue();
    }

    void UpdateRoomQueue()
    {
        if (isLoadingRoom) return;

        // If all normal rooms are spawned, spawn boss room and remove unconnected doors
        if (loadRoomQueue.Count == 0)
        {
            // If boss room not spawned, spawn boss room
            if (!spawnedBossRoom)
            {
                StartCoroutine(SpawnBossRoom());
            }
            // If boss room is spawned, remove unconnected doors
            else if (spawnedBossRoom && !updatedRooms)
            {
                foreach (Room room in loadedRooms)
                {
                    room.RemoveUnconnectedDoors();
                }
                updatedRooms = true;
            }
            return;
        }

        // Get next room info
        currentLoadRoomData = loadRoomQueue.Dequeue();

        if (currentLoadRoomData == null) return;

        isLoadingRoom = true;

        // Load room into unity world
        StartCoroutine(LoadRoomRoutine(currentLoadRoomData));
    }

    // Add room to queue, combine with for loop
    public void LoadRoom(string name, int x, int y)
    {
        if (DoesRoomExist(x, y)) return;

        RoomInfo newRoomData = new RoomInfo();
        newRoomData.name = name;
        newRoomData.x = x;
        newRoomData.y = y;

        loadRoomQueue.Enqueue(newRoomData);
    }

    // Load async room into unity world
    IEnumerator LoadRoomRoutine(RoomInfo info)
    {
        string roomName = currentWorldName + info.name;

        AsyncOperation loadRoom = SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);

        while (loadRoom.isDone == false)
        {
            yield return null;
        }
    }

    // Load boss room into unity world
    IEnumerator SpawnBossRoom()
    {
        spawnedBossRoom = true;
        yield return new WaitForSeconds(0.5f);
        if (loadRoomQueue.Count == 0)
        {
            Room tempRoom = loadedRooms[loadedRooms.Count - 1];
            var roomToRemove = loadedRooms.Single(r => r.x == tempRoom.x && r.y == tempRoom.y);
            loadedRooms.Remove(roomToRemove);
            LoadRoom("End", tempRoom.x, tempRoom.y);
        }
    }

    // Set room position and add to loaded rooms list
    public void RegisterRoom(Room room)
    {
        if (!DoesRoomExist(currentLoadRoomData.x, currentLoadRoomData.y))
        {
            room.transform.position = new Vector3(
                currentLoadRoomData.x * room.width,
                currentLoadRoomData.y * room.height,
                0);

            room.x = currentLoadRoomData.x;
            room.y = currentLoadRoomData.y;
            room.name = currentWorldName + "-" + currentLoadRoomData.name + " " + room.x + ", " + room.y;
            room.transform.parent = transform;

            isLoadingRoom = false;

            if (loadedRooms.Count == 0)
            {
                CameraController.instance.currentRoom = room;
            }
            loadedRooms.Add(room);

        }
        else
        {
            Destroy(room.gameObject);
            isLoadingRoom = false;
        }

    }
    public bool DoesRoomExist(int x, int y)
    {
        return loadedRooms.Find(item => item.x == x && item.y == y) != null;
    }

    public Room GetRoom(int x, int y)
    {
        return loadedRooms.Find(item => item.x == x && item.y == y);
    }

    // Update current room when player enter room
    public void OnPlayerEnterRoom(Room room)
    {
        CameraController.instance.currentRoom = room;
        MinimapController.instance.currentRoom = room;
        room.enemies.ForEach(e => e.gameObject.SetActive(true));
        
    }
}
