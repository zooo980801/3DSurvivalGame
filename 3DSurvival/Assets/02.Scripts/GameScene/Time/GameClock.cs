using System;
using Tenkoku.Core;
using UnityEngine;

public class GameClock : MonoBehaviour
{
    public ClockValue currentHour = 12;
    public ClockValue currentMinute = 0;
    public ClockValue currentDay = 1;
    public float realSecondsPerGameMinute = 1f / 60f; // 현실 1초에 게임 시간 1분이 흐름 (총 24초에 하루)

    public TenkokuModule tenkokuModule; // Tenkoku 모듈 참조 (Inspector에서 연결하거나 Start에서 자동 탐색)

    public event Action<int, int> OnTimeChanged; // 시간 변경 이벤트 (hour, minute)

    public event Action<int> OnDayChanged; // 날짜 변경 이벤트 (day)

    public event Action OnClockChanged;

    private float timer; // 시간 누적용 변수
    
    private ResourceObj[] resources;
    void Start()
    {
        currentHour.onValueChanged += _ => OnClockChanged?.Invoke();
        currentMinute.onValueChanged += _ => OnClockChanged?.Invoke();
        currentDay.onValueChanged += _ => OnClockChanged?.Invoke();

        if (tenkokuModule == null)
            tenkokuModule = FindObjectOfType<TenkokuModule>();

        //ResourceObj 스크립트가 붙은 오브젝트 탐색
        resources = UnityEngine.Object.FindObjectsByType<ResourceObj>(FindObjectsInactive.Include, FindObjectsSortMode.None);
    }

    void Update()
    {
        timer += Time.deltaTime; // 프레임 시간 누적

        if (timer >= realSecondsPerGameMinute) // 1분(가상 시간) 경과했는지 체크
        {
            timer -= realSecondsPerGameMinute; // 누적된 시간에서 1분만큼 차감
            currentMinute.Value += 1;

            if (currentMinute.Value >= 60)
            {
                currentMinute.Value = 0;
                currentHour.Value = (currentHour.Value + 1) % 24;

                if (currentHour.Value == 0)
                {
                    currentDay.Value += 1;
                    //리소스아이템(나무,돌) 리셋
                    foreach (ResourceObj obj in resources)
                    {
                        if (obj != null)
                        {
                            obj.ResetResource(); // 리셋 실행
                        }
                    }
                    OnDayChanged?.Invoke(currentDay);
                }
            }

            OnTimeChanged?.Invoke(currentHour, currentMinute);


            OnTimeChanged?.Invoke(currentHour, currentMinute); // 시간 변경 이벤트 호출
            UpdateTenkokuTime(); // Tenkoku에 현재 시간 반영
        }
    }

    void UpdateTenkokuTime()
    {
        if (tenkokuModule != null)
        {
            // Tenkoku의 자동 시간 설정을 비활성화하고 수동으로 제어
            tenkokuModule.useAutoTime = false;
            tenkokuModule.autoTimeSync = false;

            // 현재 시간, 분, 초를 Tenkoku에 전달
            tenkokuModule.currentHour = currentHour;
            tenkokuModule.currentMinute = currentMinute;

            // 초는 0~59 범위에서 추정 (timer의 비율을 기반으로 계산)
            tenkokuModule.currentSecond = Mathf.FloorToInt((timer / realSecondsPerGameMinute * 60f) % 60f);

            // Tenkoku 내부 시간 계산 강제 갱신
            tenkokuModule.RecalculateTime();
        }
    }

    public string GetTimeString()
    {
        // 시간 문자열을 2자리 형식(HH:MM)으로 반환
        return $"{currentHour:D2}:{currentMinute:D2}";
    }


    public void WriteSaveClock(SaveData data)
    {
        data.currentHour = currentHour;
        data.currentMinute = currentMinute;
        data.currentDay = currentDay;

        UpdateTenkokuTime(); // Tenkoku 시간 동기화도 반영
    }

    public void ApplySaveClock(SaveData data)
    {
        currentHour.Value = data.currentHour;
        currentMinute.Value = data.currentMinute;
        currentDay.Value = data.currentDay;
    }
}
