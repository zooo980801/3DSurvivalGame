using UnityEngine;

public class SaveTester : MonoBehaviour
{
    private void Start()
    {
        SaveData data = new SaveData
        {
            level = 1,
            hp = 100,
            stamina = 50,
            exp = 0,
            gold = 999
        };

        data.inventory.Add(new SaveItem { itemId = "apple", quantity = 3 });
        data.inventory.Add(new SaveItem { itemId = "arrow", quantity = 15 });

        data.equipped = new SaveEquipment
        {
            weaponId = "bow_basic",
            armorId = "armor_leather"
        };

        data.completedQuests.Add("quest_intro");

        SaveManager.Instance.SaveData(data); // ✅ 실제 저장
        Debug.Log("[TEST] Save complete!");
    }
}
