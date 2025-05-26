using UnityEngine;
using UnityEngine.UI;

public class TitleButtonSound : MonoBehaviour
{
    public AudioSource audioSource;      // 사운드 재생용 AudioSource
    public AudioClip clickClip;          // 버튼 클릭 사운드 클립

    public Button[] buttons;             // 소리를 적용할 버튼 목록

    private void Start()
    {
        // 각 버튼에 클릭 시 PlayClickSound 메서드를 이벤트로 등록
        foreach (var btn in buttons)
        {
            btn.onClick.AddListener(PlayClickSound);
        }
    }

    void PlayClickSound()
    {
        // 오디오 소스와 사운드 클립이 존재할 때만 재생
        if (audioSource != null && clickClip != null)
        {
            audioSource.PlayOneShot(clickClip); // 지정된 클립을 1회 재생
        }
    }
}