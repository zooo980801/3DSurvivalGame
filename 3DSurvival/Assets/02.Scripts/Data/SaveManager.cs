using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance; // 싱글톤 인스턴스

    private string savePath; // 저장 파일의 전체 경로

    private void Awake()
    {
        if (Instance == null) // 싱글톤이 없으면
        {
            Instance = this; // 현재 인스턴스를 등록
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 이 오브젝트 유지
            savePath = Path.Combine(Application.persistentDataPath, "save.json"); // 저장 경로 설정
            Debug.Log("[SAVE] 저장 위치: " + Application.persistentDataPath);
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 있으면 중복 방지 위해 삭제
        }
    }

    public bool HasSavedData()
    {
        return File.Exists(savePath); // 저장 파일이 존재하는지 확인
    }

    public void ResetData()
    {
        Debug.Log(Application.persistentDataPath); // 저장 경로를 콘솔에 출력

        if (File.Exists(savePath)) // 파일이 존재하면
        {
            File.Delete(savePath); // 파일 삭제
        }
    }

    public void SaveData(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        Debug.Log("[SAVE] 저장 위치: " + Application.persistentDataPath);
        File.WriteAllText(savePath, json);
    }

    public SaveData LoadData()
    {
        if (!File.Exists(savePath)) return null;

        string json = File.ReadAllText(savePath);
        Debug.Log("[SAVE] 저장 위치: " + Application.persistentDataPath);
        return JsonUtility.FromJson<SaveData>(json);
    }

}

[System.Serializable] // 직렬화를 위해 필요 (JsonUtility 사용 시 필수)
public class MyGameData
{
    public int hp; // 플레이어 체력
    public int gold; // 보유 골드
    public int level; // 현재 레벨
    // 필요한 필드 추가
}