using System;
using UnityEngine;


// 구조 설명:
// ItemData : 아이템의 설계 및 기본 정보 보유 (ScriptableObject)
// SaveItem : 저장 시 사용하는 ID + 수량 구조체 (별도 정의되어 있어야 함)
// SaveData : 전체 게임 상태를 저장하는 마스터 클래스 (별도 정의되어 있어야 함)

// 소비 아이템 효과 정보 클래스
[System.Serializable] // 직렬화하여 인스펙터에 보이게 함
public class ItemDataConsumable
{
    public CONSUMABLETYPE type; // 효과 종류 (허기, 체력 등)
    public float value;         // 회복/감소 등 효과량
}
[Serializable]//직렬화하여 인스펙터에 표시
public class CraftMaterial
{
    public MATERIALTYPE type;//재료타입
    public float value;//재료갯수
}

// ScriptableObject를 생성할 수 있게 하는 속성
[CreateAssetMenu(fileName = "Item", menuName = "new Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string id;                // 저장 및 참조용 고유 ID (예: "item_apple")
    public string displayName;      // 게임 내 표시될 이름
    public string description;      // 아이템 설명
    public ITEMTYPE type;           // 아이템 종류
    public Sprite icon;             // UI에서 사용할 아이콘
    public GameObject dropPrefab;   // 월드에 떨어뜨릴 때의 프리팹

    [Header("Stacking")]
    public bool canStack;           // 아이템 중첩 가능 여부
    public int maxStackAmount;      // 최대 중첩 수량

    [Header("Equip")]
    public int damage;          // 피해량
    public int maxDurability;   // 최대 내구도
    public int currentDurability;   // 현재 내구도


    [Header("Consumable")]
    public ItemDataConsumable[] consumables; // 소비 효과 리스트 (여러 효과 적용 가능)
    
    [Header("Material")]
    public CraftMaterial[] material;//아이템 제작에사용할 재료리스트
}