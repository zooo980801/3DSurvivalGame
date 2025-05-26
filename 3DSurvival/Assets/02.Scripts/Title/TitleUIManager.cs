using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // ← 코루틴 쓸 때 필요

public class TitleUIManager : MonoBehaviour
{
    [Header("에러 메시지 캔버스")]
    public GameObject loadErrorUI; // Inspector에서 연결해야 함

    private Coroutine blinkCoroutine;

    public void OnClickNewGame()
    {
        SaveManager.Instance.ResetData();
        SceneManager.LoadScene("GameScene");
    }

    public void OnClickLoadGame()
    {
        if (SaveManager.Instance.HasSavedData())
        {
            SaveManager.Instance.LoadData();
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            Debug.Log("저장된 데이터가 없습니다.");

            if (blinkCoroutine != null)
                StopCoroutine(blinkCoroutine); // 중복 방지

            blinkCoroutine = StartCoroutine(BlinkLoadError());
        }
    }

    public void OnClickQuit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private IEnumerator BlinkLoadError()
    {
        int count = 3;
        float interval = 0.3f;

        for (int i = 0; i < count; i++)
        {
            loadErrorUI.SetActive(true);
            yield return new WaitForSeconds(interval);
            loadErrorUI.SetActive(false);
            yield return new WaitForSeconds(interval);
        }
    }

}
