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

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = walkSpeed;
        SetState(ALSTATE.IDLE);
    }

    void SetState(ALSTATE state)
    {
        aiState = state;

        switch(aiState)
        {
            case ALSTATE.IDLE:
                StartCoroutine(IdleRoutine());
                break;
            case ALSTATE.WANDERING:
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

        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }

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
