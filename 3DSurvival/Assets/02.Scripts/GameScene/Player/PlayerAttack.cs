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

    private Camera camera;

    void Start()
    {
        controller = GetComponent<PlayerController>();
        status = GetComponent<PlayerStatus>();
        animationHandler = GetComponent<PlayerAnimationHandler>();

        camera = Camera.main;
    }

    public void EquipNew(ItemData data)
    {
        UnEquip();
        // 장비 장착

    }

    public void UnEquip()
    {
        // 장비 해제
        if (curEquip != null)
        {
            Destroy(curEquip.gameObject);
            curEquip = null;
        }
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed &&controller.canLook)
        {
            if (curEquip == null)
            {
                // 장비가 없을 경우
                Attack();
            }
            else
            {
                // 장비가 있을 경우
                curEquip.OnAttackInput();
            }
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
        Ray ray;

        if (controller.isFirstPerson)
        {
            ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        }
        else
        {
            ray = new Ray(controller.CameraContainer.position, camera.transform.forward);
        }

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
