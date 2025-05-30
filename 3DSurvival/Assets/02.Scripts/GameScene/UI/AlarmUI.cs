using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlarmUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI alarmText;
    [SerializeField] private CanvasGroup canvasGroup;

    private Coroutine alarmCoroutine;

    private void Awake()
    {
        // 시작 시 감춰두기
        if (canvasGroup != null)
            canvasGroup.alpha = 0f;
    }

    public void Show(string message, float duration = 2f)
    {
        Debug.Log($"[AlarmUI] Show 호출됨: {message}"); // 로그 추가
        if (alarmCoroutine != null)
            StopCoroutine(alarmCoroutine);

        alarmCoroutine = StartCoroutine(AlarmRoutine(message, duration));
    }

    private IEnumerator AlarmRoutine(string message, float duration)
    {
        alarmText.text = message;

        // 나타나기
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / 0.2f; // 0.2초 안에 1로 가게
            canvasGroup.alpha = t; // 점점 증가
            yield return null;
        }
        canvasGroup.alpha = 1f;


        yield return new WaitForSeconds(duration);

        // 사라지기
        t = 1f;
        while (t > 0f)
        {
            t -= Time.deltaTime / 0.5f; // 0.5초에 걸쳐 사라지기
            canvasGroup.alpha = t;
            yield return null;
        }
        canvasGroup.alpha = 0f;

    }
}
