using System.Collections;
using UnityEngine;

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
            else if (toolItemData.type == ITEMTYPE.BUILDING)
            {
                OnBuild();
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

    public void OnBuild()
    {
        CharacterManager.Instance.Player.soundHandler.AttackGruntSound();

        Transform cameraContainer = CharacterManager.Instance.Player.controller.CameraContainer;
        Ray ray = new Ray(cameraContainer.position, cameraContainer.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, attackDistance))
        {
            //설치 불가능 일때 효과 넣기
            return;
        }
        //설치
        Vector3 placePos = cameraContainer.position + cameraContainer.forward * attackDistance;
        Instantiate(toolItemData.dropPrefab, placePos, Quaternion.identity);
        //핸드 비우기
        playerAttack.UnEquip();
        //장비 장착 창 비우기
        InventoryManager.Instance.Inventory.InventoryUI.CraftingUnEquip();
    }
    public void OnHit()
    {
        CharacterManager.Instance.Player.soundHandler.AttackGruntSound();

        Transform cameraContainer = CharacterManager.Instance.Player.controller.CameraContainer;
        Ray ray = new Ray(cameraContainer.position, cameraContainer.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, attackDistance))
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