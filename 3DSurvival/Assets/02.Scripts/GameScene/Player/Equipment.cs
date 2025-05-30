using UnityEngine;
using UnityEngine.InputSystem;

public class Equipment : MonoBehaviour
{
    public Equip curEquip;
    public Transform equipParent;

    private PlayerController controller;

    void Start()
    {
        controller = GetComponent<PlayerController>();
    }

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
        if (context.phase == InputActionPhase.Performed && curEquip != null && controller.canLook)
        {
            curEquip.OnAttackInput();
        }
    }
}