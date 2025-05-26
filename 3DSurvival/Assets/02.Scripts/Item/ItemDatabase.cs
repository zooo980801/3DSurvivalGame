using UnityEngine;
using System.Linq;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase Instance;

    [Header("아이템 리스트")]
    public ItemData[] items;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public ItemData GetItemById(string id)
    {
        return items.FirstOrDefault(item => item.id == id);
    }
}
