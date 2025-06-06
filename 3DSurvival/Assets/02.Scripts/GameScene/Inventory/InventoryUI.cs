using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryWindow;
    private Inventory _inventory;
    public ItemCombine itemCombine;
    [SerializeField] private DialogueManager dialogueManager;
    
    [Header("Select ItemUI")]
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedItemStatName;
    public TextMeshProUGUI selectedItemStatValue;
    
    [Header("Seonbi StatusUI")]
    public TextMeshProUGUI seonbiCurrentStatusText;
    public TextMeshProUGUI seonbiResultStatusText;
    
    public TextMeshProUGUI playerStatusText;
    
    [Header("Inventory Button")]
    public GameObject useBtn;
    public GameObject equipBtn;
    public GameObject unEquipBtn;
    public GameObject dropBtn;
    
    public NPCStatus Seonbi;
    private PlayerController controller;
    private PlayerStatus _playerStatus;
    public PlayerStatus PlayerStatus{get{return _playerStatus;}}
    
    
    private float seonbiCurHunger;
    private float seonbiCurThirst;

    private bool isFirst = true;
    // public void OnTest()
    // {
    //     _inventory.AddTestItem(_inventory.selectedItem);
    // }

    private void OnEnable()
    {
        if (isFirst)
        {
            isFirst = false;
            return;
        }
        UIUpdate();
    }

    private void Start()
    {
        
        _playerStatus = CharacterManager.Instance.Player.status;
        Seonbi.Hunger.onValueChanged += SeonbiStatusUI;
        Seonbi.Thirst.onValueChanged += ChangedSlot;
        _playerStatus.Hunger.onValueChanged += InventoryPlayerInfo;
        _inventory = InventoryManager.Instance.Inventory;
        InventoryManager.Instance.Inventory.InventoryUI = this;
        controller = CharacterManager.Instance.Player.controller;
        _playerStatus = CharacterManager.Instance.Player.status;
        
        controller.inventory += Toggle;
        ClearSelectedItemWindow();
        inventoryWindow.SetActive(false);
    }

    public void SelectItemBtnUI(int idx) //찾은(눌린)아이템 에 대한 버튼UI
    {
        useBtn.SetActive(_inventory.selectedItem.type == ITEMTYPE.CONSUMABLE);
        
        equipBtn.SetActive(_inventory.selectedItem.type == ITEMTYPE.EQUIPABLE &&
                           !_inventory.slotPanel.inventorySlots[idx].equipped||
                           _inventory.selectedItem.type == ITEMTYPE.BUILDING&&
                           !_inventory.slotPanel.inventorySlots[idx].equipped);
        
        dropBtn.SetActive(true);
    }
    public void SelectEquipmentBtnUI(int idx) //장비창에서 찾은(눌린)아이템 에 대한 버튼UI
    {
        useBtn.SetActive(_inventory.selectedItem.type == ITEMTYPE.CONSUMABLE);
        unEquipBtn.SetActive(_inventory.selectedItem.type == ITEMTYPE.EQUIPABLE &&
                             _inventory.slotPanel.equipmentSlots[idx].equipped||
                             _inventory.selectedItem.type == ITEMTYPE.BUILDING&&
                             _inventory.slotPanel.equipmentSlots[idx].equipped);
        
        dropBtn.SetActive(true);
    }

    public void UIUpdate()//UI갱신
    {
        if (isFirst)
        {
            return;
        }
        for (int i = 0; i < _inventory.slotPanel.inventorySlots.Length; i++)
        {
            if (_inventory.slotPanel.inventorySlots[i].item != null)
            {
                _inventory.slotPanel.inventorySlots[i].Set();
            }
            else
            {
                _inventory.slotPanel.inventorySlots[i].Clear();
            }
        }

        InventoryManager.Instance.ItemData = null;
    }

    public void ClearSelectedItemWindow()//인벤토리창 초기화
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

    public void OnEquipBtn()
    {
        // 기존 장비가 있으면 해제
        if (_inventory.slotPanel.equipmentSlots[0].item != null)
        {
            UnEquip(0);
        }

        // 선택된 아이템 장비 슬롯으로 이동
        var selectedSlot = _inventory.slotPanel.inventorySlots[_inventory.SelectedIdx];

        _inventory.slotPanel.equipmentSlots[0].item = selectedSlot.item;
        _inventory.slotPanel.equipmentSlots[0].quantity = selectedSlot.quantity;
        _inventory.slotPanel.equipmentSlots[0].equipped = true;
        _inventory.slotPanel.equipmentSlots[0].Set();

        CharacterManager.Instance.Player.equip.EquipNew(selectedSlot.item);

        // 인벤토리 슬롯 비우기
        selectedSlot.item = null;
        selectedSlot.quantity = 0;
        selectedSlot.Clear();

        _inventory.selectedItem = null;
        InventoryManager.Instance.Inventory.ClearSelectedSlotState();
        ClearSelectedItemWindow();
        UIUpdate();
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
        var equipSlot = _inventory.slotPanel.equipmentSlots[idx];

        if (equipSlot.item == null) return;

        // 플레이어 해제 처리
        CharacterManager.Instance.Player.equip.UnEquip();

        // 장비 해제, 인벤토리에 되돌리기
        CharacterManager.Instance.Player.itemData = equipSlot.item;
        CharacterManager.Instance.Player.addItem(); // 인벤토리에 추가됨
        CharacterManager.Instance.Player.itemData = null;

        // 슬롯 비우기
        equipSlot.item = null;
        equipSlot.quantity = 0;
        equipSlot.equipped = false;
        equipSlot.Clear();

        _inventory.selectedItem = null;
        ClearSelectedItemWindow();
        UIUpdate();
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

    public void ChangedSlot()
    {
        if (InventoryManager.Instance.Inventory.selectedItem is null)
        {
            return;
        }
        if (InventoryManager.Instance.Inventory.selectedItem.type == ITEMTYPE.CONSUMABLE)
        {
            for (int i = 0; i < InventoryManager.Instance.Inventory.selectedItem.consumables.Length; i++)
            {
                if (i >= 0 && i < InventoryManager.Instance.Inventory.selectedItem.consumables.Length)
                {
                    switch (InventoryManager.Instance.Inventory.selectedItem.consumables[i].type)
                    {
                        case CONSUMABLETYPE.THIRST:
                            seonbiCurThirst = InventoryManager.Instance.Inventory.selectedItem.consumables[i].value;
                            break;
                        case CONSUMABLETYPE.HUNGER:
                            seonbiCurHunger = InventoryManager.Instance.Inventory.selectedItem.consumables[i].value;
                            break;
                    }
                }
            }
        }
    }

    public void OnFoodEatBtn()
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
                            Seonbi.Drink(InventoryManager.Instance.Inventory.selectedItem.consumables[i].value);
                            break;
                        case CONSUMABLETYPE.HUNGER:
                            Seonbi.Eat(InventoryManager.Instance.Inventory.selectedItem.consumables[i].value);
                            break;
                    }
                }
            }

            InventoryManager.Instance.Inventory.RemoveSelectedItem();
        }
    }

    public void SeonbiStatusUI()
    {
        seonbiCurrentStatusText.text = $"배고픔 : {Seonbi.Hunger.CurValue:f0}/{Seonbi.Hunger.MaxValue}\n목마름 : {Seonbi.Thirst.CurValue:f0}/{Seonbi.Thirst.MaxValue}";
        seonbiResultStatusText.text = $"배고픔 : {((Seonbi.Hunger.CurValue + seonbiCurHunger)>=100? 100:Seonbi.Hunger.CurValue + seonbiCurHunger) :f0}/{Seonbi.Hunger.MaxValue:f0}\n목마름{((Seonbi.Thirst.CurValue + seonbiCurThirst)>=100?100:Seonbi.Thirst.CurValue + seonbiCurThirst):f0}/{Seonbi.Thirst.MaxValue}";
    }

    public void InventoryPlayerInfo()
    {
        playerStatusText.text = $"체력 : {_playerStatus.Health.CurValue:f0}/{_playerStatus.Health.MaxValue}\n체력재생 : {_playerStatus.Health.PassiveValue:f0}\n공격력 : {_playerStatus.AttackPower:f0}";
    }

    public void CraftingUnEquip()
    {
        var equipSlot = _inventory.slotPanel.equipmentSlots[0];

        equipSlot.item = null;
        equipSlot.quantity = 0;
        equipSlot.equipped = false;
        equipSlot.Clear();

        _inventory.selectedItem = null;
        ClearSelectedItemWindow();
        UIUpdate();
    }
    //제작 창 열기
    public void CreftingUI()
    {
        InventoryManager.Instance.CraftingBG.SetActive(true);
        InventoryManager.Instance.InventoryBG.SetActive(false);
        InventoryManager.Instance.SeonbiBG.SetActive(false);
        inventoryWindow.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
}