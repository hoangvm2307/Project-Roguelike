using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public enum DoorType
    {
        left, right, top, bottom
    }

    public DoorType doorType;
    public GameObject wall;

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Equals("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>(); 

            // Get parent
            Room room = this.GetComponentInParent<Room>();

            Room targetRoom = null;

            // Define target room
            switch (doorType)
            {
                case DoorType.left:
                    player.MovePlayerThroughWall(Vector2Int.left * 5);
                    targetRoom = RoomController.instance.GetRoom(room.x - 1, room.y);
                    break;
                case DoorType.right:
                    player.MovePlayerThroughWall(Vector2Int.right * 5);
                    targetRoom = RoomController.instance.GetRoom(room.x + 1, room.y);
                    break;
                case DoorType.top:
                    player.MovePlayerThroughWall(Vector2Int.up * 3);
                    targetRoom = RoomController.instance.GetRoom(room.x, room.y + 1);
                    break;
                case DoorType.bottom:
                    player.MovePlayerThroughWall(Vector2Int.down * 3);
                    targetRoom = RoomController.instance.GetRoom(room.x, room.y - 1);
                    break;
            }

            // Set target room
            RoomController.instance.OnPlayerEnterRoom(targetRoom);

            // Move player
         
        }
    }
}
