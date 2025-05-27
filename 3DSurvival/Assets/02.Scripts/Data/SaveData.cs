using System.Collections.Generic;

[System.Serializable]
public class SaveData // ← 저장용 클래스 명칭
{
    public int level;
    public int exp;
    public int gold;

    public SaveStatusData health;
    public SaveStatusData stamina;
    public SaveStatusData hunger;
    public SaveStatusData thirst;

    public List<SaveItem> inventory = new List<SaveItem>();
    public SaveEquipment equipped;
    public List<string> completedQuests = new List<string>();
}

[System.Serializable]

public class SaveStatusData
{
    public float curValue;
    public float maxValue;
    public float passiveValue;
}

public class SaveItem // 저장용 아이템
{
    public string itemId;
    public int quantity;
}

[System.Serializable]
public class SaveEquipment // 저장용 장비 정보
{
    public string weaponId;
    public string armorId;
}
