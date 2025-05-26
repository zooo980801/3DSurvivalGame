using UnityEngine;
using System.Linq;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase Instance; // 싱글톤 인스턴스

    [Header("아이템 리스트")]
    public ItemData[] items; // 등록된 아이템 데이터 배열 (인스펙터에서 설정)

    private void Awake()
    {
        if (Instance == null) // 싱글톤 인스턴스가 없을 경우
        {
            Instance = this; // 현재 인스턴스를 싱글톤으로 등록
            DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject); // 중복된 인스턴스는 제거
        }
    }

    public ItemData GetItemById(string id)
    {
        // 주어진 id와 일치하는 아이템을 items 배열에서 찾아 반환
        // 일치하는 것이 없으면 null 반환
        return items.FirstOrDefault(item => item.id == id);
    }
}