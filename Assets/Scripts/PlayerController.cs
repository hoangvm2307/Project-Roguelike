using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float gridSize = 1f;
    private Vector2Int currentCell;
    public LayerMask collisionMask;
    public BoxCollider2D boxCollider;
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        currentCell = new Vector2Int(0, 0);
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            MovePlayer(Vector2Int.right);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            MovePlayer(Vector2Int.left);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            MovePlayer(Vector2Int.up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            MovePlayer(Vector2Int.down);
        }
    }

    public void MovePlayer(Vector2Int direction)
    {
        Vector2Int newCell = currentCell + direction;
        Vector3 newPosition = new Vector3((newCell.x + 0.5f) * gridSize, (newCell.y + 0.5f) * gridSize, 0f);

        if (!IsCellBlocked(newPosition, direction))
        {
            StartCoroutine(JumpToPosition(newPosition));
            currentCell = newCell;
        }
    }
    public void MovePlayerThroughWall(Vector2Int direction)
    {
        Vector2Int newCell = currentCell + direction;
        Vector3 newPosition = new Vector3((newCell.x + 0.5f) * gridSize, (newCell.y + 0.5f) * gridSize, 0f);

        if (!IsCellBlocked(newPosition, direction))
        {
            transform.position = newPosition;
            currentCell = newCell;
        }
    }
    private bool IsCellBlocked(Vector3 newPosition, Vector2Int direction)
    {
        Vector2 origin = new Vector2(transform.position.x, transform.position.y);
        Vector2 destination = new Vector2(newPosition.x, newPosition.y);
        Vector2 directionVector = destination - origin;

        RaycastHit2D hit = Physics2D.Raycast(origin, directionVector, directionVector.magnitude, collisionMask);
        return hit.collider != null;
    }

    private IEnumerator JumpToPosition(Vector3 endPosition)
    {
        boxCollider.enabled = false;
        Vector3 startPos = transform.position;
        float duration = 0.1f;

        float elapsedTime = 0f;
        Vector3 midPoint = (startPos + endPosition) / 2f + Vector3.up * 0.5f; // Adjust the height of the jump here

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // Quadratic Bezier curve for smooth jump
            Vector3 currentPosition = Vector3.Lerp(Vector3.Lerp(startPos, midPoint, t), Vector3.Lerp(midPoint, endPosition, t), t);
            transform.position = currentPosition;

            yield return null;
        }
        boxCollider.enabled = true;
        transform.position = endPosition;
    }

}
