[System.Serializable]
public class SavedItem
{
    public string itemId;
    public int amount;
    public bool equipped; // 장착 상태 추가
    public int slotIndex; // 슬롯 인덱스 추가
}
