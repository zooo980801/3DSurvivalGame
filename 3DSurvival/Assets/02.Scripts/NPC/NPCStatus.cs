using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStatus : BaseStatus
{

    [SerializeField] private StatusData exp;

    [Header("선비 고유 배고픔+수분 소모 속도 배율")]
    [SerializeField] private float hungerDecayMultiplier = 0.5f;  // 배고픔 플레이어보다 천천히 감소
    [SerializeField] private float thirstDecayMultiplier = 0.8f;  // 목마름 플레이어보다 천천히 감소

    protected override void Update()
    {
        hunger.Subtract(hunger.PassiveValue * Time.deltaTime * hungerDecayMultiplier);
        thirst.Subtract(thirst.PassiveValue * Time.deltaTime * thirstDecayMultiplier);
    }

    public void GetExp(float amount)
    {
        //만족 조건이 필요
        //오르는 양이 다름
        //레벨 별 설계 필요
        exp.Add(amount);
    }




}
