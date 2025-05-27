using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStatus : BaseStatus
{

    [SerializeField] private StatusData exp;

    protected override void Update()
    {
        base.Update();
    }

    public void GetExp(float amount)
    {
        //만족 조건이 필요
        //오르는 양이 다름
        //레벨 별 설계 필요
        exp.Add(amount);
    }

}
