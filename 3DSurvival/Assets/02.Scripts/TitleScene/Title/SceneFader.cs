using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    public Image fadeImage;                   // Fade 효과에 사용할 UI Image (검은 배경)
    public float fadeDuration = 1f;           // Fade에 걸리는 시간 (초)

    // 씬을 전환할 때 사용하는 메서드
    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeAndLoad(sceneName));   // 코루틴으로 Fade 후 씬 로드
    }

    // 게임을 종료할 때 사용하는 메서드
    public void FadeAndQuit()
    {
        StartCoroutine(FadeAndQuitCoroutine());   // 코루틴으로 Fade 후 종료
    }

    // Fade Out 후 씬을 로드하는 코루틴
    private IEnumerator FadeAndLoad(string sceneName)
    {
        yield return StartCoroutine(Fade(0f, 1f));  // 투명 → 불투명으로 Fade Out
        SceneManager.LoadScene(sceneName);          // Fade가 끝난 뒤 씬 로드
    }

    // Fade Out 후 게임을 종료하는 코루틴
    private IEnumerator FadeAndQuitCoroutine()
    {

        Debug.Log("▶ 페이드 시작");
        yield return StartCoroutine(Fade(0f, 5f));  // 투명 → 불투명으로 Fade Out

        Debug.Log("▶ 페이드 완료");
        yield return new WaitForSeconds(5f);         // 반초 정도 기다리기 (사용자가 페이드 끝을 볼 수 있게)

        Debug.Log("▶ 대기 완료 후 종료");
        Application.Quit();                         // 애플리케이션 종료
        

#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false; // 에디터에서는 Play 모드 종료
#endif
    }

    // 알파 값을 조절하여 Fade 효과를 주는 코루틴
    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float time = 0f;                              // 경과 시간 초기화
        Color color = fadeImage.color;               // 현재 이미지의 색상을 가져옴

        // 지정된 시간 동안 알파 값을 서서히 변화시킴
        while (time < fadeDuration)
        {
            time += Time.deltaTime;                      // 경과 시간 누적
            float t = time / fadeDuration;               // 진행률 (0~1)
            color.a = Mathf.Lerp(startAlpha, endAlpha, t); // 알파 값 보간 (Lerp)
            fadeImage.color = color;                     // 이미지에 적용
            yield return null;                           // 다음 프레임까지 대기
        }

        color.a = endAlpha;                         // 마지막 알파 값 보정
        fadeImage.color = color;                    // 이미지에 최종 색상 적용
    }
}