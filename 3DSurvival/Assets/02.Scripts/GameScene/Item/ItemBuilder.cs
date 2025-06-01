using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBuilder : MonoBehaviour
{
    public ItemData item;     // 설치할 아이템
    public float placeDistance = 2f;       // 설치 위치: 플레이어 앞 몇 미터

    public void BuildingItem()
    {
        if (item == null) return;

        Vector3 placePos = transform.position + transform.forward * placeDistance;
        Instantiate(item.dropPrefab, placePos, Quaternion.identity);
    }
}
