using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class NPCWandering : MonoBehaviour
{
    [Header("AI")]
    private NavMeshAgent agent;
    public float detectDistance;//자동으로 혼자 배회하는 거리
    private ALSTATE aiState;

    [Header("Wandering")]
    public float walkSpeed = 3f;
    public float minWanderDistance = 2f;
    public float maxWanderDistance = 5f;
    public float minWanderWaitTime = 5f;
    public float maxWanderWaitTime = 10f;

    private Animator anim;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        agent.speed = walkSpeed;
        SetState(ALSTATE.IDLE);
    }

    void SetState(ALSTATE state)
    {
        aiState = state;

        switch(aiState)
        {
            case ALSTATE.IDLE:
                anim.SetBool("IsWalk", false);
                StartCoroutine(IdleRoutine());
                Debug.Log("멈춤");
                break;
            case ALSTATE.WANDERING:
                anim.SetBool("IsWalk", true);
                StartCoroutine(WanderRoutine());
                break;
        }
    }

    IEnumerator IdleRoutine()
    {
        float waitTime = Random.Range(minWanderWaitTime, maxWanderWaitTime);
        yield return new WaitForSeconds(waitTime);

        SetState(ALSTATE.WANDERING);
    }

    IEnumerator WanderRoutine()
    {
        Vector3 wanderTarget = GetWanderLocation();
        agent.SetDestination(wanderTarget);

        agent.isStopped = false;//이동재개

        // 실제로 이동 중일 때 루프
        while (agent.hasPath && agent.remainingDistance > agent.stoppingDistance)//유효한 경로이고 남은거리가 0보다 클때 
        {
            Debug.Log("걷는중");
            yield return null;
        }

        // 멈추고 Idle로 전환
        agent.isStopped = true;//이동 중지

        SetState(ALSTATE.IDLE);
    }
    Vector3 GetWanderLocation()
    {
        NavMeshHit hit;

        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);

        int i = 0;
        while (Vector3.Distance(transform.position, hit.position) < detectDistance)
        {
            //onUnitSphere는 반지름이 1인 가상의 구
            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);
            i++;
            if (i == 30)
                break;
        }

        return hit.position;
    }
}
