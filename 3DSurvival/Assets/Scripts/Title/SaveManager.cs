using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private string savePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            savePath = Path.Combine(Application.persistentDataPath, "save.json");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool HasSavedData()
    {
        return File.Exists(savePath);
    }

    public void ResetData()
    {
        Debug.Log(Application.persistentDataPath);

        if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }
    }

    public void SaveData(MyGameData data)
    {
        string json = JsonUtility.ToJson(data, true); //  저장
        File.WriteAllText(savePath, json);
    }

    public MyGameData LoadData()
    {
        if (!File.Exists(savePath)) return null;

        string json = File.ReadAllText(savePath);
        return JsonUtility.FromJson<MyGameData>(json);
    }

}

[System.Serializable]
public class MyGameData
{
    public int hp;
    public int gold;
    public int level;
    // 필요한 필드 추가
}
