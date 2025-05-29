using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Inventory : MonoBehaviour
{
    private SlotPanel _slotPanel;
    public SlotPanel slotPanel{get{return _slotPanel;}set{_slotPanel = value;}}
    public ItemData selectedItem;
    private int selectedIdx;
    public int SelectedIdx{get{return selectedIdx;}set{selectedIdx=value;}}
    public Transform dropPosition;
    public int testQuantity=10;
    void Awake()
    {
        InventoryManager.Instance.Inventory = this;

    }

    private void Start()
    {
        if (selectedItem != null)
        {
            AddTestItem(selectedItem, testQuantity);
        }
    }

    public void SelectItem(int idx)
    {
        if (slotPanel.itemSlots[idx].item == null) return;

        selectedItem = slotPanel.itemSlots[idx].item;
        selectedIdx = idx;

        InventoryManager.Instance.InventoryUI.SelectItemUI(idx);//버튼,UI 아이템에따라 활성화
    }
    public void RemoveSelectedItem()
    {
        slotPanel.itemSlots[selectedIdx].quantity--;
        if (slotPanel.itemSlots[selectedIdx].quantity <= 0)
        {
            selectedItem = null;
            slotPanel.itemSlots[selectedIdx].item = null;
            selectedIdx = -1;
            InventoryManager.Instance.InventoryUI.ClearSelectedItemWindow();
        }

        InventoryManager.Instance.InventoryUI.UIUpdate();
    }
    public void ThrowItem(ItemData data)
    {
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }
    
    public void AddTestItem(ItemData itemData, int quantity = 1)
    {
        foreach (var slot in slotPanel.itemSlots)
        {
            if (slot.item == null)
            {
                slot.item = itemData;
                slot.quantity = quantity;
                slot.Set(); // 슬롯 UI 갱신
                return;
            }
        }

        Debug.LogWarning("인벤토리에 빈 슬롯이 없습니다.");
    }

}
