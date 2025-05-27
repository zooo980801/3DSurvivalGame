using UnityEngine;

public class SaveTester : MonoBehaviour
{
    public void CreateInitialSaveData()
    {
        SaveData data = new SaveData
        {
            level = 1,
            hp = 100,
            stamina = 50,
            exp = 0,
            gold = 999,
            inventory = new System.Collections.Generic.List<SaveItem>(),
            equipped = new SaveEquipment(),
            completedQuests = new System.Collections.Generic.List<string>()
        };

        data.inventory.Add(new SaveItem { itemId = "apple", quantity = 3 });
        data.inventory.Add(new SaveItem { itemId = "arrow", quantity = 15 });

        data.equipped.weaponId = "bow_basic";
        data.equipped.armorId = "armor_leather";

        data.completedQuests.Add("quest_intro");

        SaveManager.Instance.SaveData(data); // ✅ 실제 저장
        Debug.Log("[SaveTester] 초기 SaveData 저장 완료");
    }
}
