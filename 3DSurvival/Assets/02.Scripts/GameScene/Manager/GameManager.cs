using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameClock clock;                     // GameClock 컴포넌트 참조 (Inspector에서 연결)
    public PlayerStatus playerStatus;
    public SleepManager sleepManager;           // SleepManager 컴포넌트 참조 (Inspector에서 연결)
    private RandomEventManager eventManager;    // 코드로 추가할 RandomEventManager 컴포넌트

    public GameObject gameOverUI;
    public GameObject clearUI;

    public UIFader Fader;

    public House[] houses; // Inspector에서 3개 할당
    private int destroyedHouseCount = 0;

    private bool isDirty = false;
    private float lastSaveTime;
    private float saveInterval = 2f;

    void Start()
    {
        // 인벤토리 매니저 초기화 대기
        StartCoroutine(InitializeGame());
    }

    IEnumerator InitializeGame()
    {
        // 필수 시스템 초기화 대기
        yield return new WaitUntil(() => InventoryManager.Instance != null);
        yield return new WaitUntil(() => SaveManager.Instance != null);

        if (!SaveManager.IsNewGame)
        {
            Debug.Log("[GameManager] 저장된 데이터를 적용합니다.");

            // 명시적으로 데이터 로드
            var data = SaveManager.Instance.LoadData();

            if (data != null)
            {
                Debug.Log($"인벤토리 아이템 수: {data.inventoryItems?.Count ?? 0}");

                playerStatus.ApplySaveStatus(data);
                clock.ApplySaveClock(data);

                playerStatus.transform.position = new Vector3(
                    data.playerPosX,
                    data.playerPosY,
                    data.playerPosZ
                );
                Vector3 euler = playerStatus.transform.eulerAngles;
                playerStatus.transform.rotation = Quaternion.Euler(0, data.playerRotY, 0);
                PlayerController pc = playerStatus.GetComponent<PlayerController>();
                if (pc != null)
                    pc.SetCamXRot(data.cameraRotX);
                foreach (var npc in FindObjectsOfType<NPCStatus>())
                {
                    var saved = data.npcs.Find(n => n.npcId == npc.npcId);
                    if (saved != null)
                    {
                        npc.ApplySave(saved);
                        Debug.Log($"NPC {npc.npcId} 상태 로드 완료: 레벨 {saved.curLevel}, EXP {saved.curExp}");
                    }
                    else
                    {
                        Debug.LogWarning($"NPC {npc.npcId}에 대한 저장 데이터를 찾지 못했습니다.");
                    }
                }


                foreach (var savedHouse in data.houses)
                {
                    GameObject prefab = Resources.Load<GameObject>($"Prefabs/{savedHouse.prefabId}");
                    if (prefab == null)
                    {
                        Debug.LogWarning($"House 프리팹 {savedHouse.prefabId} 를 찾을 수 없습니다.");
                        continue;
                    }

                    GameObject go = Instantiate(prefab);
                    House house = go.GetComponent<House>();
                    if (house != null)
                    {
                        house.LoadFromSave(savedHouse);
                    }
                    else
                    {
                        Debug.LogWarning("프리팹에 House 컴포넌트가 없습니다.");
                    }
                }// 인벤토리 로드

                foreach (var dropped in data.droppedItems)
                {
                    ItemData itemData = ItemDatabase.Instance.GetItemById(dropped.itemId);
                    if (itemData == null)
                    {
                        Debug.LogWarning($"아이템 ID '{dropped.itemId}' 에 해당하는 ItemData를 찾을 수 없습니다.");
                        continue;
                    }

                    GameObject prefab = itemData.dropPrefab; // ← 이게 반드시 설정돼 있어야 함!
                    if (prefab == null)
                    {
                        Debug.LogWarning($"dropPrefab이 ItemData '{itemData.id}' 에 설정되어 있지 않습니다.");
                        continue;
                    }

                    GameObject itemObj = Instantiate(prefab);
                    itemObj.transform.position = new Vector3(dropped.posX, dropped.posY, dropped.posZ);
                    itemObj.transform.rotation = Quaternion.Euler(dropped.rotX, dropped.rotY, dropped.rotZ);

                    var itemComponent = itemObj.GetComponent<ItemObject>();
                    if (itemComponent != null)
                    {
                        itemComponent.data = itemData;
                        itemComponent.quantity = dropped.amount;
                    }
                }
                InventoryManager.Instance.Inventory.LoadInventory(data);
            }
            else
            {
                Debug.LogWarning("[GameManager] 저장된 데이터가 없음! 새 게임으로 시작합니다.");
                SaveManager.IsNewGame = true;
            }
        }
        else
        {
            Debug.Log("[GameManager] 새 게임 시작 - 초기 상태 유지");
            SaveManager.Instance.CreateNewGameData();
        }

        // 나머지 초기화 코드...
        sleepManager.clock = clock;
        eventManager = gameObject.AddComponent<RandomEventManager>();
        eventManager.clock = clock;

        clock.OnDayChanged += CheckGameClearCondition;
        clock.OnClockChanged += () => isDirty = true;
        playerStatus.Health.onValueChanged += () => isDirty = true;
        playerStatus.Stamina.onValueChanged += () => isDirty = true;
        playerStatus.Hunger.onValueChanged += () => isDirty = true;
        playerStatus.Thirst.onValueChanged += () => isDirty = true;
    }

    void Update()
    {
        if (isDirty && Time.time - lastSaveTime >= saveInterval)
        {
            SaveAll();
            isDirty = false;
            lastSaveTime = Time.time;
        }
    }
    public void SaveAll()
    {
        SaveData data = new SaveData();
        Debug.Log("[SaveAll] Called");

        if (playerStatus != null)
        {
            Debug.Log("[SaveAll] playerStatus not null");
            playerStatus.WriteSaveStatus(data);
            Vector3 pos = playerStatus.transform.position;
            data.playerPosX = pos.x;
            data.playerPosY = pos.y;
            data.playerPosZ = pos.z;
            data.playerRotY = playerStatus.transform.eulerAngles.y; 
            PlayerController pc = playerStatus.GetComponent<PlayerController>();
            if (pc != null)
                data.cameraRotX = pc.GetCamXRot();
        }
        else
        {
            Debug.LogWarning("[SaveAll] playerStatus is NULL!!");
        }

        if (clock != null)
            clock.WriteSaveClock(data);

        InventoryManager.Instance.Inventory.SaveInventory(data);

        data.npcs.Clear(); // 중복 방지
        foreach (var npc in FindObjectsOfType<NPCStatus>())
        {
            npc.WriteSaveStatus(data);
        }
        data.houses.Clear();
        foreach (var house in FindObjectsOfType<House>())
        {
            SavedHouse saved = new SavedHouse();
            house.WriteSave(saved);
            data.houses.Add(saved);
        }
        data.droppedItems.Clear();

        foreach (var itemObj in FindObjectsOfType<ItemObject>())
        {
            data.droppedItems.Add(itemObj.WriteSave());
        }

        SaveManager.Instance.SaveData(data);
    }
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void CheckGameClearCondition(int day)
    {
        if (day >= 4) // 3일 후 → 4일차 진입
        {
            ClearGame();
        }
    }

    public void ClearGame()
    {
        if (gameOverUI != null)
            gameOverUI.SetActive(true);
        Debug.Log("유배 종료, 클리어!");

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Fader.FadeIn();
    }


    public void GameOver()
    {

        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        Debug.Log("게임오버");

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Fader.FadeIn();

    }


    //테스트용 게임오버 버튼 (UI에서 연결)
    public void TestGameOver()
    {
        GameOver();
    }

    public void NotifyHouseDestroyed(House house)
    {
        destroyedHouseCount++;
        Debug.Log($"파괴된 집 수: {destroyedHouseCount}");

        AlarmUI alarm = FindObjectOfType<AlarmUI>();
        if (alarm != null)
            alarm.Show("집이 파괴되었습니다!");

        if (destroyedHouseCount >= houses.Length)
        {
            GameOver();
        }
    }
}
