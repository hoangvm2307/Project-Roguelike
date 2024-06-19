using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public Room currentRoom;
    public float moveSpeedWhenRoomChange;
    public float offset;
    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        UpdatePosition();
    }

    // Update Position Every Frame
    void UpdatePosition()
    {
        if (currentRoom == null)
        {
            return;
        }

        Vector3 targetPos = GetCameraTargetPosition();
        targetPos = new Vector3(targetPos.x, targetPos.y - offset, -10);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeedWhenRoomChange);
    }

    // Get position camera will move to
    private Vector3 GetCameraTargetPosition()
    {
        if (currentRoom == null) return Vector3.zero;

        // Get center position of room
        Vector3 targetPos = currentRoom.GetRoomCenter();

        return targetPos;
    }

    public bool IsSwitchingScene()
    {
        return transform.position.Equals(GetCameraTargetPosition()) == false;
    }

}
