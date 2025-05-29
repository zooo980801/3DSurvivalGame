using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameClock clock;                     // GameClock 컴포넌트 참조 (Inspector에서 연결)
    public SleepManager sleepManager;           // SleepManager 컴포넌트 참조 (Inspector에서 연결)
    private RandomEventManager eventManager;    // 코드로 추가할 RandomEventManager 컴포넌트

    public GameObject gameOverUI;
    public GameObject clearUI;

    public UIFader Fader;

    void Start()
    {
        // SleepManager에 GameClock 연결
        sleepManager.clock = clock;

        // RandomEventManager를 동적으로 추가하고 GameClock 연결
        eventManager = gameObject.AddComponent<RandomEventManager>();
        eventManager.clock = clock;

        // 날짜 변경 이벤트 연결
        clock.OnDayChanged += CheckGameClearCondition;
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
        Debug.Log("선비가 죽었습니다...");
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
}
