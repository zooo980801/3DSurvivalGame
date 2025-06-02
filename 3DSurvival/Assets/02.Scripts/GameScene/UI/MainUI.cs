using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{

    // 플레이어 상태 UI
    [Header("플레이어 상태UI")]
    [SerializeField] private PlayerStatus playerStatus; // 플레이어 상태
    [SerializeField] private StatusUI playerHealthUI;
    [SerializeField] private StatusUI playerStaminaUI;
    [SerializeField] private StatusUI playerHungerUI;
    [SerializeField] private StatusUI playerThirstUI;

    //NPC 상태 UI
    [Header("NPC 상태UI")]
    [SerializeField] private NPCStatus npcStatus;
    [SerializeField] private StatusUI npcHungerUI;
    [SerializeField] private StatusUI npcThirstUI;
    [SerializeField] private TextMeshProUGUI levelExp;

    [Header("시간 표시")]
    [SerializeField] private GameClock gameClock;   // GameClock 연결
    [SerializeField] private GameClock gameDay;   // GameClock 연결
    [SerializeField] private TextMeshProUGUI timeText;         // 시간 표시용 UI 텍스트 (TextMeshPro 사용 시 TMPro.TextMeshProUGUI)
    [SerializeField] private TextMeshProUGUI dayText;         //  날짜 표시용 UI 텍스트 (TextMeshPro 사용 시 TMPro.TextMeshProUGUI)


    [SerializeField] private AlarmUI alarmUI;

    private float lastHungerAlarmTime = -5f;
    private float lastThirstAlarmTime = -5f;
    private const float ALARM_INTERVAL = 5f; // 5초 간격

    private void Start()
    {
        // 플레이어 상태 데이터를 각 UI에 연결
        playerHealthUI.Bind(playerStatus.Health);
        playerStaminaUI.Bind(playerStatus.Stamina);
        playerHungerUI.Bind(playerStatus.Hunger);
        playerThirstUI.Bind(playerStatus.Thirst);

        //NPC 상태 데이터를 각 UI에 연결
        npcHungerUI.Bind(npcStatus.Hunger);
        npcThirstUI.Bind(npcStatus.Thirst);
        UpdateLevel();

        gameClock.OnTimeChanged += UpdateTimeUI;
        gameClock.OnDayChanged += UpdateDayUI;

        // 시작 시간도 바로 반영
        UpdateTimeUI(gameClock.currentHour, gameClock.currentMinute);
        UpdateDayUI(gameClock.currentDay);
    }

    private void Update()
    {
        float currentTime = Time.time;
        float hungerRatio = npcStatus.Hunger.CurValue / npcStatus.Hunger.MaxValue;
        float thirstRatio = npcStatus.Thirst.CurValue / npcStatus.Thirst.MaxValue;

        bool isHungry = hungerRatio < 0.3f;
        bool isThirsty = thirstRatio < 0.3f;

        if ((isHungry || isThirsty) && currentTime - lastHungerAlarmTime >= ALARM_INTERVAL)
        {
            string message = "";

            if (isHungry && isThirsty)
                message = "선비가 배고프고 목말라합니다!";
            else if (isHungry)
                message = "선비가 배고픕니다!";
            else if (isThirsty)
                message = "선비가 목말라합니다!";

            if (!string.IsNullOrEmpty(message))
            {
                alarmUI.Show(message);
                lastHungerAlarmTime = currentTime;
                lastThirstAlarmTime = currentTime; // 두 상태 모두 시간 갱신
            }
        }
    }
    private void UpdateTimeUI(int hour, int minute)
    {
        if (timeText != null)
            timeText.text = $"{hour:D2}:{minute:D2}";
    }
    private void UpdateDayUI(int day)
    {
        if (dayText != null)
            dayText.text = $"Day {day}";
    }
    private void UpdateLevel()
    {
        levelExp.text = $"레벨 : {npcStatus.CurLevel} 경험치 : {npcStatus.CurExp}";
    }
}
