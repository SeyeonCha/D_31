using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CafeManager : MonoBehaviour
{
    [SerializeField]
    private DisplayScrapData displayer;

    private CafeTitle sourceTitle;

    private int classId;
    private int clicked;

    // =========================================================
    // 시나리오 관련 필드
    // =========================================================
    [Header("Scenario Dialogue")]
    [Tooltip("대화 패널 전체 GameObject")]
    public GameObject dialoguePanel; 
    [Tooltip("대화 시스템을 처리하는 컴포넌트 (DialogueSystem 같은)")]
    public DialogueSystem dialogueSystem; // DialogueSystem 컴포넌트가 필요합니다.
    [Tooltip("대화 완료 후 업데이트할 퀘스트 텍스트")]
    public TextMeshProUGUI questText;
    
    [Header("Scenario Data")]
    // [TextArea] 속성을 추가하여 인스펙터에서 여러 줄 편집이 용이하도록 합니다.
    [TextArea(4, 15)] // 최소 4줄, 최대 15줄 높이로 인스펙터에 표시
    [Tooltip("순서대로 표시될 대화 문장들. '화자: 내용' 형식으로 작성하세요.")]
    public string[] scenarioSentences; 
    
    [TextArea(4, 15)]
    [Tooltip("대화 완료 후 QuestText에 표시될 메시지")]
    public string questUpdateMessage; 
    // =========================================================

    public TextMeshProUGUI title;
    public TextMeshProUGUI writer;
    // public TextMeshProUGUI views;
    public TextMeshProUGUI like;
    public TextMeshProUGUI content;

    // 댓글 UI 
    public List<TextMeshProUGUI> comment1;
    public List<TextMeshProUGUI> comment2;
    public List<TextMeshProUGUI> comment3;

    private List<List<TextMeshProUGUI>> comments;


    // 👇 길이 반영 딜레이 문제 때문에 아래 코드 추가됨
    public RectTransform cafePanelRectTransform;



    void Awake()
    {
        comments = new List<List<TextMeshProUGUI>>() {comment1, comment2, comment3};

        // 팝업 패널은 비활성화 상태로 시작합니다.
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
    }

    public void GetSourceTitle(CafeTitle stitle)
    {
        // 데이터의 정보 받아오기
        sourceTitle = stitle;

        classId = sourceTitle.data.classId;
        clicked = sourceTitle.data.isScrapped;

        // =========================================================
        // 시나리오 시작 조건 확인 (DayEnded = 1, uniqueId = 6)
        // =========================================================
        if (GameManager.DayEnded == 1 && sourceTitle.data.uniqueId == 6)
        {
            // 조건 충족 시 5초 대기 코루틴 호출
            StartCoroutine(CheckScenarioCondition());
        }
        // =========================================================

        // 패널 UI 텍스트들 채우기
        title.text = sourceTitle.data.title;
        writer.text = sourceTitle.data.writer;
        // views.text = sourceTitle.data.views.ToString();
        like.text = sourceTitle.data.like.ToString();
        content.text = sourceTitle.data.content.Replace("<n>","\n");
        
        for (int i = 0; i<3;i++)
        {
            comments[i][0].text = sourceTitle.data.comments[i][0];
            comments[i][1].text = sourceTitle.data.comments[i][1];
        }


        // 👇 길이 반영 딜레이 문제 때문에 아래 코드 추가됨
        if (cafePanelRectTransform != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(cafePanelRectTransform);
        }
    }

    /// 시나리오 시작 조건이 충족되면 5초를 대기한 후 대화를 시작합니다.
    private IEnumerator CheckScenarioCondition()
    {
        Debug.Log("[CafeManager] Scenario condition met. Waiting for 5 seconds before starting dialogue...");
        
        // 5초 대기
        yield return new WaitForSeconds(5f);
        
        Debug.Log("[CafeManager] 5 seconds elapsed. Starting dialogue.");
        
        // 대화 시작 코루틴 호출
        StartCoroutine(StartScenarioDialogue());
    }

    /// 시나리오 대화를 시작하고 완료될 때까지 관리
    private IEnumerator StartScenarioDialogue()
    {
        if (dialoguePanel == null || dialogueSystem == null || questText == null)
        {
            Debug.LogError("[CafeManager] Dialogue components (Panel, System, or QuestText) are not assigned.");
            yield break;
        }

        // 1. 대화 패널 활성화
        dialoguePanel.SetActive(true);

        // 2. DialogueSystem에게 대화 시작을 요청하고 완료될 때까지 기다립니다.
        // DialogueSystem에는 대화를 모두 표시한 후 true를 반환하는 코루틴이 있다고 가정합니다.
        yield return dialogueSystem.StartDialogue(scenarioSentences); 

        // 3. 대화 완료 후
        
        // 대화 패널 비활성화
        dialoguePanel.SetActive(false);

         if (!questText.gameObject.activeSelf)
        {
            questText.gameObject.SetActive(true);
        }
        
        // 퀘스트 텍스트 업데이트
        questText.text = questUpdateMessage;
        
        Debug.Log("[CafeManager] Scenario Dialogue finished and Quest Text updated.");
    }
    
    // 스크랩 버튼이 눌리면 불러와질 함수 정의
    public void ScrapButtonClicked()
    {
        sourceTitle.data.isScrapped = 1;
        Debug.Log($"SCRAPPED : {sourceTitle.data.isScrapped}, class : {classId}");

        displayer.ScrapCounter(classId);

    }
}
