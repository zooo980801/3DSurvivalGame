using System;
using Tenkoku.Core;
using UnityEngine;

public class GameClock : MonoBehaviour
{
    public int currentHour = 6; // 게임 시작 시 6시
    public int currentMinute = 0;
    public float realSecondsPerGameMinute = 1f/60f; // 1초에 1분 흐름 = 60초 → 1시간
    public TenkokuModule tenkokuModule; // TenkokuModule 참조
    public event Action<int, int> OnTimeChanged;

    private float timer;

    void Start()
    {
        if (tenkokuModule == null)
            tenkokuModule = FindObjectOfType<TenkokuModule>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= realSecondsPerGameMinute)
        {
            timer -= realSecondsPerGameMinute;
            currentMinute++;

            if (currentMinute >= 60)
            {
                currentMinute = 0;
                currentHour = (currentHour + 1) % 24;
            }
            OnTimeChanged?.Invoke(currentHour, currentMinute);
            UpdateTenkokuTime();
        }
    }
    void UpdateTenkokuTime()
    {
        if (tenkokuModule != null)
        {
            // Tenkoku 시간 설정
            tenkokuModule.currentHour = currentHour;
            tenkokuModule.currentMinute = currentMinute;
            tenkokuModule.currentSecond = Mathf.FloorToInt((timer / realSecondsPerGameMinute * 60f) % 60f);
            // 시간 압축 비활성화 (직접 제어할 것이므로)
            tenkokuModule.useAutoTime = false;
            tenkokuModule.autoTimeSync = false;

            // Tenkoku 시간 강제 업데이트
            tenkokuModule.RecalculateTime();
        }
    }
    public string GetTimeString()
    {
        return $"{currentHour:D2}:{currentMinute:D2}";
    }
}
