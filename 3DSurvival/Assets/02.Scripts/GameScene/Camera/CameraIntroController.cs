using UnityEngine.Playables;
using UnityEngine;

public class CameraIntroController : MonoBehaviour
{
    public PlayableDirector director;
    public GameObject player;
    public GameObject uiRoot;

    [Header("카메라 전환용")]
    public GameObject mainCamera;      // MainCamera 직접 할당
    public GameObject playerCamera;    // PlayerCamera 직접 할당

    public static bool IsIntroPlaying { get; private set; } = true;

    void Start()
    {
        if (!SaveManager.IsNewGame)
        {
            // 불러오기로 시작했다면 인트로 생략
            player.SetActive(true);
            uiRoot.SetActive(true);
            mainCamera?.SetActive(false);
            playerCamera?.SetActive(true);
            IsIntroPlaying = false;
            return;
        }

        // 새 게임이면 인트로 실행
        player.SetActive(false);
        uiRoot.SetActive(false);
        playerCamera.SetActive(false);
        IsIntroPlaying = true;

        director.stopped += OnFinished;
        director.Play();
    }


    void OnFinished(PlayableDirector d)
    {
        IsIntroPlaying = false;

        player.SetActive(true);
        uiRoot.SetActive(true);

        // 카메라 전환
        if (mainCamera != null) mainCamera.SetActive(false);
        if (playerCamera != null) playerCamera.SetActive(true);
    }
}
