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
public class Enemy : MonoBehaviour
{
    public float hp;
    public float atk;
    public float speed = 3;
    public float detectedDistance;  //감지 거리
    public float playerDistance;    //플레이어와의 거리
    public float ChaseMaxDistance;     //추격가능한 거리
    public EnemyState enemyState;
    public GameObject playerHouse;
    private NavMeshAgent agent;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveToHouse();
    }

    public void MoveToHouse()
    {
        agent.SetDestination(playerHouse.transform.position);
    }
}
