using UnityEngine.Playables;
using UnityEngine;

public class CameraIntroController : MonoBehaviour
{
    public PlayableDirector director;
    public GameObject player;
    public GameObject uiRoot;
    public GameObject menuButton;

    public static bool IsIntroPlaying { get; private set; } = true;

    void Start()
    {
        player.SetActive(false);
        uiRoot.SetActive(false);
        menuButton.SetActive(false);

        IsIntroPlaying = true;

        director.stopped += OnFinished;
        director.Play();
    }

    void OnFinished(PlayableDirector d)
    {
        IsIntroPlaying = false;
        player.SetActive(true);
        uiRoot.SetActive(true);
        menuButton.SetActive(true);
    }
}
