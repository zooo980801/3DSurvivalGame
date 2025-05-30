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

    [Header("시간 표시")]
    [SerializeField] private GameClock gameClock;   // GameClock 연결
    [SerializeField] private GameClock gameDay;   // GameClock 연결
    [SerializeField] private TextMeshProUGUI timeText;         // 시간 표시용 UI 텍스트 (TextMeshPro 사용 시 TMPro.TextMeshProUGUI)
    [SerializeField] private TextMeshProUGUI dayText;         //  날짜 표시용 UI 텍스트 (TextMeshPro 사용 시 TMPro.TextMeshProUGUI)

    
    [SerializeField] private AlarmUI alarmUI;

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

        gameClock.OnTimeChanged += UpdateTimeUI;
        gameClock.OnDayChanged += UpdateDayUI;

        // 시작 시간도 바로 반영
        UpdateTimeUI(gameClock.currentHour, gameClock.currentMinute);
        UpdateDayUI(gameClock.currentDay);
    }

    private void Update()
    {
        // 선비 상태 경고
        if (npcStatus.Hunger.CurValue / npcStatus.Hunger.MaxValue < 0.3f)
        alarmUI.Show("선비가 배고픕니다!");

        if (npcStatus.Thirst.CurValue / npcStatus.Thirst.MaxValue < 0.3f)
            alarmUI.Show("선비가 목말라합니다!");
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
}
