using UnityEngine;

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
        if (!SaveManager.IsNewGame)
        {
            Debug.Log("[GameManager] 저장된 데이터를 적용합니다.");
            var data = SaveManager.Instance.CurrentData;

            if (data != null)
            {
                playerStatus.ApplySaveStatus(data);
                clock.ApplySaveClock(data);

                playerStatus.transform.position = new Vector3(
                data.playerPosX,
                data.playerPosY,
                data.playerPosZ
                );
            }
            else
            {
                Debug.LogWarning("[GameManager] 저장된 데이터가 없음!");
            }
        }
        else
        {
            Debug.Log("[GameManager] 새 게임 시작 - 초기 상태 유지");
        }
        // SleepManager에 GameClock 연결
        sleepManager.clock = clock;

        // RandomEventManager를 동적으로 추가하고 GameClock 연결
        eventManager = gameObject.AddComponent<RandomEventManager>();
        eventManager.clock = clock;

        // 날짜 변경 이벤트 연결
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
        }
        else
        {
            Debug.LogWarning("[SaveAll] playerStatus is NULL!!");
        }

        if (clock != null)
            clock.WriteSaveClock(data);

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
        Debug.Log("유배 종료, 클리어!");
        Fader.FadeIn();  // 페이드 인으로 보여주기
        if (clearUI != null)
            clearUI.SetActive(true);  // 클리어 UI 활성화
    }

    public void GameOver()
    {
        Debug.Log("게임오버");
        Fader.FadeIn();  // 페이드 인으로 보여주기
        // 조건추가 바람.
        if(gameOverUI != null)
            gameOverUI.SetActive(true);  //게임오버 UI 활성화
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

        if (destroyedHouseCount >= houses.Length)
        {
            GameOver();
        }
    }
}
