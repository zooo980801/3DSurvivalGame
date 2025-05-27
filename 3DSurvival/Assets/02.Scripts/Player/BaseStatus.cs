using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatusData
{
    [SerializeField] private float curValue;
    [SerializeField] private float maxValue;
    [SerializeField] private float passiveValue;

    public float CurValue { get => curValue; set { curValue = Mathf.Clamp(value, 0, maxValue); onValueChanged?.Invoke(); } }
    public float MaxValue => maxValue;
    public float PassiveValue => passiveValue;

    public float Percentage => curValue / maxValue;

    public event Action onValueChanged;

    public void Add(float value) => CurValue += value;
    public void Subtract(float value) => CurValue -= value;

    public SaveStatusData ToSaveData() => new SaveStatusData { curValue = curValue, maxValue = maxValue, passiveValue = passiveValue };
    public void FromSaveData(SaveStatusData data) => curValue = data.curValue; // maxValue/passiveValue는 초기값 유지

}

public class BaseStatus : MonoBehaviour
{
    [SerializeField] protected StatusData hunger;
    [SerializeField] protected StatusData thirst;

    protected virtual void Update()
    {
        hunger.Subtract(hunger.PassiveValue * Time.deltaTime);
        thirst.Subtract(thirst.PassiveValue * Time.deltaTime);
    }

    protected void Eat(float amount) => hunger.Add(amount);
    protected void GetHunger(float amount) => hunger.Subtract(amount);
    protected void Drink(float amount) => thirst.Add(amount);
    protected void GetThirst(float amount) => thirst.Subtract(amount);

    public void ApplySaveStatus(SaveData data)
    {
        hunger.FromSaveData(data.hunger);
        thirst.FromSaveData(data.thirst);
    }

    public void WriteSaveStatus(SaveData data)
    {
        data.hunger = hunger.ToSaveData();
        data.thirst = thirst.ToSaveData();
    }
}