using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private CameraShake cameraShake;
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

    public void AttackEnemyAtPosition(Vector3 position, int damage)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.1f);

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {

                GameObject enemy = collider.gameObject;
                enemy.GetComponent<EnemyHealth>().TakeDamage(damage);

                if (cameraShake != null)
                {
                    StartCoroutine(cameraShake.Shake());
                }
                break;
            }
        }

    }
}
