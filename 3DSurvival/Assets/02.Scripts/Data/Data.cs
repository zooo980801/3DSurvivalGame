using System.Collections.Generic;

[System.Serializable]
public class Data
{
    // 저장하고싶은 데이터 목록들 ( 원하면 추가 바랍니다 )
    public int level;
    public int hp;
    public int stamina;
    public int exp;
    public int gold;

    // 아이템 인벤토리 (아이템 ID 리스트 또는 직렬화 가능한 구조체 사용)
    public List<ItemData> inventory = new List<ItemData>();

    // 착용 장비
    public EquipmentData equipped;

    // 완료한 퀘스트 ID
    public List<string> completedQuests = new List<string>();
}

[System.Serializable]
public class ItemData
{
    // 아이템 데이터들 ( 원하면 추가 바랍니다 )
    public string itemId;   // 예: "potion_01"
    public int quantity;    // 예: 3
}

[System.Serializable]
public class EquipmentData
{
    // 착용 아이템 데이터들 ( 원하면 추가 바랍니다 )
    public string weaponId;
    public string armorId;
}
