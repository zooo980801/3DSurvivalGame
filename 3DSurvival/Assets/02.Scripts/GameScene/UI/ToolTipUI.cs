using System;
using UnityEngine;
using TMPro;

public class TooltipUI : MonoBehaviour
{
    private static TooltipUI _instance;
    public static TooltipUI Instance { get; set; }

    public GameObject panel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI statsText;

    private RectTransform rectTransform;

    private void Awake()
    {
        Instance = this;
        rectTransform = panel.GetComponent<RectTransform>();
        Hide();
    }

    private void Update()
    {
        if (panel.activeSelf)
        {
            Vector2 mousePos = Input.mousePosition;
            rectTransform.position = mousePos + new Vector2(10, -10); // 살짝 옆에
        }
    }

    public void Show(ItemData item)
    {
        nameText.text = item.displayName;
        descriptionText.text = item.description;
        statsText.text = String.Empty;
        for (int i = 0; i < item.consumables.Length; i++)
        {
            statsText.text += $"{item.consumables[i].type.ToString()} : {item.consumables[i].value.ToString()} \n";
        }



        panel.SetActive(true);
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}