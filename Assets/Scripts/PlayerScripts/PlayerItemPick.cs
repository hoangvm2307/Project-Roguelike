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
        if (target.CompareTag("Coin"))
        {
            InventoryManager.Instance.AddCoin();
            AudioManager.Instance.PlayItemPickupSound();
            Destroy(target.gameObject);
        }
    }

    private void PickUpItem(ItemComponent itemComponent)
    {
        AudioManager.Instance.PlayItemPickupSound();
        InventoryManager.Instance.AddItem(itemComponent.itemData);
        UIManager.Instance.ShowItemPickupMessage(itemComponent.itemData);
        Destroy(itemComponent.gameObject);
    }
}
