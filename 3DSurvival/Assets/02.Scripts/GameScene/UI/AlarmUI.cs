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
        if (alarmCoroutine != null)
            StopCoroutine(alarmCoroutine);

        alarmCoroutine = StartCoroutine(AlarmRoutine(message, duration));
    }

    private IEnumerator AlarmRoutine(string message, float duration)
    {
        alarmText.text = message;

        // 나타나기
        float t = 0f;
        while (t < 0.2f)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / 0.2f);
            yield return null;
        }
        canvasGroup.alpha = 1f;

        yield return new WaitForSeconds(duration);

        // 사라지기
        t = 0f;
        while (t < 0.5f)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / 0.5f);
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }
}
