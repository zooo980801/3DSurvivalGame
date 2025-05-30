using System;
using UnityEngine;

[Serializable]
public class StatusData
{
    [SerializeField] private float curValue;
    [SerializeField] private float maxValue;
    [SerializeField] private float passiveValue;

    public event Action<float> onUIChanged;

    public float CurValue 
    { 
        get => curValue; 
        set 
        { 
            curValue = Mathf.Clamp(value, 0, maxValue);
            onValueChanged?.Invoke();
            onUIChanged?.Invoke(Percentage);
        } 
    }
    public float MaxValue => maxValue;
    public float PassiveValue => passiveValue;

    public float Percentage => curValue / maxValue;

    public event Action onValueChanged;

    public void Add(float value) => CurValue += value;
    public void Subtract(float value) => CurValue -= value;

    public SaveStatusData ToSaveData()
    {
        return new SaveStatusData
        {
            curValue = curValue,
            maxValue = maxValue,
            passiveValue = passiveValue
        };
    }
    public void FromSaveData(SaveStatusData data)
    {
        maxValue = data.maxValue;
        passiveValue = data.passiveValue;
        curValue = Mathf.Clamp(data.curValue, 0, maxValue);
        onValueChanged?.Invoke();
        onUIChanged?.Invoke(Percentage);
    }


}

public class BaseStatus : MonoBehaviour
{
    [SerializeField] protected StatusData hunger;
    [SerializeField] protected StatusData thirst;

    public StatusData Hunger { get { return hunger; } }
    public StatusData Thirst { get { return thirst; } }

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