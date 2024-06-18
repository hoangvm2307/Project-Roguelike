using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        InventoryInput();
    }
    private void MovementInput()
    {

    }
    private void InventoryInput()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!InventoryManager.Instance.isInventoryOpen)
            {
                InventoryManager.Instance.OpenInventory();
                InventoryManager.Instance.CloseItems();
                InventoryManager.Instance.CloseEquipments();
                InventoryManager.Instance.CloseSpells();
                InventoryManager.Instance.CloseWeapons();
                //InventoryManager.Instance.CloseItemSwiper();

            }
            else if(InventoryManager.Instance.isInventoryOpen)
            {
                InventoryManager.Instance.CloseInventory();
            }
            InventoryManager.Instance.isInventoryOpen = !InventoryManager.Instance.isInventoryOpen;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (InventoryManager.Instance.isInventoryOpen)
            {
                InventoryManager.Instance.currentType = ItemType.Weapon;
                InventoryManager.Instance.OpenWeapons();

                InventoryManager.Instance.CloseInventory();
                InventoryManager.Instance.isInventoryOpen = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (InventoryManager.Instance.isInventoryOpen)
            {
                InventoryManager.Instance.currentType = ItemType.Item;
                InventoryManager.Instance.OpenItems();
                InventoryManager.Instance.CloseInventory();
                InventoryManager.Instance.isInventoryOpen = false;

            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (InventoryManager.Instance.isInventoryOpen)
            {
                InventoryManager.Instance.currentType = ItemType.Spell;
                InventoryManager.Instance.OpenSpells();
                InventoryManager.Instance.CloseInventory();
                InventoryManager.Instance.isInventoryOpen = false;

            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (InventoryManager.Instance.isInventoryOpen)
            {
                InventoryManager.Instance.currentType = ItemType.Equipment;
                InventoryManager.Instance.OpenEquipments();
                InventoryManager.Instance.CloseInventory();
                InventoryManager.Instance.isInventoryOpen = false;

            }
        }
    }
}
