using UnityEngine;

public class UIClickSoundManager : MonoBehaviour
{
    public static UIClickSoundManager Instance;

    private AudioSource audioSource;

    [Header("클릭 사운드 클립")]
    public AudioClip clickSound;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayClickSound()
    {
        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}
