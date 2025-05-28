using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    private static readonly int IsWalking = Animator.StringToHash("IsWalk");
    private static readonly int IsRunning = Animator.StringToHash("IsRun");
    private static readonly int IsJumping = Animator.StringToHash("IsJump");

    private Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Walk(bool isWalk)
    {
        animator.SetBool(IsWalking, isWalk);
    }

    public void Run(bool isRun)
    {
        animator.SetBool(IsRunning, isRun);
    }

    public void Jump(bool isJump)
    {
        animator.SetBool(IsJumping, isJump);
    }
}
