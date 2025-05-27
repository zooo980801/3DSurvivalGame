using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatusData
{
    [SerializeField] private float curValue;        // 현재 상태값
    [SerializeField] private float maxValue;        // 최대 상태값
    [SerializeField] private float passiveValue;    // 기본적으로 적용되는 상태값(자연회복 등)

    public float CurValue { get { return curValue; } set { curValue = value; } }
    public float MaxValue { get { return maxValue; } }
    public float PassiveValue { get { return passiveValue; } }

    public float Percentage => curValue / maxValue;     // UI 표시를 위한 퍼센트

    public void Add(float value)
    {
        curValue = Mathf.Min(curValue + value, maxValue);   // 상태값 회복
    }

    public void Subtract(float value)
    {
        curValue = Mathf.Max(curValue - value, 0f);         // 상태값 감소
    }
}

public class BaseStatus : MonoBehaviour
{
    [SerializeField] protected StatusData hunger;   // 배고픔
    [SerializeField] protected StatusData thirst;   // 수분

    protected virtual void Update()
    {
        hunger.Subtract(hunger.PassiveValue * Time.deltaTime);  // 기본 배고픔 감소
        thirst.Subtract(thirst.PassiveValue * Time.deltaTime);  // 기본 수분 감소
    }

    protected void Eat(float amount)
    {
        hunger.Add(amount);
    }

    protected void GetHunger(float amount)
    {
        hunger.Subtract(amount);
    }

    protected void Drink(float amount)
    {
        thirst.Add(amount);
    }

    protected void GetThirst(float amount)
    {
        thirst.Subtract(amount);
    }
}