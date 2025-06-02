using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public void GoToTitle()
    {
        Debug.Log("타이틀 화면으로 이동합니다.");
        SceneManager.LoadScene("TitleScene"); 
    }
}
