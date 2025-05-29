using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerStatus status;
    public PlayerAnimationHandler animationHandler;
    public PlayerAttack equip;

    public ItemData itemData;
    public Action addItem;

    public Transform dropPosition;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        status = GetComponent<PlayerStatus>();
        animationHandler = GetComponent<PlayerAnimationHandler>();
        equip = GetComponent<PlayerAttack>();
    }
}
