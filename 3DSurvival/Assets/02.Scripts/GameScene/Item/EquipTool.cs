using System.Collections;
using UnityEngine;
using static UnityEditor.Progress;

public class EquipTool : Equip
{
    public float attackRate;
    private bool attacking;
    public float attackDistance;
    public float useStamina;

    [Header("Resource Gathering")]
    public bool doesGatherResources;

    [Header("Combat")]
    public bool doesDealDamage;
    public int damage;

    public ItemData toolItemData;

    private Animator animator;
    PlayerAttack playerAttack;
    void Awake()
    {
        animator = GetComponent<Animator>();
        playerAttack = CharacterManager.Instance.Player.GetComponent<PlayerAttack>();
    }

    public override void OnAttackInput()
    {
        if (!attacking && CharacterManager.Instance.Player.status.UseStamina(useStamina))
        {
            if (toolItemData.id == "hand")
            {
                CharacterManager.Instance.Player.animationHandler.Punch();
            }
            else
            {
                animator.SetTrigger("Attack");
                OnHit();
            }
            StartCoroutine(CanAttackDelay());
            attacking = true;
        }
    }
    private IEnumerator CanAttackDelay()
    {
        yield return new WaitForSeconds(attackRate);
        attacking = false;
    }

    public void OnHit()
    {
        CharacterManager.Instance.Player.soundHandler.AttackGruntSound();

        Transform cameraContainer = CharacterManager.Instance.Player.controller.CameraContainer;
        Ray ray = new Ray(cameraContainer.position, cameraContainer.forward);
        RaycastHit hit;

        if (toolItemData.type == ITEMTYPE.BUILDING) // 빌딩 타입이면
        {
            //설치 할 수 있는지 확인
            if (Physics.Raycast(ray, attackDistance))
            {
                return;
            }
            //설치
            Vector3 placePos = cameraContainer.position + cameraContainer.forward * attackDistance;
            Instantiate(toolItemData.dropPrefab, placePos, Quaternion.identity);
            //핸드 비우기
            playerAttack.UnEquip();
        }
        else if (Physics.Raycast(ray, out hit, attackDistance))
        {
            //자원 수집 처리
            if (doesGatherResources && hit.collider.TryGetComponent(out ResourceObj resource))
            {
                string currentToolId = (toolItemData != null) ? toolItemData.id : "";
                resource.Gather(hit.point, hit.normal, currentToolId);
            }

            //적 공격 처리
            if (doesDealDamage && hit.collider.TryGetComponent(out IDamagable target))
            {
                target.TakePhysicalDamage(damage);
            }
        }
    }
}