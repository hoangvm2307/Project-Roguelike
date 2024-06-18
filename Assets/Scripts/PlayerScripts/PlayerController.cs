using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    private void Awake()
    {
        Instance = this;
    }
    public float gridSize = 1f;
    public Vector2Int currentCell;
    public LayerMask collisionMask;
    public BoxCollider2D boxCollider;
    private PlayerInteraction playerInteraction;
    public delegate void OnPlayerMove();
    public static event OnPlayerMove onPlayerMove;
    public GameObject slash;

    private void OnEnable()
    {
         
    }
    private void OnDisable()
    {
       
    }
 
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        currentCell = new Vector2Int(0, 0);
        playerInteraction = GetComponent<PlayerInteraction>();
    }

    void Update()
    {
        ControlPlayerMove();
    }
    public void ControlPlayerMove()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (RhythmManager.Instance.IsPlayerHit())
            {
                MovePlayer(Vector2Int.right);
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (RhythmManager.Instance.IsPlayerHit())
            {
                MovePlayer(Vector2Int.left);
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if (RhythmManager.Instance.IsPlayerHit())
            {
                MovePlayer(Vector2Int.up);
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if (RhythmManager.Instance.IsPlayerHit())
            {
                MovePlayer(Vector2Int.down);
            }
        }

    }
    public void MovePlayer(Vector2Int direction)
    {
        onPlayerMove?.Invoke();
        Vector2Int newCell = currentCell + direction;
        Vector3 newPosition = new Vector3((newCell.x + 0.5f) * gridSize, (newCell.y + 0.5f) * gridSize, 0f);

        if (playerInteraction.IsEnemyAtPosition(newPosition))
        {
            playerInteraction.AttackEnemyAtPosition(newPosition, GetWeaponDamage());
            GameObject _slash = GameObject.Instantiate(slash, newPosition, Quaternion.identity);
            AdjustSlashDirection(_slash, direction);
        }
        else if (!IsCellBlocked(newPosition, direction))
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
        transform.position = newPosition;
        currentCell = newCell;
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
        float duration = 0.075f;

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
    public void AdjustSlashDirection(GameObject _slash, Vector2Int direction)
    {
        if (direction == Vector2Int.right)
            _slash.GetComponent<SpriteRenderer>().flipX = true;
        if (direction == Vector2Int.up)
            _slash.transform.rotation = Quaternion.Euler(0, 0, -90f);
        if (direction == Vector2Int.left)
            _slash.GetComponent<SpriteRenderer>().flipX = false;
        if (direction == Vector2Int.down)
            _slash.transform.rotation = Quaternion.Euler(0, 0, 90f);
    }

    private int GetWeaponDamage()
    {
        ItemData currentWeaponData = InventoryManager.Instance.currentWeapon;
        if (currentWeaponData != null)
        {
            return currentWeaponData.damage;
        }
        return 0;  
    }
}
