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
    AttackPlayer,
    AttackHouse,
}
public class Enemy : MonoBehaviour, IDamagable
{
    public int atk;
    public float speed;

    public EnemyState enemyState;
    public float playerDistance;    //플레이어와의 거리
    public float detectedDistance = 10f;  //감지 거리
    public float chaseMaxDistance = 15f;     //추격가능한 거리
    public float attackDistance = 2f;
    public float attackCooldown = 1.5f;
    public float lastAttackTime;
    public float lookAtSpeed = 5f;
    public ItemData[] dropOnDeath;

    public GameObject target;
    public GameObject[] playerHouse;
    public House house;
    public float samplePositionDistance = 10f;
    public GameObject player;
    private NavMeshAgent agent;
    private Animator animator;
    private int index = 0;
    [SerializeField] private float remainingDistance;
    NavMeshHit hit;


    public StatusData hp;

    private CreatureSoundHandler soundHandler;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        //house = FindObjectOfType<House>();
        soundHandler = GetComponent<CreatureSoundHandler>();

        dropOnDeath[0] = ItemDatabase.Instance.items[0];    //아이템 책
        dropOnDeath[1] = ItemDatabase.Instance.items[4];    //아이템 야채
        enemyState = EnemyState.Move;
        
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

        if (playerHouse[index] == null)
        {
            index++;
            Debug.Log($"index = {index}");
            house = playerHouse[index].GetComponent<House>();
            if (NavMesh.SamplePosition(playerHouse[index].transform.position, out hit, samplePositionDistance, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
                remainingDistance = agent.remainingDistance;    //인스펙터 확인용
            }
        }

        //if (enemyState != EnemyState.Chasing && enemyState != EnemyState.AttackPlayer && enemyState != EnemyState.AttackHouse)
        //{
        //    enemyState = EnemyState.Move;
        //}

        switch (enemyState)
        {
            case EnemyState.Move:
                MoveToHouse();
                break;
            case EnemyState.Chasing:
                ChasePlayer();
                break;
            case EnemyState.AttackPlayer:
                AttackPlayer();
                break;            
            case EnemyState.AttackHouse:
                AttackHouse();
                break;
        }
    }
    public void LookAtPlayer()
    {
        // 높이 맞추기
        Vector3 targetPos = player.transform.position;
        targetPos.y = transform.position.y;

        // 회전 계산
        Quaternion targetRot = Quaternion.LookRotation(targetPos - transform.position);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            Time.deltaTime * lookAtSpeed
        );
    }
    public void MoveToHouse()
    {

        speed = 0.5f;
        agent.isStopped = false;
        animator.SetBool("IsWalk", true);
        animator.SetBool("IsChase", false);
        animator.SetBool("IsAttack", false);
        //agent.SetDestination(playerHouse[0].transform.position);
        
        if (NavMesh.SamplePosition(playerHouse[index].transform.position, out hit, samplePositionDistance, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            remainingDistance = agent.remainingDistance;    //인스펙터 확인용
        }
        if (playerDistance < detectedDistance)
        {
            enemyState = EnemyState.Chasing;
            return;
        }
        //건물에 도착하면
        //if (agent.remainingDistance < attackDistance)
        if (Vector3.Distance(transform.position, hit.position) < attackDistance)
        {
            enemyState = EnemyState.AttackHouse;
            return;
        }

    }

    public void ChasePlayer()
    {
        agent.isStopped = false;
        speed = 1f;
        LookAtPlayer();
        animator.SetBool("IsWalk", false);
        animator.SetBool("IsChase", true);
        animator.SetBool("IsAttack", false);
        agent.SetDestination(player.transform.position);
        remainingDistance = agent.remainingDistance;    //인스펙터 확인용
        if (playerDistance < attackDistance)
        {
            enemyState = EnemyState.AttackPlayer;
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
        agent.isStopped = true;
        LookAtPlayer();
        if (playerDistance > attackDistance)
        {
            enemyState = EnemyState.Chasing;
            return;
        }

        if (Time.time > lastAttackTime + attackCooldown)
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

    public void AttackHouse()
    {
        agent.isStopped = true;
        if (playerDistance < detectedDistance)
        {
            enemyState = EnemyState.Chasing;
            return;
        }
        if (Vector3.Distance(transform.position, hit.position) > attackDistance)
        {
            enemyState = EnemyState.Move;
            return;
        }
        if (Time.time > lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            animator.SetBool("IsWalk", false);
            animator.SetBool("IsChase", false);
            animator.SetBool("IsAttack", true);
            house.TakeDamage(atk);
        }
    }

    public void TakePhysicalDamage(int damage)
    {
        soundHandler.DamageSound();
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
                Instantiate(dropOnDeath[i].dropPrefab, transform.position, Quaternion.Euler(Vector3.one * Random.value * 360));
                Debug.Log($"drop {dropOnDeath[i].displayName}");
            }
        }
        Destroy(gameObject);
    }
}   