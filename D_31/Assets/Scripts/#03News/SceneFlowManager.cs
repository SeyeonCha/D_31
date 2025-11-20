using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneFlowManager : MonoBehaviour
{
    public GameObject HeadlinePanel;
    public GameObject headlineContent;
    public TypingEffect headlineTypingEffect;

    [Header("Headline Text")]
    [TextArea(3, 5)]
    public string headlineSentence;

    public GameObject NewsPanel;

    // 🚨 최종 팝업 패널 참조
    [Header("Conditional Flow")]
    [Tooltip("조건 충족 시 뉴스 대화 후 활성화할 최종 팝업 패널")]
    public GameObject FinalPopupPanel;

    public GameObject newsContent;
    public Animator anchorAnimator;
    public TypingEffect dialogueTypingEffect;

    [TextArea(3, 10)]
    public string[] dialogueSentences;
    public string nextSceneName;

    private int currentPhase = 0; // 0: 헤드라인, 1: 뉴스 대화, 2: 최종 팝업 대기
    private int sentenceIndex = 0;

    private bool isOrganSoldConditionMet;

    void Start()
    {
        HeadlinePanel.SetActive(true);
        NewsPanel.SetActive(false);

         // FinalPopupPanel은 시작 시 비활성화합니다.
        if (FinalPopupPanel != null)
        {
            FinalPopupPanel.SetActive(false);
        }
        
        // 🚨 조건 확인 및 저장
        // GameManager는 정적 클래스이므로 직접 접근합니다.
        isOrganSoldConditionMet = (GameManager.isOrganSold == true && GameManager.DayEnded == 3);
        
        ProceedToHeadlines();

        //headlineTypingEffect.StartTyping(headlineTypingEffect.GetComponent<TMP_Text>().text);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandleSpacebarPress();
        }
    }

    void ProceedToHeadlines()
    {
        currentPhase = 0;
        
        if (!string.IsNullOrEmpty(headlineSentence))
        {
            headlineTypingEffect.StartTyping(headlineSentence);
        }
        else
        {
            Debug.LogError("헤드라인 텍스트가 비어있습니다. Inspector에서 입력해주세요.");
        }
    }

    void HandleSpacebarPress()
    {
        if (currentPhase == 0) // 헤드라인 단계
        {
            if (headlineTypingEffect.IsTyping)
            {
                headlineTypingEffect.SkipTyping(headlineSentence);
            }
            else
            {
                TransitionToNewsReport();
            }
        }
        else if (currentPhase == 1) // 뉴스 대화 단계
        {
            if (dialogueTypingEffect.IsTyping)
            {
                dialogueTypingEffect.SkipTyping(dialogueSentences[sentenceIndex]);
            }
            else
            {
                sentenceIndex++;
                
                if (sentenceIndex < dialogueSentences.Length)
                {
                    dialogueTypingEffect.StartTyping(dialogueSentences[sentenceIndex]);
                }
                else
                {
                    // 🚨 대화 끝: 조건 검사 후 흐름 변경
                    if (isOrganSoldConditionMet)
                    {
                        ActivateFinalPanel();
                    }
                    else
                    {
                        // 조건 미충족 시 다음 씬으로 바로 이동
                        SceneManager.LoadScene(nextSceneName);
                    }
                }
            }
        }
        else if (currentPhase == 2) // 🚨 🚨 🚨 최종 팝업 대기 단계 (로직 추가) 🚨 🚨 🚨
        {
            // 최종 패널이 활성화된 상태에서 스페이스바를 누르면 다음 씬으로 이동합니다.
            SceneManager.LoadScene(nextSceneName);
        }
    }

    void TransitionToNewsReport()
    {
        currentPhase = 1;
        
        NewsPanel.SetActive(true);
        HeadlinePanel.SetActive(false);

        if (anchorAnimator != null) anchorAnimator.enabled = true;

        if (dialogueSentences.Length > 0)
        {
            sentenceIndex = 0;
            dialogueTypingEffect.StartTyping(dialogueSentences[sentenceIndex]);
        }
    }

    /// 조건 충족 시 최종 팝업 패널을 활성화하고 단계를 변경합니다.
    /// </summary>
    void ActivateFinalPanel()
    {
        if (FinalPopupPanel != null)
        {
            currentPhase = 2; // 최종 팝업 대기 단계로 전환
            NewsPanel.SetActive(false); // 뉴스 패널은 비활성화
            FinalPopupPanel.SetActive(true); // 최종 팝업 활성화
            Debug.Log("[SceneFlowManager] Final Popup Panel activated. Waiting for spacebar input to load next scene.");
        }
        else
        {
            Debug.LogError("[SceneFlowManager] FinalPopupPanel is not assigned. Loading next scene directly.");
            SceneManager.LoadScene(nextSceneName);
        }
    }
}