using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStatus : BaseStatus
{

    [SerializeField] private int curLevel = 1;//처음 레벨
    [SerializeField] private int curExp = 0;//현재 경험치

    protected override void Update()
    {
        base.Update();
    }

    public void GetExp()
    {
        //하루가 끝날 때 경험치 정산
        
        int expGain = 0;
        if(hunger.CurValue >= 80 && thirst.CurValue >= 80) //수분이 80 이상 && 배고픔이 80 이상이면 exp += 100
        {
            expGain = 100;
        }
        else if(hunger.CurValue >= 80 ||  thirst.CurValue >= 80)//둘 중 하나라도 80미만이면 exp += 50
        {
            expGain = 50;
        }
        else//둘 다 80미만이면 exp += 30
        {
            expGain = 30;
        }
        curExp += expGain;
        LevelUp();

    }

    public void LevelUp()
    {
        //레벨업이 됐을 때 호출해야함 
        Debug.Log($"레벨업 : 현재 레벨은 {curLevel}");

        if(curLevel == 2 || curLevel == 5)
        {
            //레벨에 따른 이벤트 처리
            Debug.Log($"{curLevel}레벨에서 특별 이벤트 발생!");
        }
    }
}
