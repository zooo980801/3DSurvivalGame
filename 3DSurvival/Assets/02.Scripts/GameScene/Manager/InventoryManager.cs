using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    #region

    //===================================[싱글톤]=====================================//
    private static InventoryManager _instance;

    public static InventoryManager Instance //프로퍼티(겟터)
    {
        get
        {
            if (_instance == null) //인스턴스가없으면 게임오브젝트를 생성
            {
                _instance = FindObjectOfType<InventoryManager>();
            }

            return _instance;
        }
        set { _instance = value; }
    }

    private void Awake()
    {
        InventoryBG.SetActive(true);
        SeonbiBG.SetActive(true);
        CraftingBG.SetActive(true);
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        InventoryBG.SetActive(false);
        SeonbiBG.SetActive(false);
        CraftingBG.SetActive(false);
    }
    
//========================================================================//

    #endregion

    private Inventory _inventory;

    public Inventory Inventory
    {
        get { return _inventory; }
        set { _inventory = value; }
    }

    private ItemData _itemData;

    public ItemData ItemData
    {
        get { return _itemData; }
        set { _itemData = value; }
    }

    public GameObject InventoryBG;
    public GameObject SeonbiBG;
    public GameObject CraftingBG;
}