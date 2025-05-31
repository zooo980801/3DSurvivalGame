using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    #region
    //===================================[싱글톤]=====================================//
    private static InventoryManager _instance;
    public static InventoryManager Instance//프로퍼티(겟터)
    {
        get
        {
            if (_instance == null)//인스턴스가없으면 게임오브젝트를 생성
            {
                _instance = new GameObject("InventoryManager").AddComponent<InventoryManager>();
            }
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }
    private void Awake()
    {
    
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
        
    }
//========================================================================//
    #endregion
    
    private Inventory _inventory;
    public Inventory Inventory{get{return _inventory;}set{_inventory = value;}}
    
    private PlayerStatus _playerStatus;
    public PlayerStatus PlayerStatus{get{return _playerStatus;}set{_playerStatus=value;}}
    
    private ItemData _itemData;
    public ItemData ItemData{get{return _itemData;}set{_itemData=value;}}

}
