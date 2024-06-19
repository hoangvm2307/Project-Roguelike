using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public ItemType currentType;
    public List<Item> itemList = new List<Item>();
    [Header("UI Ref")]
    public Image spellImage;
    public Image weaponImage;

    [Header("Current Inventory")]
    public ItemData currentItem;
    public ItemData currentWeapon;
    public ItemData currentEquipment;
    public ItemData currentSpell;

    [Header("Panels")]
    public Transform equipments;
    public Transform itemSwiper;
    public Transform spells;
    public Transform items;
    public Transform weapons;
    public GameObject inventoryTab;
    public event Action OnChangedItem;
    public bool isInventoryOpen;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    private void Start()
    {

    }
    public List<Item> GetList()
    {
        switch (currentType)
        {
            case ItemType.Weapon:
                return itemList.Where(item => item.itemData.type == ItemType.Weapon).ToList();
            case ItemType.Equipment:
                return itemList.Where(item => item.itemData.type == ItemType.Equipment).ToList();
            case ItemType.Spell:
                return itemList.Where(item => item.itemData.type == ItemType.Spell).ToList();
            case ItemType.Item:
                return itemList.Where(item => item.itemData.type == ItemType.Spell).ToList();

        }
        return null;
    }
    public void AddItem(ItemData itemData)
    {
        Item existingItem = itemList.Find(i => i.itemData == itemData);
        int quantity = 1;
        if (existingItem != null)
        {
            existingItem.quantity += quantity;
        }
        else
        {
            itemList.Add(new Item(itemData, quantity));
            GameObject item = Instantiate(itemData.prefab);
            switch (itemData.type)
            {
                case ItemType.Equipment:
                    item.transform.SetParent(equipments);
                    break;
                case ItemType.Spell:
                    item.transform.SetParent(spells);
                    break;
                case ItemType.Weapon:
                    item.transform.SetParent(weapons);
                    break;
                case ItemType.Item:
                    item.transform.SetParent(items);
                    break;
            }
        }

        OnChangedItem?.Invoke();

    }

    public void RemoveItem(ItemData itemData, int quantity)
    {
        Item itemToRemove = itemList.Find(i => i.itemData == itemData);

        if (itemToRemove != null)
        {
            itemToRemove.quantity -= quantity;
            if (itemToRemove.quantity <= 0)
            {
                itemList.Remove(itemToRemove);
            }
        }
        OnChangedItem?.Invoke();

    }
    public void UseItem(ItemData itemData)
    {
        Item itemToUse = itemList.Find(i => i.itemData == itemData);

        if (itemToUse != null && itemToUse.quantity > 0)
        {
            itemToUse.Use();
            if (itemToUse.quantity <= 0)
            {
                itemList.Remove(itemToUse);
            }
        }
        OnChangedItem?.Invoke();
    }
    public void SetCurrentItem(ItemData itemData)
    {
        switch (itemData.type)
        {
            case ItemType.Item:
                UseItem(itemData);
                break;
            case ItemType.Equipment:
                currentEquipment = itemData;
                break;
            case ItemType.Weapon:
                currentWeapon = itemData;
                weaponImage.sprite = itemData.sprite;
                break;
            case ItemType.Spell:
                currentSpell = itemData;
                spellImage.sprite = itemData.sprite;
                break;

        }
    }
    #region Inventory, Equipments, Weapons, Spells, Items Open Close
    public void OpenInventory()
    {
        inventoryTab.SetActive(true);
    }
    public void CloseInventory()
    {
        inventoryTab.SetActive(false);
    }
    public void OpenWeapons()
    {
        weapons.gameObject.SetActive(true);
    }
    public void CloseWeapons()
    {
        weapons.gameObject.SetActive(false);
    }
 
    public void OpenSpells()
    {
        spells.gameObject.SetActive(true);
    }
    public void CloseSpells()
    {
        spells.gameObject.SetActive(false);
    }
    public void OpenEquipments()
    {
        equipments.gameObject.SetActive(true);
    }
    public void CloseEquipments()
    {
        equipments.gameObject.SetActive(false);
    }
    public void OpenItems()
    {
        items.gameObject.SetActive(true);
    }
    public void CloseItems()
    {
        items.gameObject.SetActive(false);
    }

    #endregion
}
