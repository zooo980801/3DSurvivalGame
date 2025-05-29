using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [Header("대화창 & 텍스트")]
    public GameObject dialogue;      // 대화창 전체 
    public TextMeshProUGUI dialogueText;  // 대사 텍스트

    [Header("선택지 버튼")]
    public GameObject selectBtn1;    // 선택지 버튼 1
    public GameObject selectBtn2;    // 선택지 버튼 2

    [Header("대화 종료 버튼")]
    public GameObject closeBtn;

    private Button btn1;
    private Button btn2;
    private TextMeshProUGUI btn1Text;
    private TextMeshProUGUI btn2Text;

    private void Awake()
    {
        // Button 컴포넌트와 자식 TextMeshProUGUI 캐싱
        btn1 = selectBtn1.GetComponent<Button>();
        btn2 = selectBtn2.GetComponent<Button>();
        btn1Text = selectBtn1.GetComponentInChildren<TextMeshProUGUI>();
        btn2Text = selectBtn2.GetComponentInChildren<TextMeshProUGUI>();

        // 초기엔 대화창 숨김
        SetDialogueActive(false);
    }

    //대화창 전체 On
    public void SetDialogueActive(bool active)
    {
        dialogue.SetActive(active);
    }
    //대화창 전체 off
    public void SetEndButtonActive(bool active)
    {
        closeBtn.SetActive(active);
    }
    //대사 텍스트 변경할 때 호출
    public void SetDialogueText(string txt)
    {
        dialogueText.text = txt;
    }

    //선택지 버튼 보이기
    public void ShowChoiceButtons()
    {
        selectBtn1.SetActive(true);
        selectBtn2.SetActive(true);
    }

    //모든 선택지 숨기기
    public void HideAllChoices()
    {
        selectBtn1.SetActive(false);
        selectBtn2.SetActive(false);
    }

    //특정 번호 선택지 숨기기
    public void HideChoice(int number)
    {
        if (number == 1) selectBtn1.SetActive(false);
        else if (number == 2) selectBtn2.SetActive(false);
    }

    //특정 선택지 설정
    //number: 1 또는 2  
    //text: 버튼에 표시할 문장  
    //callback: 클릭 시 실행할 액션(다음 대사 호출)
    public void SetChoice(int number, string text, Action callback)
    {
        if (number == 1)
        {
            btn1Text.text = text;//버튼 텍스트 변경
            btn1.onClick.RemoveAllListeners();//이전에 있던 모든 클릭 이벤트 제거
            btn1.onClick.AddListener(() => callback());//새로운 콜백 등록
        }
        else if (number == 2)
        {
            btn2Text.text = text;
            btn2.onClick.RemoveAllListeners();
            btn2.onClick.AddListener(() => callback());
        }
    }
}
