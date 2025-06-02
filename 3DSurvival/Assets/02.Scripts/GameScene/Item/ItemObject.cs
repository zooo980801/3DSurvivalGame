using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IInteractable
{
    public string GetInteractPrompt();
    public void OnInteract();
}
public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;
    public int quantity = 1; // 필드에 드랍된 수량

    public string GetInteractPrompt() //아이템 바라봤을 때 리턴값
    {
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    public void OnInteract()//아이템 상호작용 리턴값
    {
        if (data.id == "13")//제작대이면
        {
            InventoryManager.Instance.Inventory.InventoryUI.CreftingUI();
        }
        else
        {
            CharacterManager.Instance.Player.itemData = data;
            CharacterManager.Instance.Player.addItem?.Invoke();
            Destroy(gameObject);
        }
    }

    public SavedDroppedItem WriteSave()
    {
        return new SavedDroppedItem
        {
            itemId = data.id,
            posX = transform.position.x,
            posY = transform.position.y,
            posZ = transform.position.z,
            rotX = transform.eulerAngles.x,
            rotY = transform.eulerAngles.y,
            rotZ = transform.eulerAngles.z,
            amount = quantity
        };
    }
}
