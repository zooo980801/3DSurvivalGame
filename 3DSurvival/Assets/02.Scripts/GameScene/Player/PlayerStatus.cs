using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable // 데미지를 받는 오브젝트가 구현해야 할 인터페이스
{
    void TakePhysicalDamage(int damage);    // 데미지를 받는 함수
}

public class PlayerStatus : BaseStatus, IDamagable
{
    [SerializeField] private StatusData health;
    [SerializeField] private StatusData stamina;

    [Header("Attack")]
    [SerializeField] private int attackPower;
    [SerializeField] private float attackDistance;
    [SerializeField] private float attackRate;
    [SerializeField] private float attackStamina;

    public StatusData Health => health;
    public StatusData Stamina => stamina;
    public int AttackPower => attackPower;
    public float AttackDistance => attackDistance;
    public float AttackRate => attackRate;
    public float AttackStamina => attackStamina;


    public event Action onTakeDamage;


    protected override void Update()
    {
        base.Update();

        if ((hunger.CurValue <= 0f || thirst.CurValue <= 0f) && health.CurValue > 0f)
        {
            health.Subtract(1f * Time.deltaTime);
        }
        else
        {
            health.Add(health.PassiveValue * Time.deltaTime);
        }

        stamina.Add(stamina.PassiveValue * Time.deltaTime);

        if (health.CurValue <= 0f)
        {
            Debug.Log("[PlayerStatus] 체력이 0이 되어 게임 오버 처리됩니다.");
            GameManager.Instance.GameOver();
            enabled = false; // 플레이어 상태 업데이트 중지 (옵션)
        }

    }
    void Awake()
    {
        if (health == null) health = new StatusData();
        if (stamina == null) stamina = new StatusData();
        if (hunger == null) hunger = new StatusData();
        if (thirst == null) thirst = new StatusData();
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void GetStamina(float amount)
    {
        stamina.Add(amount);
    }

    public void TakePhysicalDamage(int damage)
    {
        health.Subtract(damage);
        CharacterManager.Instance.Player.soundHandler.DamageSound();
        onTakeDamage?.Invoke();     // 데미지를 받았다는 이벤트 발생
    }

    public bool UseStamina(float amount)
    {
        if (stamina.CurValue - amount < 0f)
        {
            return false;
        }

        stamina.Subtract(amount);
        return true;
    }
    public void ApplySaveStatus(SaveData data)
    {
        base.ApplySaveStatus(data);
        hunger.FromSaveData(data.playerHunger);
        thirst.FromSaveData(data.playerThirst);
        health.FromSaveData(data.health);
        stamina.FromSaveData(data.stamina);
    }

    public void WriteSaveStatus(SaveData data)
    {

        Debug.Log("[WriteSaveStatus] called");
        base.WriteSaveStatus(data);
        data.playerHunger = hunger.ToSaveData();
        data.playerThirst = thirst.ToSaveData();
        data.health = health.ToSaveData();
        data.stamina = stamina.ToSaveData();
    }

}
