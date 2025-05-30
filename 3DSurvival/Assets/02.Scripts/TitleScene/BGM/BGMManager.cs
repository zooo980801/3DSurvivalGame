using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance; // 싱글톤 인스턴스

    private AudioSource audioSource; // BGM 재생을 위한 오디오 소스

    [Header("메인 BGM")]
    public AudioClip loadingBGM; // 타이틀/로딩 화면에서 사용할 BGM 클립

    [Header("볼륨 설정")]
    public float volume = 0.1f;

    private void Awake()
    {
        if (Instance == null) // 인스턴스가 없으면
        {
            Instance = this; // 현재 인스턴스를 싱글톤으로 등록
            audioSource = gameObject.AddComponent<AudioSource>(); // AudioSource 컴포넌트 동적으로 추가
            audioSource.loop = true; // 반복 재생 설정
            audioSource.playOnAwake = false; // 시작 시 자동 재생 비활성화
        }
        else
        {
            Destroy(gameObject); // 중복 인스턴스 방지
        }
    }
    private void Start()
    {
        BGMManager.Instance?.PlayLoadingBGM();
        // Start()에서 별도 볼륨 설정하지 않음
    }

    public void PlayLoadingBGM()
    {
        if (audioSource.isPlaying) return;

        audioSource.volume = volume; 
        audioSource.clip = loadingBGM;
        audioSource.Play();
    }

    public void Stop()
    {
        audioSource.Stop(); // 현재 재생 중인 BGM 정지
    }
}