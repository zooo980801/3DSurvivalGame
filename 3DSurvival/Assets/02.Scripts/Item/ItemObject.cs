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

    public string GetInteractPrompt()//커서가 아이템에 갔을 때 나오는 정보
    {
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    public void OnInteract()//상호작용 했을 때 일어나는 일
    {
        Destroy(gameObject);
    }
}
