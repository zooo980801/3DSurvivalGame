using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class SpawnAnimal : MonoBehaviour
{
    public List<GameObject> animalPrefabs;
    public List<GameObject> spawnedAnimals;
    public int maxAnimalCount = 50;
    public Transform spawnCenter;       //원 중심 위치(플레이어 집)
    public Terrain terrain;
    public float innerRadius = 20f;     //생성 안할 안쪽 원 반지름 길이 (울타리 생기면 증가?)
    public float outerRadius = 80f;     //생성 할 바깥쪽 원 반지름 길이
    public float spawnTime = 30f;       //임시용 소환 쿨타임
    public float curTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        StartSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnAnimals()
    {

        //for (int i = 0; i < maxAnimalCount; i++) //임시 조건
        {
            float angle = Random.Range(0f, Mathf.PI * 2f);      //랜덤 각도 0~360도(2파이)
            float radius = Random.Range(innerRadius, outerRadius);  //랜덤 반지름길이

            float x = spawnCenter.position.x + Mathf.Cos(angle) * radius;   //랜덤한 각도와 반지름으로 코사인을 이용해 x좌표 구하기
            float z = spawnCenter.position.z + Mathf.Sin(angle) * radius;   //랜덤한 각도와 반지름으로 사인을 이용해 z좌표 구하기
            float y = terrain.SampleHeight(new Vector3(x, 0, z)) + terrain.transform.position.y;    //터레인 x,z 좌표에 해당하는 y값(로컬좌표) + 터레인의 y값 => 월드 포지션값 구하기

            Vector3 spawnSpot = new Vector3(x, y, z);
            GameObject animal = Instantiate(animalPrefabs[0], spawnSpot, Quaternion.identity);
            spawnedAnimals.Add(animal);
        }
    }

    public void StartSpawn()
    {
        for (int i = 0; i < maxAnimalCount; i++)
        {
            SpawnAnimals();
        }
    }
}
