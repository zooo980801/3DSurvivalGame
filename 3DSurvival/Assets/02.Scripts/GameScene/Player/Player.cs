using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerStatus status;
    public PlayerAnimationHandler animationHandler;
    public PlayerSoundHandler soundHandler;
    public PlayerAttack equip;

    public ItemData itemData;
    public ItemData hand;
    public Action addItem;

    public Transform dropPosition;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        status = GetComponent<PlayerStatus>();
        animationHandler = GetComponent<PlayerAnimationHandler>();
        soundHandler = GetComponent<PlayerSoundHandler>();
        equip = GetComponent<PlayerAttack>();
    }
}
