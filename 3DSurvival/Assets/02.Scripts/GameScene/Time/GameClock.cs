using System;
using Tenkoku.Core;
using UnityEngine;

public class GameClock : MonoBehaviour
{
    public int currentHour = 6; // 게임 시작 시 시간 (6시)
    public int currentMinute = 0; // 게임 시작 시 분
    public float realSecondsPerGameMinute = 1f / 60f; // 현실 1초에 게임 시간 1분이 흐름 (총 24초에 하루)

    public TenkokuModule tenkokuModule; // Tenkoku 모듈 참조 (Inspector에서 연결하거나 Start에서 자동 탐색)

    public event Action<int, int> OnTimeChanged; // 시간 변경 이벤트 (hour, minute)

    private float timer; // 시간 누적용 변수

    void Start()
    {
        // tenkokuModule이 연결되지 않았을 경우 씬에서 자동으로 탐색
        if (tenkokuModule == null)
            tenkokuModule = FindObjectOfType<TenkokuModule>();
    }

    void Update()
    {
        timer += Time.deltaTime; // 프레임 시간 누적

        if (timer >= realSecondsPerGameMinute) // 1분(가상 시간) 경과했는지 체크
        {
            timer -= realSecondsPerGameMinute; // 누적된 시간에서 1분만큼 차감
            currentMinute++; // 게임 분 증가

            if (currentMinute >= 60)
            {
                currentMinute = 0;
                currentHour = (currentHour + 1) % 24; // 시간이 24를 넘으면 0으로 순환
            }

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
}
