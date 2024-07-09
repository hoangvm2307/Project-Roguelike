using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    Equipment,
    Weapon,
    Item,
    Spell
}
[CreateAssetMenu(fileName = "NewItem", menuName = "ScriptableObjects/Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public int damage;
    public string description;
    public Sprite sprite;
    public int maxStack;
    public ItemType type;
    public GameObject prefab;
    public virtual void Use()
    {
        Debug.Log("Using  item " + itemName);

    }
    public virtual void UseSpecialSkill(Vector3 position)
    {
        Debug.Log("Using special skill of item " + itemName);
    }
}
