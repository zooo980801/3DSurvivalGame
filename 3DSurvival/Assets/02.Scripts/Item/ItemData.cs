using UnityEngine;


//구조
// ItemData : 아이템 설계 정보
// SaveIem : 세이브시 사용하는 ID+ 수량 저장 전용
// SaveData : 전체 게임 상태 저장을 위한 마스터클래스
public enum ITEMTYPE { Resource, Equipable, Consumable }
public enum CONSUMABLETYPE { Hunger, Health } 

[System.Serializable]
public class ItemDataConsumable
{
    public CONSUMABLETYPE type;
    public float value;
}

[CreateAssetMenu(fileName = "Item", menuName = "Game/Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string id; // 저장 시 참조할 고유 ID
    public string displayName;
    public string description;
    public ITEMTYPE type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;
}
