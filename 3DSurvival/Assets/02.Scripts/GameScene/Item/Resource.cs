// Resource.cs

using UnityEngine;

public class Resource : MonoBehaviour
{
    public ItemData itemToGive;
    public int quantityPerHit = 1; // 기본 획득량
    public int capacity = 5; // 총 용량

    [Header("Bonus Resources with Specific Tools")]
    public int bonusQuantityOnToolUse = 1; // 특정 도구 사용 시 추가 획득량

    public void Gather(Vector3 hitPoint, Vector3 hitNormal, string toolIDUsed)
    {
        //기본 자원 드롭 로직 (도구 유무와 관계없이 실행)
        DropItem(itemToGive, quantityPerHit, hitPoint, hitNormal);

        if (capacity > 0)
        {
            // 특정 도구 사용 시 추가 획득 조건
            if (itemToGive != null && bonusQuantityOnToolUse > 0)
            {
                // 도끼로 나무를 쳤을 때
                if (toolIDUsed == "10" && this.CompareTag("Tree"))
                {
                    DropItem(itemToGive, bonusQuantityOnToolUse, hitPoint, hitNormal);
                }
                // 곡괭이로 돌을 쳤을 때
                else if (toolIDUsed == "11" && this.CompareTag("Rock"))
                {
                    DropItem(itemToGive, bonusQuantityOnToolUse, hitPoint, hitNormal);
                }
            }
        }
    }

    private void DropItem(ItemData itemData, int quantity, Vector3 hitPoint, Vector3 hitNormal)
    {
        for (int i = 0; i < quantity; i++)
        {
            if (capacity <= 0) break; // 용량이 없으면 드롭 중단
            capacity -= 1;
            Instantiate(itemData.dropPrefab, hitPoint + Vector3.up, Quaternion.LookRotation(hitNormal, Vector3.up));
        }
    }

    public void ResetCapacity()
    {
        capacity = 5;
    }
}