using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Inventory : MonoBehaviour
{
    private SlotPanel _slotPanel;
    public SlotPanel slotPanel{get{return _slotPanel;}set{_slotPanel = value;}}
    
    private SLOTTYPE _selectedSlotType;
    public SLOTTYPE SelectedSlotType{get{return _selectedSlotType;}set{_selectedSlotType = value;}}
    
    private InventoryUI _inventoryUI;
    public InventoryUI InventoryUI{get{return _inventoryUI;}set{_inventoryUI=value;}}
    
    private int selectedIdx;
    public int SelectedIdx{get{return selectedIdx;}set{selectedIdx=value;}}
    
    public ItemData selectedItem;
    public Transform dropPosition;

    void Awake()
    {
        InventoryManager.Instance.Inventory = this;
        
    }

    // private void Start()
    // {
    //     if (selectedItem != null)
    //     {
    //         AddTestItem(selectedItem, testQuantity);
    //     }
    // }

    public void SelectItem(int idx,SLOTTYPE slotType)
    {
        switch (slotType)
        {
            case SLOTTYPE.INVENTORY:
                SelectItemFromInventory(idx);
                InventoryUI.SelectItemBtnUI(idx);
                break;
            case SLOTTYPE.RECIPE:
                SelectItemFromRecipe(idx);
                break;
            case SLOTTYPE.EQUIPMENT:
                SelectItemFromEquipment(idx);
                break;
        }
        _selectedSlotType = slotType;
    }

    public void SelectItemFromInventory(int idx)
    {
        ItemSlot selectSlot = slotPanel.inventorySlots[idx];
        if (selectSlot.item == null)return;
        
        selectedItem = selectSlot.item;
        selectedIdx = idx;
 
    }
    public void SelectItemFromRecipe(int idx)
    {
        ItemSlot selectSlot = slotPanel.recipeSlots[idx];
        if (selectSlot.item == null)return;
        
        selectedItem = selectSlot.item;
        selectedIdx = idx;
 
    }
    public void SelectItemFromEquipment(int idx)//장비창인덱스
    {
        ItemSlot selectSlot = slotPanel.equipmentSlots[idx];
        if (selectSlot.item == null || selectSlot.item == null)return;
        
        selectedItem = selectSlot.item;
        selectedIdx = idx;
 
    }
    public void RemoveSelectedItem()
    {
        switch (_selectedSlotType)
        {
            case SLOTTYPE.INVENTORY:
                var slot = slotPanel.inventorySlots[selectedIdx];
                slot.quantity--;
                if (slot.quantity <= 0)
                {
                    slot.item = null;
                    selectedItem = null;
                    selectedIdx = -1;
                }
                break;

            case SLOTTYPE.RECIPE:
                // 레시피는 일반적으로 삭제하지 않음
                Debug.LogWarning("레시피 슬롯에서 삭제는 허용되지 않습니다.");
                return;

            case SLOTTYPE.EQUIPMENT:
                var equipSlot = slotPanel.equipmentSlots[selectedIdx];
                equipSlot.item = null;
                equipSlot.quantity = 0;
                equipSlot.equipped = false;
                selectedItem = null;
                selectedIdx = -1;
                break;
        }

        InventoryUI.ClearSelectedItemWindow();
        InventoryUI.UIUpdate();
    }
    public void ThrowItem(ItemData data)
    {
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }
    public void RemoveItemByName(string name, int count)//설계도에서 사용할 이름으로 아이템지우기
    {
        foreach (var slot in slotPanel.inventorySlots)
        {
            if (slot.item != null && slot.item.displayName == name)
            {
                if (slot.quantity >= count)
                {
                    slot.quantity -= count;
                    if (slot.quantity <= 0)
                    {
                        slot.item = null;
                    }
                    InventoryUI.UIUpdate();
                    return;
                }
            }
        }
    }
    public void SaveInventory(SaveData data)
    {
        data.inventoryItems.Clear();

        for (int i = 0; i < slotPanel.inventorySlots.Length; i++)
        {
            var slot = slotPanel.inventorySlots[i];
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
        foreach (var slot in slotPanel.inventorySlots)
        {
            slot.Clear();
            slot.equipped = false;
        }

        // 아이템 로드
        foreach (var savedItem in data.inventoryItems)
        {
            if (savedItem.slotIndex < slotPanel.inventorySlots.Length)
            {
                ItemData itemData = ItemDatabase.Instance?.GetItemById(savedItem.itemId);
                if (itemData != null)
                {
                    var slot = slotPanel.inventorySlots[savedItem.slotIndex];
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
        InventoryUI.ClearSelectedItemWindow();
        InventoryUI.UIUpdate();
    }


}
