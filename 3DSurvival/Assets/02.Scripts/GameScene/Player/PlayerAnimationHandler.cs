using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    private static readonly int IsWalking = Animator.StringToHash("IsWalk");

    private Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Walk(bool isWalk)
    {
        animator.SetBool(IsWalking, isWalk);
    }
}
