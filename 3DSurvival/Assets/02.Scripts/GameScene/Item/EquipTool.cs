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
    private Camera _camera;

    void Awake()
    {
        animator = GetComponent<Animator>();
        _camera = Camera.main;
    }

    public override void OnAttackInput()
    {
        if (!attacking && CharacterManager.Instance.Player.status.UseStamina(useStamina))
        {
            //animator.SetTrigger("Attack");
            OnHit();
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
        Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, attackDistance))
        {
            //자원 수집 처리
            if (doesGatherResources && hit.collider.TryGetComponent(out Resource resource))
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