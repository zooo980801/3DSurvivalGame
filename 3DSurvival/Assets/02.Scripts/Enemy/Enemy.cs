using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,
    Attacking,
    Chasing,
    Move
}
public class Enemy : MonoBehaviour, IDamagable
{
    //public int hp;
    public int atk;
    public float speed;

    public EnemyState enemyState;
    public float playerDistance;    //타겟과의 거리
    public float detectedDistance = 10f;  //감지 거리
    public float chaseMaxDistance = 15f;     //추격가능한 거리
    public float attackDistance = 1f;
    public float attackCooldown = 1.5f;
    public float lastAttackTime;
    public ItemData[] dropOnDeath;

    public GameObject target;
    public GameObject playerHouse;
    public GameObject player;
    private NavMeshAgent agent;
    private Animator animator;

    public StatusData hp;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hp.CurValue <= 0)
        {
            Debug.Log("산적 피빵");
            Die();
        }
        playerDistance = Vector3.Distance(transform.position, player.transform.position);
        
        if (playerDistance < detectedDistance || (enemyState == EnemyState.Chasing && playerDistance < chaseMaxDistance))
        {
            
            ChasePlayer();
            if (playerDistance < attackDistance)
            {
                AttackPlayer();
            }
        }
        else
        {
            MoveToHouse();
        }
    }

    public void MoveToHouse()
    {
        enemyState = EnemyState.Move;
        speed = 0.5f;
        //animator.SetBool("Walk", true);
        //animator.SetBool("Sprint", false);
        //animator.SetBool("Punch", false);
        agent.SetDestination(playerHouse.transform.position);
    }

    public void ChasePlayer()
    {
        enemyState = EnemyState.Chasing;
        target = player;
        speed = 1.0f;
        //animator.SetBool("Walk", false);
        //animator.SetBool("Sprint", true);
        //animator.SetBool("Punch", false);
        agent.SetDestination(player.transform.position);
    }

    public void AttackPlayer()
    {
        enemyState = EnemyState.Attacking;
        speed = 0f;
        if(Time.time > lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            //animator.SetBool("Walk", false);
            //animator.SetBool("Sprint", false);
            //animator.SetBool("Punch", true);
            IDamagable damagable = player.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakePhysicalDamage(atk);
            }
        }
    }

    public void TakePhysicalDamage(int damage)
    {
        hp.Subtract(damage);
        Debug.Log("산적 아야");
        //onTakeDamage?.Invoke();     // 데미지를 받았다는 이벤트 발생
    
    }

    public void Die()
    {
        if (dropOnDeath  != null)
        {
            for (int i = 0; i > dropOnDeath.Length; i++)
            {
                Instantiate(dropOnDeath[i]);
                Debug.Log($"drop {i}");
            }
        }
        Destroy(gameObject);
    }
}   