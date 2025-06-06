using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [Header("스폰장소")]
    public Transform spawnCenter;       //원 중심 위치(플레이어 집)
    public Terrain terrain;
    public float innerRadius = 30f;     //생성 안할 안쪽 원 반지름 길이 (울타리 생기면 증가?)
    public float outerRadius = 80f;     //생성 할 바깥쪽 원 반지름 길이
    public float spawnTime = 30f;       //임시용 소환 쿨타임
    public float curTime = 0f;
    [Header("스폰시간")]
    public GameClock clock;
    public int spawnStartTime = 13;
    public int spawnEndTime = 22;
    [Header("스폰에너미")]
    public List<GameObject> enemyPrefabs;
    public GameObject[] playerHouse;   //생성돼서 향하는 타겟
    public GameObject player;   //플레이어
    private Enemy controller;

    // Start is called before the first frame update
    void Start()
    {
        TerrainData terrainData = terrain.terrainData;
    }

    // Update is called once per frame
    void Update()
    {
        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        if (clock.currentHour >= spawnStartTime && clock.currentHour <= spawnEndTime)
        {
            while (Time.time > curTime + spawnTime) //임시 조건
            {
                float angle = Random.Range(0f, Mathf.PI * 2f);      //랜덤 각도 0~360도(2파이)
                float radius = Random.Range(innerRadius, outerRadius);  //랜덤 반지름길이

                float x = spawnCenter.position.x + Mathf.Cos(angle) * radius;   //랜덤한 각도와 반지름으로 코사인을 이용해 x좌표 구하기
                float z = spawnCenter.position.z + Mathf.Sin(angle) * radius;   //랜덤한 각도와 반지름으로 사인을 이용해 z좌표 구하기
                float y = terrain.SampleHeight(new Vector3(x, 0, z)) + terrain.transform.position.y;    //터레인 x,z 좌표에 해당하는 y값(로컬좌표) + 터레인의 y값 => 월드 포지션값 구하기

                Vector3 spawnSpot = new Vector3(x, y, z);
                GameObject enemy = Instantiate(enemyPrefabs[0], spawnSpot, Quaternion.identity);

                controller = enemy.GetComponent<Enemy>();
                controller.playerHouse = playerHouse;
                controller.player = player;
                for(int i = 0;  i < playerHouse.Length; i++)
                {
                    if (playerHouse[i] != null)
                    {
                        controller.house = playerHouse[i].GetComponent<House>();
                        break;
                    }
                }

                curTime = Time.time;
            }
        }
    }
}
