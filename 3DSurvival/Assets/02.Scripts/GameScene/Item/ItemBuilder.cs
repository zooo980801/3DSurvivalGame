using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBuilder : MonoBehaviour
{
    public GameObject placeablePrefab;     // 설치할 프리팹
    public float placeDistance = 2f;       // 설치 위치: 플레이어 앞 몇 미터

    public void InstallItem()
    {
        if (placeablePrefab == null) return;

        Vector3 placePos = transform.position + transform.forward * placeDistance;
        

        Instantiate(placeablePrefab, placePos, Quaternion.identity);
    }
}
