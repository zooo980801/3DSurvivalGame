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

    public string GetInteractPrompt() //아이템 바라봤을 때 리턴값
    {
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    public void OnInteract()//아이템 상호작용 리턴값
    {
        Destroy(gameObject);
    }
}
