[System.Serializable]
public class SavedMapObject
{
    public string prefabId;
    public float posX, posY, posZ;
    public float rotX, rotY, rotZ;
    public float scaleX, scaleY, scaleZ;

    // 선택 사항 (예: 체력, 상태)
    public float health;
    public bool isDestroyed;
}
