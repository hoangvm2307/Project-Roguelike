using UnityEngine;

[CreateAssetMenu(fileName = "NewSpecificItemData", menuName = "Inventory/SpecificItemData")]
public class SpecificItemData : ItemData
{
    public GameObject specialSkillEffect; 

    public override void UseSpecialSkill(Vector3 position)
    {
        if (specialSkillEffect != null)
        {
            Instantiate(specialSkillEffect, position, Quaternion.identity);
            Debug.Log("Using special skill of Example item: " + itemName);
        }
    }
}
