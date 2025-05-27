using System.Collections;
using UnityEngine;

public class SleepManager : MonoBehaviour
{
    public GameClock clock; // 게임 시간 참조 (외부에서 연결 또는 자동 할당)

    private bool isSleeping = false; // 현재 수면 중인지 여부

    void Start()
    {
        // clock이 연결되지 않았다면 씬에서 자동 탐색
        if (clock == null)
            clock = FindObjectOfType<GameClock>();

        // 시간 변경 이벤트 구독
        clock.OnTimeChanged += CheckSleep;
    }

    // 시간 변경 시 호출되는 콜백 함수
    void CheckSleep(int hour, int minute)
    {
        // 새벽 2시가 되면 수면 루틴 시작
        if (!isSleeping && hour == 2)
        {
            isSleeping = true;
            StartCoroutine(SleepRoutine());
        }
    }

    // 수면 처리 루틴 (몇 초 후에 자동 기상)
    IEnumerator SleepRoutine()
    {
        Debug.Log("잠에 듭니다...");

        // 수면 연출 대기 시간 (현실 시간 기준)
        yield return new WaitForSeconds(5f);

        Debug.Log("기상! 아침이 되었습니다.");

        // 시간 초기화 (오전 6시로 이동)
        clock.currentHour = 6;
        clock.currentMinute = 0;

        isSleeping = false;
    }
}
