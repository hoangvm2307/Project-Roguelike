using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int width { get; set; }
    public int height { get; set; }
    public int x { get; set; }
    public int y { get; set; }
    private bool updatedDoors = false;
    public string sceneName;
    public List<Door> doors = new List<Door>();
    public List<GameObject> enemies = new List<GameObject>();
    public Room(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    void Start()
    {
        width = 18;
        height = 10;
        if (RoomController.instance == null)
        {
            Debug.Log("You presed play in the wrong scene!");
            return;
        }

        // Get all doors in children
        Door[] ds = GetComponentsInChildren<Door>();
        foreach (Door d in ds)
        {
            doors.Add(d);
        }

        // Initially spawn at position when enabled
        RoomController.instance.RegisterRoom(this);
    }

    private void Update()
    {
        // When boss room is spawned and doors are not updated yet, update doors and set updated true
        if (name.Contains("End") && !updatedDoors)
        {
            RemoveUnconnectedDoors();
            updatedDoors = true;
        }
    }

    // Remove doors if they are not connected to any room
    public void RemoveUnconnectedDoors()
    {
        foreach (Door door in doors)
        {
            switch (door.doorType)
            {
                case Door.DoorType.left:
                    if (!RoomController.instance.DoesRoomExist(x - 1, y))
                    {
                        door.wall.SetActive(true);
                        door.gameObject.SetActive(false);
                    }
                    break;
                case Door.DoorType.right:
                    if (!RoomController.instance.DoesRoomExist(x + 1, y))
                    {
                        door.wall.SetActive(true);
                        door.gameObject.SetActive(false);
                    }
                    break;
                case Door.DoorType.top:
                    if (!RoomController.instance.DoesRoomExist(x, y + 1))
                    {
                        door.wall.SetActive(true);
                        door.gameObject.SetActive(false);
                    }
                    break;
                case Door.DoorType.bottom:
                    if (!RoomController.instance.DoesRoomExist(x, y - 1))
                    {
                        door.wall.SetActive(true);
                        door.gameObject.SetActive(false);
                    }
                    break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(transform.position.x + 1.5f, transform.position.y + 0.5f, 0), new Vector3(width, height, 0));
    }
    public Vector3 GetRoomCenter()
    {
        return new Vector3(x * width + 1.5f, y * height + 0.5f);
    }

}
