using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private bool playerAttacking;

    [SerializeField] private Transform equipParent;//장착모션 보여질 카메라
    public Equip curEquip;//현재 장착 아이템
    

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

    // 아이템 장착
    public void EquipNew(ItemData data)
    {
        UnEquip();
        GameObject newEquipObject = Instantiate(data.equipPrefab, equipParent);
        curEquip = newEquipObject.GetComponent<Equip>();

        if (curEquip is EquipTool equipTool)
        {
            equipTool.toolItemData = data;
        }
        Debug.Log(curEquip);
    }

    // 장착 해제
    public void UnEquip()
    {
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
                Attack();//+자원채집
            }
            else
            {
                Debug.Log("장비있슴"+curEquip);
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
