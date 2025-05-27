using UnityEngine;
using System.Collections.Generic;

public class SaveTester : MonoBehaviour
{
    public void CreateInitialSaveData()
    {
        SaveData data = new SaveData
        {
            level = 1,
            exp = 0,
            gold = 999,

            health = new SaveStatusData
            {
                curValue = 100,
                maxValue = 100,
                passiveValue = 0.5f
            },
            stamina = new SaveStatusData
            {
                curValue = 50,
                maxValue = 50,
                passiveValue = 0.3f
            },
            hunger = new SaveStatusData
            {
                curValue = 80,
                maxValue = 100,
                passiveValue = 0.1f
            },
            thirst = new SaveStatusData
            {
                curValue = 70,
                maxValue = 100,
                passiveValue = 0.1f
            },

            inventory = new List<SaveItem>
            {
                new SaveItem { itemId = "apple", quantity = 3 },
                new SaveItem { itemId = "arrow", quantity = 15 }
            },
            equipped = new SaveEquipment
            {
                weaponId = "bow_basic",
                armorId = "armor_leather"
            },
            completedQuests = new List<string> { "quest_intro" }
        };

        SaveManager.Instance.SaveData(data);
        Debug.Log("[SaveTester] 초기 SaveData 저장 완료");
    }
}
