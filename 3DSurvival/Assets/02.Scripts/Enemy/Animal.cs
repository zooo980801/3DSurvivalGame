using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Animal : MonoBehaviour, IDamagable
{
    public StatusData hp;
    public ItemData[] dropOnDeath;
    public SpawnAnimal spawnManager;

    public void TakePhysicalDamage(int damage)
    {
        hp.Subtract(damage);
        Debug.Log("동물 아야");
        //onTakeDamage?.Invoke();     // 데미지를 받았다는 이벤트 발생
        //반대방향으로 도망치기
    }

    public void Die()
    {
        if (dropOnDeath != null)
        {
            for (int i = 0; i < dropOnDeath.Length; i++)
            {
                Instantiate(dropOnDeath[i].dropPrefab, transform.position, Quaternion.Euler(Vector3.one * Random.value * 360));
                Debug.Log($"drop {dropOnDeath[i].displayName}");
            }
        }
        spawnManager.spawnedAnimals.Remove(gameObject);
        spawnManager.SpawnAnimals();
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        spawnManager = FindObjectOfType<SpawnAnimal>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hp.CurValue <= 0)
        {
            Debug.Log("동물 피빵");
            Die();
        }
    }
}
