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
    public void SaveInventory(SaveData data)
    {
        data.inventoryItems.Clear();

        for (int i = 0; i < slotPanel.itemSlots.Length; i++)
        {
            var slot = slotPanel.itemSlots[i];
            if (slot.item != null)
            {
                SavedItem savedItem = new SavedItem
                {
                    itemId = slot.item.id,
                    amount = slot.quantity,
                    equipped = slot.equipped,
                    slotIndex = i
                };
                data.inventoryItems.Add(savedItem);
            }
        }
    }

    public void LoadInventory(SaveData data)
    {
        if (data == null)
        {
            Debug.LogError("로드할 데이터가 null입니다!");
            return;
        }

        if (data.inventoryItems == null)
        {
            Debug.LogWarning("인벤토리 데이터가 null입니다. 새 리스트 생성.");
            data.inventoryItems = new List<SavedItem>();
        }

        Debug.Log($"로드 시작 - 저장된 아이템 수: {data.inventoryItems.Count}");

        // 모든 슬롯 초기화
        foreach (var slot in slotPanel.itemSlots)
        {
            slot.Clear();
            slot.equipped = false;
        }

        // 아이템 로드
        foreach (var savedItem in data.inventoryItems)
        {
            if (savedItem.slotIndex < slotPanel.itemSlots.Length)
            {
                ItemData itemData = ItemDatabase.Instance?.GetItemById(savedItem.itemId);
                if (itemData != null)
                {
                    var slot = slotPanel.itemSlots[savedItem.slotIndex];
                    slot.item = itemData;
                    slot.quantity = savedItem.amount;
                    slot.equipped = savedItem.equipped;
                    slot.Set();
                    Debug.Log($"아이템 로드 성공: {itemData.displayName} (슬롯 {savedItem.slotIndex})");
                }
                else
                {
                    Debug.LogError($"아이템 ID '{savedItem.itemId}'를 찾을 수 없습니다!");
                }
            }
            else
            {
                Debug.LogError($"유효하지 않은 슬롯 인덱스: {savedItem.slotIndex}");
            }
        }

        // UI 갱신
        selectedItem = null;
        selectedIdx = -1;
        InventoryManager.Instance.InventoryUI.ClearSelectedItemWindow();
        InventoryManager.Instance.InventoryUI.UIUpdate();
    }

}
