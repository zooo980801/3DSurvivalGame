using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    [SerializeField] private PlayerStatus playerStatus; // 플레이어 상태

    // 플레이어 상태 UI
    [SerializeField] private StatusUI playerHealthUI;
    [SerializeField] private StatusUI playerStaminaUI;
    [SerializeField] private StatusUI playerHungerUI;
    [SerializeField] private StatusUI playerThirstUI;

    [Header("시간 표시")]
    [SerializeField] private GameClock gameClock;   // GameClock 연결
    [SerializeField] private TextMeshProUGUI timeText;         // 시간 표시용 UI 텍스트 (TextMeshPro 사용 시 TMPro.TextMeshProUGUI)

    private void Start()
    {
        // 플레이어 상태 데이터를 각 UI에 연결
        playerHealthUI.Bind(playerStatus.Health);
        playerStaminaUI.Bind(playerStatus.Stamina);
        playerHungerUI.Bind(playerStatus.Hunger);
        playerThirstUI.Bind(playerStatus.Thirst);

        gameClock.OnTimeChanged += UpdateTimeUI;

        // 시작 시간도 바로 반영
        UpdateTimeUI(gameClock.currentHour, gameClock.currentMinute);
    }

    private void UpdateTimeUI(int hour, int minute)
    {
        if (timeText != null)
            timeText.text = $"{hour:D2}:{minute:D2}";
    }
}
