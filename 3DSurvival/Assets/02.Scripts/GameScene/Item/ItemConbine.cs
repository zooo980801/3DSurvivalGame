using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemConbine : MonoBehaviour
{
    public List<Combine> combines;
    public Inventory inventory;
    private void Start()
    {
        inventory = InventoryManager.Instance.Inventory;
    }

    public void OnCombineBtn()
    {
        combines.Clear();
        foreach (var combine in inventory.selectedItem.resultItems.ToList())
        {
            combines.Add(combine);
        }
        ItemData selectedItemA = inventory.selectedItem.resultItems[inventory.SelectedIdx].itemA;
        ItemData selectedItemB = inventory.selectedItem.resultItems[inventory.SelectedIdx].itemB;
        tryCombine(selectedItemA, selectedItemB);
    }

    public bool tryCombine(ItemData itemA, ItemData itemB)
    {
        foreach (var combine in combines)
        {
            if ((combine.itemA == itemA && combine.itemB == itemB) || (combine.itemA == itemB && combine.itemB == itemA))
            {
                if (( HasItem(itemA,1)||(HasItem(itemB,1))))
                {
                    CharacterManager.Instance.Player.itemData = combine.reultItem;
                    InventoryManager.Instance.Inventory.slotPanel.AddItem();
            
                    InventoryManager.Instance.Inventory.RemoveItemByName(itemA.displayName,1);
                    InventoryManager.Instance.Inventory.RemoveItemByName(itemB.displayName,1); 
                    return true;
                }
            }
        }
        return false;
    }
    public bool HasItem(ItemData item, int count)
    {
        int total = 0;
        foreach (var slot in inventory.slotPanel.itemSlots)
        {
            if (slot.item == item)
            {
                total += slot.quantity;
                if (total >= count)
                    return true;
            }
        }
        return false;
    }
    
}
