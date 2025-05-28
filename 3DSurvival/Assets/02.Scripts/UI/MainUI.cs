using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    [SerializeField] private PlayerStatus playerStatus; // 플레이어 상태

    // 플레이어 상태 UI
    [SerializeField] private StatusUI playerHealthUI;
    [SerializeField] private StatusUI playerStaminaUI;
    [SerializeField] private StatusUI playerHungerUI;
    [SerializeField] private StatusUI playerThirstUI;

    private void Start()
    {
        // 플레이어 상태 데이터를 각 UI에 연결
        playerHealthUI.Bind(playerStatus.Health);
        playerStaminaUI.Bind(playerStatus.Stamina);
        playerHungerUI.Bind(playerStatus.Hunger);
        playerThirstUI.Bind(playerStatus.Thirst);
    }
}
