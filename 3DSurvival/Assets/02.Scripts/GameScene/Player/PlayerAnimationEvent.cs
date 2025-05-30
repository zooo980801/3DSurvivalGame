using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    public PlayerAttack playerAttack;

    public void AttackHit()
    {
        if (playerAttack != null)
        {
            playerAttack.OnPunch(); // Player에 있는 함수 호출
        }
    }
}
