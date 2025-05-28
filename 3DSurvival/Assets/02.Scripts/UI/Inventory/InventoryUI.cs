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


        public GameObject test;

        void OnTest()
        {
            _inventory.slotPanel.AddItem();
        }
        private void Start()
        {
            _inventory = InventoryManager.Instance.Inventory;
            InventoryManager.Instance.InventoryUI = this;
        }
        
        public void SelectItemUI(int idx)//찾은(눌린)아이템 UI
        {
            selectedItemName.text = _inventory.selectedItem.displayName;
            selectedItemDescription.text = _inventory.selectedItem.description;
            
            selectedItemStatName.text = string.Empty;
            selectedItemStatValue.text = string.Empty;
            
            for (int i = 0; i < _inventory.selectedItem.consumables.Length; i++)
            {
                selectedItemStatName.text += _inventory.selectedItem.consumables[i].type.ToString() + "\n";
                selectedItemStatValue.text += _inventory.selectedItem.consumables[i].value.ToString() + "\n";
            }
            
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
                            case CONSUMABLETYPE.HEALTH:
                                InventoryManager.Instance.PlayerStatus.Heal(InventoryManager.Instance.Inventory.selectedItem.consumables[i].value);
                                break;
                            case CONSUMABLETYPE.HUNGER:
                                InventoryManager.Instance.PlayerStatus.GetStamina(InventoryManager.Instance.Inventory.selectedItem.consumables[i].value);
                                break;
                        }
                    }
                }
    
                InventoryManager.Instance.Inventory.RemoveSelectedItem();
            }
        }
        public void OnDropBtn()
        {
            _inventory.ThrowItem(InventoryManager.Instance.Inventory.selectedItem);
            InventoryManager.Instance.Inventory.RemoveSelectedItem();
        }
    }