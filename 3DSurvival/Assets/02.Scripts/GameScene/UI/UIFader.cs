using UnityEngine;
using System.Collections;

public class UIFader : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 1.0f;

    public void FadeIn()
    {
        gameObject.SetActive(true);
        StartCoroutine(FadeRoutine(0f, 1f));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeRoutine(1f, 0f));
    }

    private IEnumerator FadeRoutine(float from, float to)
    {
        float elapsed = 0f;
        canvasGroup.alpha = from;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = to;

        if (to == 0f)
            gameObject.SetActive(false);
    }
}
