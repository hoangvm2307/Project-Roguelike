using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 2;
    private int currentHealth;
    public GameObject heartPrefab;
    private Transform heartsContainer;
    public float heartSpacing = 0.5f;

    private void Start()
    {
        currentHealth = maxHealth;
        heartsContainer = new GameObject("HeartsContainer").transform;
        heartsContainer.SetParent(transform);
        heartsContainer.localPosition = Vector3.up * 2f;
        UpdateHearts();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHearts();
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UpdateHearts();
    }

    private void Die()
    {
        foreach (Transform heart in heartsContainer)
        {
            Destroy(heart.gameObject);
        }
        Destroy(gameObject);
    }

    private void UpdateHearts()
    {
        // Clear existing hearts
        foreach (Transform heart in heartsContainer)
        {
            Destroy(heart.gameObject);
        }

        // Instantiate new hearts and position them
        for (int i = 0; i < currentHealth; i++)
        {
            Instantiate(heartPrefab, heartsContainer).transform.localPosition = new Vector3(i * heartSpacing - (currentHealth - 1) * heartSpacing / 2, 0, 0);
        }
    }

    private void Update()
    {
        // Keep the hearts container above the enemy
        heartsContainer.position = transform.position + Vector3.up * 0.75f;
    }
}
