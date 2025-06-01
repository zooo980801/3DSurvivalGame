using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private bool playerAttacking;

    [SerializeField] private Transform equipParent;

    private Equip curEquip;

    private PlayerController controller;
    private PlayerStatus status;
    private PlayerAnimationHandler animationHandler;
    private PlayerSoundHandler soundHandler;

    private Camera camera;
    private Equip hand;
    void Start()
    {
        controller = GetComponent<PlayerController>();
        status = GetComponent<PlayerStatus>();
        animationHandler = GetComponent<PlayerAnimationHandler>();
        soundHandler = GetComponent<PlayerSoundHandler>();
        camera = Camera.main;

        hand = Instantiate(CharacterManager.Instance.Player.hand.equipPrefab, equipParent).GetComponent<Equip>();
        curEquip = hand;
    }

    public void EquipNew(ItemData data)
    {
        UnEquip();

        curEquip = Instantiate(data.equipPrefab, equipParent).GetComponent<Equip>();

        //if (curEquip is EquipTool tool)
        //    tool.toolItemData = data;
        //// 장비 장착

    }

    public void UnEquip()
    {
        // 장비 해제
        if (curEquip != hand)
        {
            Destroy(curEquip.gameObject);
            curEquip = hand;
        }
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed &&controller.canLook)
        {
            curEquip.OnAttackInput();
            //if (curEquip == null)
            //{
            //    // 장비가 없을 경우
            //    Attack();
            //}
            //else
            //{
            //    // 장비가 있을 경우
            //    curEquip.OnAttackInput();
            //}
        }
    }

    #region 플레이어 맨손 공격
    void Attack()
    {
        if (!playerAttacking)
        {
            if (status.UseStamina(status.AttackStamina))
            {
                playerAttacking = true;
                animationHandler.Punch();
                StartCoroutine(CanAttack(status.AttackRate));
            }
        }
    }

    private IEnumerator CanAttack(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerAttacking = false;
    }

    public void OnPunch()
    {
        soundHandler.AttackGruntSound();
        soundHandler.PunchSound();

        Ray ray = new Ray(controller.CameraContainer.position, camera.transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, status.AttackDistance))
        {
            if (hit.collider.TryGetComponent(out IDamagable target))
            {
                target.TakePhysicalDamage(status.AttackPower);
            }
        }
    }
    #endregion
}
