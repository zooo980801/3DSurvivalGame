using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SlotPanel : MonoBehaviour
{
      public ItemSlot[] itemSlots;
      public Transform slotPanel;
    void Start()
    {
        InventoryManager.Instance.Inventory.slotPanel = this;
        //controller.inventory += Toggle;
        //CharacterManager.Instance.Player.addItem += AddItem; 

        itemSlots = new ItemSlot[slotPanel.childCount];
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            itemSlots[i].idx = i;
            itemSlots[i].inventory = InventoryManager.Instance.Inventory;
            itemSlots[i].inventory.slotPanel = this;
        }
        
    }
    
    public void AddItem()
    {
        ItemData data = InventoryManager.Instance.ItemData;
    
    
        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                slot.quantity++;
                InventoryManager.Instance.InventoryUI.UIUpdate();
                return;
            }
        }
    
        ItemSlot emptySlot = GetEmptySlot(); 
        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
                InventoryManager.Instance.InventoryUI.UIUpdate();
            return;
        }
    
        InventoryManager.Instance.Inventory.ThrowItem(data);
        InventoryManager.Instance.ItemData = null;
    }
    
    ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            
            if (itemSlots[i].item == data && itemSlots[i].quantity < data.maxStackAmount)
            {
                return itemSlots[i]; 
            }
        }
    
        return null;
    }
    
    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].item == null)
            {
                return itemSlots[i];
            }
        }
    
        return null;
    }

    

    
}
