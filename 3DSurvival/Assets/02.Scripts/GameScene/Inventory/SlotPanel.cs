using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SlotPanel : MonoBehaviour
{
    public ItemSlot[] recipeSlots;     // 조합 전용 슬롯
    public ItemSlot[] inventorySlots;  // 인벤토리 슬롯
    public ItemSlot[] equipmentSlots;
    public Transform inventoryPanel;
    public Transform recipePanel;
    public Transform equipmentPanel;
    public int recipeSlotCount; //초기값 레시피슬롯
    public int inventorySlotCount; //초기값 인벤토리슬롯
    public int equipmentSlotCount;
    void Start()
    {
        InventoryManager.Instance.Inventory.slotPanel = this;
        CharacterManager.Instance.Player.addItem += AddItem;
        
        recipeSlotCount = recipePanel.childCount;
        inventorySlotCount = inventoryPanel.childCount;
        equipmentSlotCount = equipmentPanel.childCount;
        
        recipeSlots = new ItemSlot[recipeSlotCount];
        inventorySlots = new ItemSlot[inventorySlotCount];
        equipmentSlots = new ItemSlot[equipmentSlotCount];
        
        for (int i = 0; i < recipeSlotCount; i++)
        {
            recipeSlots[i] = recipePanel.GetChild(i).GetComponent<ItemSlot>();
            recipeSlots[i].idx = i;
            recipeSlots[i].inventory = InventoryManager.Instance.Inventory;
            recipeSlots[i].slotType = SLOTTYPE.RECIPE;
            recipeSlots[i].inventory.slotPanel = this;
            
            
        }
        for (int i = 0; i < inventorySlotCount; i++)
        {
            inventorySlots[i] = inventoryPanel.GetChild(i).GetComponent<ItemSlot>();
            inventorySlots[i].idx = i;
            inventorySlots[i].inventory = InventoryManager.Instance.Inventory;
            inventorySlots[i].slotType = SLOTTYPE.INVENTORY;
            inventorySlots[i].inventory.slotPanel = this;
            
        }
        for (int i = 0; i < equipmentSlotCount; i++)
        {
            equipmentSlots[i] = equipmentPanel.GetChild(i).GetComponent<ItemSlot>();
            equipmentSlots[i].idx = i;
            equipmentSlots[i].inventory = InventoryManager.Instance.Inventory;
            equipmentSlots[i].slotType = SLOTTYPE.EQUIPMENT;
            equipmentSlots[i].inventory.slotPanel = this;
            
        }
    }

    public void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;

        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                slot.quantity++;
                InventoryManager.Instance.Inventory.InventoryUI.UIUpdate();
                return;
            }
        }

        ItemSlot emptySlot = GetEmptySlot();
        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            InventoryManager.Instance.Inventory.InventoryUI.UIUpdate();
            return;
        }

        InventoryManager.Instance.Inventory.ThrowItem(data);
        InventoryManager.Instance.ItemData = null;
    }

    ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].item == data && inventorySlots[i].quantity < data.maxStackAmount)
            {
                return inventorySlots[i];
            }
        }

        return null;
    }

    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].item == null)
            {
                return inventorySlots[i];
            }
        }

        return null;
    }
}