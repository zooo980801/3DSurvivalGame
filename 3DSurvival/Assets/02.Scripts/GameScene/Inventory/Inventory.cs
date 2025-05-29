using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public SlotPanel slotPanel;
    public ItemData selectedItem;
    private int selectedIdx;
    public int SelectedIdx{get{return selectedIdx;}set{selectedIdx=value;}}
    public Transform dropPosition;
    void Start()
    {
        InventoryManager.Instance.Inventory = this;

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

}
