using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable // 데미지를 받는 오브젝트가 구현해야 할 인터페이스
{
    void TakePhysicalDamage(int damage);    // 데미지를 받는 함수
}

public class PlayerStatus : BaseStatus
{
    [SerializeField] private StatusData health;
    [SerializeField] private StatusData stamina;

    public event Action onTakeDamage;

    protected override void Update()
    {
        base.Update();

        health.Add(health.PassiveValue * Time.deltaTime);  // 기본 체력 증가
        stamina.Add(stamina.PassiveValue * Time.deltaTime);  // 기본 스테미나 증가
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void GetStamina(float amount)
    {
        stamina.Add(amount); // 입력 값만큼 스테미나 회복
    }

    public void TakePhysicalDamage(int damage)
    {
        health.Subtract(damage);    // 입력 값만큼 체력 감소
        onTakeDamage?.Invoke();     // 데미지를 받았다는 이벤트 발생
    }

    public bool UseStamina(float amount)
    {
        if (stamina.CurValue - amount < 0f) // 스테미나가 부족할 경우
        {
            return false;
        }

        stamina.Subtract(amount);   // 입력 값만큼 스테미나 소모
        return true;
    }
}
