using UnityEngine;

public class Step : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagName.Player))
        {
            DungeonManager.Instance.GoToNextFloor();
            Debug.Log("Change Floor");
        }
    }
}
