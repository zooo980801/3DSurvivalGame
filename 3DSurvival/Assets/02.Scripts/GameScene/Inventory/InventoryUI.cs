using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryWindow;
    private Inventory _inventory;
    
    [SerializeField] private DialogueManager dialogueManager;
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

    public void SelectItemUI(int idx) //찾은(눌린)아이템 에 대한 버튼UI
    {
        useBtn.SetActive(_inventory.selectedItem.type == ITEMTYPE.CONSUMABLE);
        equipBtn.SetActive(_inventory.selectedItem.type == ITEMTYPE.EQUIPABLE &&
                           !_inventory.slotPanel.itemSlots[idx].equipped);
        unEquipBtn.SetActive(_inventory.selectedItem.type == ITEMTYPE.EQUIPABLE &&
                             _inventory.slotPanel.itemSlots[idx].equipped);
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
                            CharacterManager.Instance.Player.status.Drink(InventoryManager.Instance.Inventory
                                .selectedItem.consumables[i].value);
                            break;
                        case CONSUMABLETYPE.HUNGER:
                            CharacterManager.Instance.Player.status.Eat(InventoryManager.Instance.Inventory.selectedItem
                                .consumables[i].value);
                            break;
                    }
                }
            }

            InventoryManager.Instance.Inventory.RemoveSelectedItem();
        }
    }

    public void OnCraftBtn()
    {
        var selectedItem = _inventory.slotPanel.itemSlots[_inventory.SelectedIdx].item;

        if (selectedItem == null || selectedItem.type != ITEMTYPE.CRAFT)
        {
            Debug.Log("선택된 아이템이 설계도가 아닙니다.");
            return;
        }

        CraftMaterial[] materials = selectedItem.materials;
        bool hasAllMaterials = true;

        // 1. 재료 확인
        foreach (var mat in materials)
        {
            int total = 0;

            foreach (var slot in _inventory.slotPanel.itemSlots)
            {
                if (slot.item != null && slot.item.matType == mat.type)
                {
                    total += slot.quantity;
                }
            }

            if (total < mat.value)
            {
                Debug.LogWarning($"재료 부족: {mat.type} (필요: {mat.value}, 보유: {total})");
                hasAllMaterials = false;
                break;
            }
        }

        // 2. 부족하면 종료
        if (!hasAllMaterials)
        {
            Debug.Log("재료가 부족하여 제작할 수 없습니다.");
            return;
        }

        // 3. 재료 차감
        foreach (var mat in materials)
        {
            int need = Mathf.CeilToInt(mat.value);
            for (int i = 0; i < _inventory.slotPanel.itemSlots.Length && need > 0; i++)
            {
                var slot = _inventory.slotPanel.itemSlots[i];

                if (slot.item != null && slot.item.matType == mat.type)
                {
                    int remove = Mathf.Min(slot.quantity, need);
                    slot.quantity -= remove;
                    need -= remove;

                    if (slot.quantity <= 0)
                        slot.item = null;
                }
            }
        }

        // 4. 결과 아이템 생성
        var result = selectedItem.craftItem;
        if (result != null)
        {
            CharacterManager.Instance.Player.itemData = result;
            CharacterManager.Instance.Player.addItem?.Invoke();
            Debug.Log($"{result.displayName} 제작 완료!");
        }
        else
        {
            Debug.Log("설계도에 결과 아이템(craftItem)이 설정되어 있지 않습니다.");
        }

        UIUpdate();
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


    public void OnCloseBtn()
    {
        inventoryWindow.SetActive(false);
        dialogueManager.EndConversation();
    }

}