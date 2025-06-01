using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    public EquipTool punchAttack;

    public void AttackHit()
    {
        if (punchAttack != null)
        {
            punchAttack.OnHit();
        }
    }
}
