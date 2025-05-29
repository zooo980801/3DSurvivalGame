using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#region 대화데이터
[System.Serializable]
public class DialogChoice
{
    public string text;
    public int nextId;
}

[System.Serializable]
public class DialogLine
{
    public int id;
    public string text;
    public List<DialogChoice> choices;
}

[System.Serializable]
public class DialogueData
{
    public List<DialogLine> lines;
}
#endregion
public class DialogueManager : MonoBehaviour
{
    [SerializeField] private DialogueUI dialogueUI;

    private DialogueData dialogueData;
    private List<DialogLine> startableLines;  // 대화 시작용 라인들만 모아둠
    private Dictionary<int, DialogLine> lookup;
    public bool isTalk= false;//임시. 대화중인지 표시

    private void Awake()
    {
        // JSON 로드
        TextAsset ta = Resources.Load<TextAsset>("Dialogue");
        dialogueData = JsonUtility.FromJson<DialogueData>(ta.text);

        // lookup 사전 생성
        lookup = new Dictionary<int, DialogLine>();
        foreach (var line in dialogueData.lines)
            lookup[line.id] = line;

        // 후속 대사 ID 수집
        var referencedIds = new HashSet<int>();
        foreach (var line in dialogueData.lines)
            if (line.choices != null)
                foreach (var c in line.choices)
                    referencedIds.Add(c.nextId);

        // 대화 시작용 라인 필터링
        startableLines = new List<DialogLine>();
        foreach (var line in dialogueData.lines)
        {
            bool hasChoice = line.choices != null && line.choices.Count > 0;
            bool isReply = referencedIds.Contains(line.id);

            if (hasChoice || (!hasChoice && !isReply))
                startableLines.Add(line);
        }
    }

    private void Update()
    {
        //임시 키 설정
        if (!isTalk && Input.GetKeyDown(KeyCode.Alpha2))
        {
            isTalk = true;
            StartConversation();
        }
    }

    //대화 시작 (랜덤)
    public void StartConversation()
    {
        if (startableLines.Count == 0) return;

        int idx = Random.Range(0, startableLines.Count);
        ShowLine(startableLines[idx]);
    }

    private void ShowLine(DialogLine line)
    {
        dialogueUI.SetDialogueActive(true);
        dialogueUI.SetDialogueText(line.text);

        bool hasChoice = (line.choices != null && line.choices.Count > 0);

        if (hasChoice)
        {
            // 선택지가 있는 대사일 때: 선택지 표시, 끝내기 버튼 숨기기
            dialogueUI.ShowChoiceButtons();
            dialogueUI.SetEndButtonActive(false);

            // 첫 번째 선택지
            dialogueUI.SetChoice(1, line.choices[0].text,
                () => OnChoice(line.choices[0].nextId));

            // 두 번째 선택지 (없으면 숨김)
            if (line.choices.Count > 1)
                dialogueUI.SetChoice(2, line.choices[1].text,
                    () => OnChoice(line.choices[1].nextId));
            else
                dialogueUI.HideChoice(2);
        }
        else
        {
            // 일반 대사일 때: 선택지 숨기고 끝내기 버튼 표시
            dialogueUI.HideAllChoices();
            dialogueUI.SetEndButtonActive(true);
        }
    }

    private void OnChoice(int nextId)
    {
        // 선택 후 후속 대사 보여주기
        if (lookup.TryGetValue(nextId, out var nextLine))
            ShowLine(nextLine);
        else
            EndConversation();
    }

    // 대화끝내기 버튼에 OnClick에 넣기
    public void EndConversation()
    {
        dialogueUI.SetDialogueActive(false);
        dialogueUI.HideAllChoices();
        dialogueUI.SetEndButtonActive(false);
        isTalk = false;
    }
}
