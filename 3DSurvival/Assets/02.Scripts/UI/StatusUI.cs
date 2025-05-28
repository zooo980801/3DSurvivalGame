using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour
{
    [SerializeField] private Image uiBar;       // UI바

    public void Bind(StatusData statusData)
    {
        statusData.OnValueChanged += UpdateUI;  // 이벤트 구독
    }

    private void UpdateUI(float percentage)
    {
        uiBar.fillAmount = percentage;          // UI바 업데이트
    }
}
