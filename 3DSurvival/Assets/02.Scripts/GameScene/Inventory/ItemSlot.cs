using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public ItemData item;
    
    public Button Btn;
    public Image Icon;
    public TextMeshProUGUI quantityText;
    private Outline outline;
    public Inventory inventory;
    
    public int idx;
    public bool equipped;
    public int quantity;
    // Start is called before the first frame update
    void Awake()
    {
        outline = GetComponent<Outline>();
    }
    
    // Update is called once per frame
    // void OnEnable()
    // {
    //     outline.enabled = equipped;
    // }

    
    public void Set()
    {
        Icon.gameObject.SetActive(true);
        Icon.sprite = item.icon;
        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty;

        if (outline != null)
        {
            outline.enabled = equipped;
        }
    }

    public void Clear()
    {
        item = null;
        Icon.gameObject.SetActive(false);
        quantityText.text = string.Empty;
    }

    public void OnClickBtn()//버튼이 눌렸을때(슬롯에 붙는거) 나오는 UI와 임시저장
    {
        InventoryManager.Instance.Inventory.SelectItem(idx);
    }
}
