using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerController playerController;
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        InventoryInput();
        AttackInput();
        MovementInput();
    }
    private void MovementInput()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (RhythmManager.Instance.IsPlayerHit())
            {
                playerController.MovePlayer(Vector2Int.right);
            }
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (RhythmManager.Instance.IsPlayerHit())
            {
                playerController.MovePlayer(Vector2Int.left);
            }
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            if (RhythmManager.Instance.IsPlayerHit())
            {
                playerController.MovePlayer(Vector2Int.up);
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (RhythmManager.Instance.IsPlayerHit())
            {
                playerController.MovePlayer(Vector2Int.down);
            }
        }
    }
    private void AttackInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            playerController.AttackEnemy(Vector2Int.right);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            playerController.AttackEnemy(Vector2Int.left);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            playerController.AttackEnemy(Vector2Int.down);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            playerController.AttackEnemy(Vector2Int.up);
        }
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
            else if (InventoryManager.Instance.isInventoryOpen)
            {
                InventoryManager.Instance.CloseInventory();

            }
            InventoryManager.Instance.isInventoryOpen = !InventoryManager.Instance.isInventoryOpen;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (InventoryManager.Instance.isInventoryOpen)
            {
                InventoryManager.Instance.currentType = ItemType.Weapon;
                InventoryManager.Instance.OpenWeapons();

                InventoryManager.Instance.CloseInventory();
                InventoryManager.Instance.isInventoryOpen = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (InventoryManager.Instance.isInventoryOpen)
            {
                InventoryManager.Instance.currentType = ItemType.Item;
                InventoryManager.Instance.OpenItems();
                InventoryManager.Instance.CloseInventory();
                InventoryManager.Instance.isInventoryOpen = false;

            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (InventoryManager.Instance.isInventoryOpen)
            {
                InventoryManager.Instance.currentType = ItemType.Spell;
                InventoryManager.Instance.OpenSpells();
                InventoryManager.Instance.CloseInventory();
                InventoryManager.Instance.isInventoryOpen = false;

            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
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
