using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIManager : MonoBehaviour
{
    public static HealthUIManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    public PlayerHealth playerHealth;
    public Image heartPrefab;
    public Transform heartsContainer;
    public float heartSpacing = 30f;

    private List<Image> hearts = new List<Image>();

    void Start()
    {
        playerHealth.ResetHealth();
        InitializeHearts();
        UpdateHearts();
    }

    void InitializeHearts()
    {

        for (int i = 0; i < playerHealth.maxHealth; i++)
        {
            CreateHeart(i);
        }
    }

    public void UpdateHearts()
    {

        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < playerHealth.currentHealth)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    public void TakeDamage(int damage)
    {

        playerHealth.TakeDamage(damage);
        UpdateHearts();
    }

    public void Heal(int amount)
    {

        playerHealth.Heal(amount);

        while (hearts.Count < playerHealth.currentHealth)
        {
            CreateHeart(hearts.Count);
        }

        UpdateHearts();
    }

    private void CreateHeart(int index)
    {

        Image heart = Instantiate(heartPrefab, heartsContainer);
        heart.transform.localPosition = new Vector3(index * heartSpacing, 0, 0);
        hearts.Add(heart);
    }
}
