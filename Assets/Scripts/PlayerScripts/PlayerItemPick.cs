using UnityEngine;

public class PlayerItemPick : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.CompareTag("Item"))
        {
            ItemComponent itemComponent = target.GetComponent<ItemComponent>();
            if (itemComponent != null)
            {
                PickUpItem(itemComponent);
            }
        }
    }

    private void PickUpItem(ItemComponent itemComponent)
    {
        InventoryManager.Instance.AddItem(itemComponent.itemData);

        Destroy(itemComponent.gameObject);
    }
}
