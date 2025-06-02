using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenuController : MonoBehaviour
{
    [Header("패널 연결")]
    public GameObject menuPanel; // 메뉴 버튼 눌렀을 때 열리는 메뉴 UI

    [Header("메인 씬 이름")]
    public string mainMenuSceneName = "TitleScene";


    private bool isMenuOpen = false;

    // 메뉴 버튼 (☰) 클릭 시
    public void OnClickMenuButton()
    {
        // 인트로 재생 중이면 메뉴 열기 막기
        if (CameraIntroController.IsIntroPlaying)
        {
            Debug.Log("[메뉴] 인트로 재생 중에는 메뉴를 열 수 없습니다.");
            return;
        }

        menuPanel.SetActive(true);
        isMenuOpen = true;
        Time.timeScale = 0f;
    }

    // 돌아가기 버튼
    public void OnClickBack()
    {
        menuPanel.SetActive(false);
        isMenuOpen = false;
        Time.timeScale = 1f;
    }

    // 메인화면으로
    public void OnClickGoToMain()
    {
        Time.timeScale = 1f;
        Debug.Log("[OnClickGoToMain] 씬 전환 시도: " + mainMenuSceneName);
        SceneManager.LoadScene(mainMenuSceneName);
    }

    // 게임 종료
    public void OnClickQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // ESC 키 대응
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isMenuOpen)
        {
            OnClickBack();
        }
    }
}
