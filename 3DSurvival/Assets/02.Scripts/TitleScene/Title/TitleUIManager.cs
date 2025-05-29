using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // ← 코루틴 쓸 때 필요

public class TitleUIManager : MonoBehaviour
{
    [Header("에러 메시지 캔버스")]
    public GameObject loadErrorUI;   // 저장 데이터 없을 때 띄울 에러 UI (인스펙터에서 연결)

    private Coroutine blinkCoroutine; // 에러 메시지 깜빡임을 위한 코루틴 저장용 변수
    private void Start()
    {
        // 타이틀 화면에서 배경 음악 재생 시작
        BGMManager.Instance?.PlayLoadingBGM();

        // 볼륨을 0.2로 낮춤 (조용한 분위기용)
        BGMManager.Instance?.SetVolume(0.2f);
    }

    public SceneFader sceneFader; // Fade 효과와 씬 전환을 위한 SceneFader 연결

    // "새 게임" 버튼 클릭 시 호출
    public void OnClickNewGame()
    {
        SaveManager.IsNewGame = true; //새 게임 시작으로 설정

        SaveManager.Instance.ResetData(); // 기존 데이터 삭제

        SaveData data = new SaveData();   // 아무 값도 안 넣음 (기본 생성자만)

        SaveManager.Instance.SaveData(data); // 빈 구조만 저장

        sceneFader.FadeToScene("GameScene"); // 게임씬으로 이동
    }


    // "불러오기" 버튼 클릭 시 호출
    public void OnClickLoadGame()
    {


        // 저장 데이터가 존재하면
        if (SaveManager.Instance.HasSavedData())
        {
            SaveManager.IsNewGame = false;
            SaveManager.Instance.LoadData();        // 저장 데이터 로드
            sceneFader.FadeToScene("GameScene");    // GameScene으로 이동 (페이드 포함)
        }
        else
        {
            // 저장된 데이터가 없을 경우
            Debug.Log("저장된 데이터가 없습니다."); // 콘솔에 메시지 출력

            // 이미 실행 중인 코루틴이 있다면 중단
            if (blinkCoroutine != null)
                StopCoroutine(blinkCoroutine);       // 중복 깜빡임 방지

            // 에러 메시지 깜빡이는 코루틴 실행
            blinkCoroutine = StartCoroutine(BlinkLoadError());
        }
    }

    // "종료" 버튼 클릭 시 호출
    public void OnClickQuit()
    {
        sceneFader.FadeAndQuit(); // 페이드 아웃 후 게임 종료 처리
    }

    // 에러 메시지를 깜빡이게 만드는 코루틴
    private IEnumerator BlinkLoadError()
    {
        int count = 3;               // 깜빡일 횟수
        float interval = 0.3f;       // 깜빡임 간격 (초)

        for (int i = 0; i < count; i++)
        {
            loadErrorUI.SetActive(true);  // 에러 메시지 켜기
            yield return new WaitForSeconds(interval); // 대기
            loadErrorUI.SetActive(false); // 에러 메시지 끄기
            yield return new WaitForSeconds(interval); // 대기
        }
    }
}