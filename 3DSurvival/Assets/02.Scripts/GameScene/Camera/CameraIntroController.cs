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
        player.SetActive(false);
        uiRoot.SetActive(false);
        playerCamera.SetActive(false);    // PlayerCamera도 꺼두기

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
