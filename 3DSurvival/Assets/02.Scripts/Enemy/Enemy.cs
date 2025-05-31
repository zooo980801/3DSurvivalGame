using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Move,
    Chasing,
    Attacking,
}
public class Enemy : MonoBehaviour, IDamagable
{
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
        animator = GetComponent<Animator>();
        dropOnDeath[0] = ItemDatabase.Instance.items[0];
        dropOnDeath[1] = ItemDatabase.Instance.items[4];
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

        if(enemyState != EnemyState.Chasing && enemyState != EnemyState.Attacking)
        {
            enemyState = EnemyState.Move;
        }

        switch (enemyState)
        {
            case EnemyState.Move:
                MoveToHouse();
                break;
            case EnemyState.Chasing:
                ChasePlayer();
                break;
            case EnemyState.Attacking:
                AttackPlayer();
                break;
        }
    }

    public void MoveToHouse()
    {
        speed = 0.5f;
        animator.SetBool("IsWalk", true);
        animator.SetBool("IsChase", false);
        animator.SetBool("IsAttack", false);
        agent.SetDestination(playerHouse.transform.position);
        if (playerDistance < detectedDistance)
        {
            enemyState = EnemyState.Chasing;
            return;
        }
        //건물에 도착하면
        //멈춰서
        //건물에 데미지
    }

    public void ChasePlayer()
    {
        speed = 1f;
        animator.SetBool("IsWalk", false);
        animator.SetBool("IsChase", true);
        animator.SetBool("IsAttack", false);
        agent.SetDestination(player.transform.position);
        if (playerDistance < attackDistance)
        {
            enemyState = EnemyState.Attacking;
            return;
        }
        if (playerDistance > chaseMaxDistance)
        {
            enemyState = EnemyState.Move;
            return;
        }
    }

    public void AttackPlayer()
    {
        speed = 0f;
        if (playerDistance > attackDistance)
        {
            enemyState = EnemyState.Chasing;
            return;
        }
        if(Time.time > lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            animator.SetBool("IsWalk", false);
            animator.SetBool("IsChase", false);
            animator.SetBool("IsAttack", true);
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
        if (dropOnDeath != null)
        {
            for (int i = 0; i < dropOnDeath.Length; i++)
            {
                Instantiate(dropOnDeath[i]/*,transform.position, Quaternion.identity*/);
                Debug.Log($"drop {i}");
            }
        }
        Destroy(gameObject);
    }
}   