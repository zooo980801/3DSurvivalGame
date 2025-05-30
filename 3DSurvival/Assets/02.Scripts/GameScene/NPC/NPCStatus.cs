using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
    public int level;
    public int requiredExp;
}
public class NPCStatus : BaseStatus
{
    //배고픔, 수분의 증감을 표시하고
    //하루가 끝날때 배고픔, 수분 수치 정도에 따라 경험치를 얻고
    //레벨업을 합니다

    public string npcId = "npc_Seonbi";
    public GameClock clock;

    //저장되어야 하는 수치
    [SerializeField] private int curLevel = 1;//처음 레벨
    [SerializeField] private int curExp = 0;//현재 경험치

    //외부에서 읽기만 가능하게
    public int CurLevel => curLevel;
    public int CurExp => curExp;

    [Header("레벨업 조건표")]
    [SerializeField] private List<LevelData> levelTable;

    void Start()
    {
        // clock이 연결되지 않았다면 씬에서 자동 탐색
        if (clock == null)
            clock = FindObjectOfType<GameClock>();

        // 시간 변경 이벤트 구독
        clock.OnTimeChanged += CheckSleep;
    }

    protected override void Update()
    {
        base.Update();

    }
    // 시간 변경 시 호출되는 콜백 함수
    void CheckSleep(int hour, int minute)
    {
        // 새벽 2시가 되면 하루 종료
        if (hour == 2 && minute == 0)
        {
            GetExp();
        }
        else if(hour == 6 && minute == 1)
        {
            hunger.CurValue = hunger.MaxValue;
            thirst.CurValue = thirst.MaxValue;
        }

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
        //하루가 끝났을 때 기본 호출
        Debug.Log($"현재 레벨은 {curLevel}, 현재 경험치는 {curExp}");

        bool didLevelUp = false;

        // 현재 레벨에 해당하는 조건표를 찾아서 반복 레벨업
        while (true)
        {
            LevelData data = levelTable.Find(x => x.level == curLevel);
            if (data == null)
            {
                Debug.LogWarning($"레벨 {curLevel} 조건표가 없습니다. 레벨업 중단");
                break;
            }

            if (curExp >= data.requiredExp)
            {
                curExp -= data.requiredExp;
                curLevel++;
                didLevelUp = true;
                Debug.Log($"레벨업! 새 레벨: {curLevel}");

                if (curLevel == 2 || curLevel == 5)
                {
                    //레벨에 따른 이벤트 처리
                    Debug.Log($"{curLevel}레벨에서 특별 이벤트 발생!");
                }
            }
            else
            {
                break;
            }
        }

        if (!didLevelUp)
            Debug.Log("레벨업하지 않았습니다.");

        Debug.Log($"최종 상태: 레벨 {curLevel}, 남은 EXP {curExp}");


    }

    public void WriteSaveStatus(SaveData data)
    {
        base.WriteSaveStatus(data); // hunger, thirst 저장
        data.npcs.Add(new SavedNPC
        {
            npcId = npcId,
            curLevel = curLevel,
            curExp = curExp,
            hunger = hunger.ToSaveData(),
            thirst = thirst.ToSaveData()
        });
    }


    public void ApplySave(SavedNPC data)
    {
        curLevel = data.curLevel;
        curExp = data.curExp;
        hunger.FromSaveData(data.hunger);
        thirst.FromSaveData(data.thirst);
    }
}
