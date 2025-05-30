using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryWindow;
    private Inventory _inventory;
    [Header("Select ItemUI")]
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedItemStatName;
    public TextMeshProUGUI selectedItemStatValue;
    public GameObject useBtn;
    public GameObject equipBtn;
    public GameObject unEquipBtn;
    public GameObject dropBtn;

    public Crafting crafting;
    public GameObject test;
    private PlayerController controller;
    private PlayerStatus status;

    private int curEquipIdx;
    // public void OnTest()
    // {
    //     _inventory.AddTestItem(_inventory.selectedItem);
    // }
    
    private void Start()
    {
        _inventory = InventoryManager.Instance.Inventory;
        InventoryManager.Instance.InventoryUI = this;
        controller = CharacterManager.Instance.Player.controller;
        status = CharacterManager.Instance.Player.status;

        controller.inventory += Toggle;
        ClearSelectedItemWindow();
        inventoryWindow.SetActive(false);
    }

    public void SelectItemUI(int idx)//찾은(눌린)아이템 UI
    {
        useBtn.SetActive(_inventory.selectedItem.type == ITEMTYPE.CONSUMABLE);
        equipBtn.SetActive(_inventory.selectedItem.type == ITEMTYPE.EQUIPABLE && !_inventory.slotPanel.itemSlots[idx].equipped);
        unEquipBtn.SetActive(_inventory.selectedItem.type == ITEMTYPE.EQUIPABLE && _inventory.slotPanel.itemSlots[idx].equipped);
        dropBtn.SetActive(true);

    }
    public void UIUpdate()
    {
        for (int i = 0; i < _inventory.slotPanel.itemSlots.Length; i++)
        {
            if (_inventory.slotPanel.itemSlots[i].item != null)
            {
                _inventory.slotPanel.itemSlots[i].Set();
            }
            else
            {
                _inventory.slotPanel.itemSlots[i].Clear();
            }
        }

        InventoryManager.Instance.ItemData = null;
    }
    public void ClearSelectedItemWindow()
    {
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        useBtn.SetActive(false);
        equipBtn.SetActive(false);
        unEquipBtn.SetActive(false);
        dropBtn.SetActive(false);
    }
    public void OnUseBtn()
    {
        if (InventoryManager.Instance.Inventory.selectedItem.type == ITEMTYPE.CONSUMABLE)
        {
            for (int i = 0; i < InventoryManager.Instance.Inventory.selectedItem.consumables.Length; i++)
            {
                if (i >= 0 && i < InventoryManager.Instance.Inventory.selectedItem.consumables.Length)
                {
                    switch (InventoryManager.Instance.Inventory.selectedItem.consumables[i].type)
                    {
                        case CONSUMABLETYPE.THIRST:
                            CharacterManager.Instance.Player.status.Drink(InventoryManager.Instance.Inventory.selectedItem.consumables[i].value);
                            break;
                        case CONSUMABLETYPE.HUNGER:
                            CharacterManager.Instance.Player.status.Eat(InventoryManager.Instance.Inventory.selectedItem.consumables[i].value);
                            break;
                    }
                }
            }

            InventoryManager.Instance.Inventory.RemoveSelectedItem();
        }
    }

    public void OnEquipBtn()
    {
        if (_inventory.slotPanel.itemSlots[curEquipIdx].equipped)
        {
            UnEquip(curEquipIdx);
        }
        _inventory.slotPanel.itemSlots[curEquipIdx].equipped = false;
        curEquipIdx = _inventory.SelectedIdx;
        CharacterManager.Instance.Player.equip.EquipNew(_inventory.selectedItem);
        UIUpdate();
        _inventory.SelectItem(_inventory.SelectedIdx);
    }

    
    public void OnDropBtn()
    {
        _inventory.ThrowItem(InventoryManager.Instance.Inventory.selectedItem);
        InventoryManager.Instance.Inventory.RemoveSelectedItem();
    }

    public void Toggle()
    {
        inventoryWindow.SetActive(!inventoryWindow.activeInHierarchy);
    }
    public void UnEquip(int idx)
    {
        _inventory.slotPanel.itemSlots[idx].equipped = false;
        CharacterManager.Instance.Player.equip.UnEquip();
        UIUpdate();
        if (_inventory.SelectedIdx == idx)
        {
            _inventory.SelectItem(idx);
        }
    }

    public void OnUnEquipBtn()
    {
        UnEquip(_inventory.SelectedIdx);  
    }
    public void OnCraftBtn()
    {
        crafting.materialItem = "Wood";        // 예시: 나무를 재료로
        crafting.resultItem = "Axe";           // 예시: 결과 아이템은 도끼
        crafting.Craft();
    }
    
    
}