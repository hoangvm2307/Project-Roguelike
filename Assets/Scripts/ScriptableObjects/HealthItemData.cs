using UnityEngine;

[CreateAssetMenu(fileName = "NewHealthItem", menuName = "Inventory/HealthItem")]
public class HealthItemData : ItemData
{
    public int healthIncreaseAmount;

    public override void Use()
    {
        base.Use();
        HealthUIManager.Instance.Heal(healthIncreaseAmount);
        Debug.Log("Used health item: " + itemName + ", increased health by: " + healthIncreaseAmount);
    }
}
