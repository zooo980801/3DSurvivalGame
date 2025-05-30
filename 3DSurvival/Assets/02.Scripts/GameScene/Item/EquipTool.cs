using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTool : Equip
{
    public float attackRate;
    public bool attacking;
    public float attackDistance;
    public float useStamina;

    [Header("Resource Gathering")]
    public bool doesGatherResources;

    [Header("Combat")]
    public bool doesDealDamage;
    public int damage;

    private Animator animator;
    private Camera camera;

    void Start()
    {
        animator = GetComponent<Animator>();
        camera = Camera.main;
    }

    public override void OnAttackInput()
    {
        if (!attacking)
        {
            if (CharacterManager.Instance.Player.status.UseStamina(useStamina))
            {
                attacking = true;
                // 공격 애니메이션

                StartCoroutine(CanAttack(attackRate));
            }
        }
    }

    private IEnumerator CanAttack(float delay)
    {
        yield return new WaitForSeconds(delay);
        attacking = false;
    }

    public void OnHit()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, attackDistance))
        {
            if (doesGatherResources && hit.collider.TryGetComponent(out Resource resource))
            {
                resource.Gather(hit.point, hit.normal);
            }

            if (doesDealDamage && hit.collider.TryGetComponent(out IDamagable target))
            {
                target.TakePhysicalDamage(damage);
            }
        }
    }
}
