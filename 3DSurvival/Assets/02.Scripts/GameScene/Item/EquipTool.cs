using UnityEngine;

public class EquipTool : Equip
{
    private float attackRate;
    private int damage;

    [SerializeField]
    private bool attacking = false;

    public float attackDistance;
    public float useStamina;

    [Header("Resource Gathering")]
    public bool doesGatherResources;

    [Header("Combat")]
    public bool doesDealDamage; 

    public ItemData toolItemData;

    private Animator animator;
    private Camera _camera;

    void Start()
    {
        animator = GetComponent<Animator>();
        _camera = Camera.main;


        if (toolItemData != null)
        {
            attackRate = toolItemData.delay; 
            damage = toolItemData.damage;
        }
        else
        {
            Debug.LogWarning("EquipTool에 toolItemData가 없음", this);
        }
    }

    public override void OnAttackInput()
    {
        _camera = Camera.main;
        if (!attacking)
        {
            if (CharacterManager.Instance.Player.status.UseStamina(useStamina))
            {
                Invoke("OnCanAttack", attackRate);
                //animator.SetTrigger("Attack");ass
                OnHit();
                attacking = true;
            }
        }
    }

    void OnCanAttack()
    {
        Debug.Log("어택 펄스로바꿈");
        attacking = false;
    }

    public void OnHit()
    {
        Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, attackDistance))
        {
            if (doesGatherResources && hit.collider.TryGetComponent(out Resource resource))
            {
                string currentToolId = (toolItemData != null) ? toolItemData.id : "";
                resource.Gather(hit.point, hit.normal, currentToolId);
            }
            if (doesDealDamage && hit.collider.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);
            }
        }
    }
}

public interface IDamageable
{
    void TakeDamage(int amount);
}