using UnityEngine;
using UnityEngine.Playables;

public class CameraIntroController : MonoBehaviour
{
    public PlayableDirector director;
    public GameObject player;
    public GameObject uiRoot;

    void Start()
    {
        player.SetActive(false);
        uiRoot.SetActive(false);

        director.stopped += OnFinished;
        director.Play();
    }

    void OnFinished(PlayableDirector d)
    {
        player.SetActive(true);
        uiRoot.SetActive(true);
    }
}
