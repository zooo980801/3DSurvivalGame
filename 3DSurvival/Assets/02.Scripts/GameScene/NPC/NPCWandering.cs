using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class NPCWandering : MonoBehaviour, IInteractable
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

    [Header("Look At Player")]
    public Transform playerTransform;   // 플레이어 Transform을 인스펙터에 연결
    public float lookAtSpeed = 5f;  // 회전 속도

    private Animator anim;
    public DialogueManager dialogueManager;//임시
    private Coroutine stateRoutine;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        agent.speed = walkSpeed;
        // 시작은 Idle 상태로
        SetState(ALSTATE.IDLE);
    }


    private void Update()
    {
        // 대화 중이라면 무조건 Idle
        if (dialogueManager.isTalk)
        {
            EnterIdle();//강제로 Idle
            LookAtPlayer();//대화중엔 플레이어 쳐다보기
            return;
        }

        // 대화가 끝났고 Idle 상태이고 코루틴이 없을 때 다시 배회 시작
        if (aiState == ALSTATE.IDLE && stateRoutine == null && dialogueManager.isTalk == false)
        {
            SetState(ALSTATE.WANDERING);
        }
    }

    // 강제 Idle 진입: 코루틴 정리, 애니메이션 이동 멈춤
    private void EnterIdle()
    {
        StopStateRoutine();
        aiState = ALSTATE.IDLE;
        anim.SetBool("IsWalk", false);
        agent.ResetPath();
        // Idle 상태에선 대기 루틴 실행하지 않고,
        // 대화가 끝난 뒤 Update에서 배회 재개
    }

    // 상태 전환 메서드
    void SetState(ALSTATE state)
    {
        StopStateRoutine();
        aiState = state;

        switch (state)
        {
            case ALSTATE.IDLE:
                anim.SetBool("IsWalk", false);
                stateRoutine = StartCoroutine(IdleRoutine());
                break;

            case ALSTATE.WANDERING:
                anim.SetBool("IsWalk", true);
                stateRoutine = StartCoroutine(WanderRoutine());
                break;
        }
    }

    // 실행 중인 코루틴 정리

    private void StopStateRoutine()
    {
        if (stateRoutine != null)
        {
            StopCoroutine(stateRoutine);
            stateRoutine = null;
        }
    }

    IEnumerator IdleRoutine()//Idle 코루틴
    {
        float waitTime = Random.Range(minWanderWaitTime, maxWanderWaitTime);
        yield return new WaitForSeconds(waitTime);
        if (dialogueManager.isTalk)
        {
            stateRoutine = null;
            yield break;
        }
        // 코루틴 종료 표시
        stateRoutine = null;
        SetState(ALSTATE.WANDERING);
    }

    #region 배회할 때 부르는 코루틴
    IEnumerator WanderRoutine()
    {
        Vector3 wanderTarget = GetWanderLocation();
        agent.SetDestination(wanderTarget);
        agent.isStopped = false;

        float startTime = Time.time; //걷기 시작 시간
        float maxWanderTime = 5f; //5초 내에 목표 지점에 도착하지 못하면 재계산 하기 위한 변수

        // 경로 계산 대기
        while (agent.pathPending)
            yield return null;

        // 실제 이동 중일 때만 루프
        while (agent.hasPath && agent.remainingDistance > agent.stoppingDistance)
        {
            if(Time.time - startTime > maxWanderTime)
            {
                Debug.Log("[NPCWandering] 배회 시간초과. 재계산");
                agent.isStopped = true;
                stateRoutine = null;
                SetState(ALSTATE.IDLE);
                yield break;
            }
            yield return null;
        }

        agent.isStopped = true;
        stateRoutine = null;
        SetState(ALSTATE.IDLE);
    }
    #endregion
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

    public string GetInteractPrompt()
    {
        string str = "대화";
        return str;
    }

    public void OnInteract()
    {
        dialogueManager.isTalk = true;
        dialogueManager.StartConversation();
    }
    private void LookAtPlayer()
    {
        // 높이 맞추기
        Vector3 targetPos = playerTransform.position;
        targetPos.y = transform.position.y;

        // 회전 계산
        Quaternion targetRot = Quaternion.LookRotation(targetPos - transform.position);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            Time.deltaTime * lookAtSpeed
        );
    }
}
