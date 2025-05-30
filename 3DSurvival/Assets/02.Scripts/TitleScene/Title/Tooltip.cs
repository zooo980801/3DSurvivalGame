using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI Help;
    public GameObject tooltipPanel;
    public Camera uiCamera;

    private void Awake()
    {
        tooltipPanel.SetActive(false);
    }

    public void OnHelpButtonClick()
    {
        tooltipPanel.SetActive(true);
        Help.gameObject.SetActive(false);

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent as RectTransform,
            Input.mousePosition + new Vector3(0, 50f),
            uiCamera,
            out pos);

        tooltipPanel.GetComponent<RectTransform>().anchoredPosition = pos;
    }

    public void OnCloseButtonClick()
    {
        tooltipPanel.SetActive(false);
        Help.gameObject.SetActive(true);
    }
}
