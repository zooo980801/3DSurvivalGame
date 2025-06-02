using System.Collections.Generic;

[System.Serializable]
public class SavedDroppedItem
{
    public string itemId;
    public float posX, posY, posZ;
    public float rotX, rotY, rotZ;
    public int amount;
}

[System.Serializable]
public class SavedHouse
{
    public string prefabId; // 예: "house_basic"
    public float hp;
    public float posX, posY, posZ;
    public bool isDestroyed;
}

[System.Serializable]
public class SavedNPC
{
    public string npcId; // 고유 ID (ex: "npc_kim")
    public int curLevel;
    public int curExp;
    public SaveStatusData hunger;
    public SaveStatusData thirst;
}
[System.Serializable]
public class SaveData // ← 저장용 클래스 명칭
{
    public SaveLevel level;
    public int gold;

    public SaveStatusData playerHunger;
    public SaveStatusData playerThirst;
    public SaveStatusData health;
    public SaveStatusData stamina;
    public SaveStatusData hunger;
    public SaveStatusData thirst;

    public List<SavedItem> inventoryItems = new List<SavedItem>();
    public SaveEquipment equipped;
    public List<SavedNPC> npcs = new List<SavedNPC>();
    public List<SavedHouse> houses = new List<SavedHouse>();
    public List<string> completedQuests = new List<string>();
    public List<SavedDroppedItem> droppedItems = new();

    public int currentDay;
    public int currentHour;
    public int currentMinute;

    public float playerPosX;
    public float playerPosY;
    public float playerPosZ;
    public float playerRotY;
    public float cameraRotX;

}

[System.Serializable]

public class SaveStatusData
{
    public float curValue;
    public float maxValue;
    public float passiveValue;
}
[System.Serializable]
public class SaveLevel
{
    public int curLevel;
    public int curExp;
}
[System.Serializable]
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