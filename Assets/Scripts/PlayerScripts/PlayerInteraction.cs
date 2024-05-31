using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private CameraShake cameraShake;
    public CameraController cameraController;
    private void Start()
    {
        cameraShake = Camera.main.GetComponent<CameraShake>();
    }
    public bool IsEnemyAtPosition(Vector3 position)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.1f);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                return true;
            }
        }
        return false;
    }

    public void AttackEnemyAtPosition(Vector3 position)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.1f);

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {

                Destroy(collider.gameObject);
                Debug.Log("Enemy destroyed");

                if (cameraShake != null)
                {
                    StartCoroutine(cameraShake.Shake(.15f, 0.2f));
                }
                break;
            }
        }

    }
}
