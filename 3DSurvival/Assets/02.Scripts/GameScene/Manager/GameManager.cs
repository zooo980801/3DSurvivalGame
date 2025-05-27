using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameClock clock;                     // GameClock 컴포넌트 참조 (Inspector에서 연결)
    public SleepManager sleepManager;           // SleepManager 컴포넌트 참조 (Inspector에서 연결)
    private RandomEventManager eventManager;    // 코드로 추가할 RandomEventManager 컴포넌트

    void Start()
    {
        // SleepManager에 GameClock 연결
        sleepManager.clock = clock;

        // RandomEventManager를 동적으로 추가하고 GameClock 연결
        eventManager = gameObject.AddComponent<RandomEventManager>();
        eventManager.clock = clock;
    }
}
