using UnityEngine;

public interface BluePrint
{
    string materialItem{get;set;}//
    string resultItem{get;set;}//결과아이템이름
    public void Craft();//만들기(재료여부확인, 재료소모, 결과물주기)
}

public class Crafting : MonoBehaviour,BluePrint
{
    public string materialItem { get; set; }
    public int quantity { get; set; }
    public string resultItem { get; set; }
    public void Craft()
    {
        var inventory = InventoryManager.Instance.Inventory;
        int materialCount = 0;

        //재료 개수체크
        foreach (var slot in inventory.slotPanel.itemSlots)
        {
            if (slot.item != null && slot.item.displayName == materialItem)
            {
                materialCount += slot.quantity;
            }
        }

        if (materialCount < 1)
        {
            Debug.Log("재료 부족");
            return;
        }

        // 재료소모
        inventory.RemoveItemByName(materialItem, quantity);

        //결과 아이템 생성
        ItemData result = Resources.Load<ItemData>($"ItemData/{resultItem}");
        if (result == null)
        {
            Debug.LogError($"결과 아이템 {resultItem} 을(를) Resources에서 찾을 수 없습니다.");
            return;
        }

        //플레이어에게 아이템 지급 (addItem 이벤트 사용)
        CharacterManager.Instance.Player.itemData = result;
        CharacterManager.Instance.Player.addItem?.Invoke();

        Debug.Log($"{resultItem} 제작 완료");
    }

}