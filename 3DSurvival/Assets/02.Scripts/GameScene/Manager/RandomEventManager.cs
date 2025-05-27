using UnityEngine;

public class RandomEventManager : MonoBehaviour
{
    public GameClock clock; // 게임 시간 참조 (외부에서 연결)

    private float eventTimer = 0f; // 경과 시간 측정용
    public float minEventInterval = 10f; // 이벤트 최소 간격 (초)
    public float maxEventInterval = 30f; // 이벤트 최대 간격 (초)

    private float nextEventTime; // 다음 이벤트가 발생할 시간 (초 단위)

    void Start()
    {
        // 첫 이벤트 예약
        ScheduleNextEvent();
    }

    void Update()
    {
        // 수면 시간(02:00 ~ 05:59)에는 이벤트 발생 금지
        if (clock.currentHour >= 2 && clock.currentHour < 6)
            return;

        // 시간 누적
        eventTimer += Time.deltaTime;

        // 다음 이벤트 시간이 되었는가?
        if (eventTimer >= nextEventTime)
        {
            TriggerRandomEvent();
            ScheduleNextEvent();
        }
    }

    // 다음 이벤트까지의 시간 랜덤 설정
    void ScheduleNextEvent()
    {
        eventTimer = 0f;
        nextEventTime = Random.Range(minEventInterval, maxEventInterval);
    }

    // 랜덤 이벤트 실행
    void TriggerRandomEvent()
    {
        int rand = Random.Range(0, 3);
        switch (rand)
        {
            case 0:
                Debug.Log("나무에서 열매를 발견했습니다.");
                // TODO: 아이템 획득 로직 추가
                break;
            case 1:
                Debug.Log("수상한 NPC가 다가옵니다.");
                // TODO: NPC 등장 및 대화 이벤트 추가
                break;
            case 2:
                Debug.Log("늑대가 접근하고 있습니다!");
                // TODO: 전투 이벤트 또는 회피 선택지 구현
                break;
        }
    }
}
