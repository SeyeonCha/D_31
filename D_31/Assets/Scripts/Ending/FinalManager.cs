using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class FinalManager : MonoBehaviour
{
    // --- 퀘스트 선택 (Start Panel) ---
    public GameObject LastQuestPanel;
    public TextMeshProUGUI CannotRideSpaceShipText;
    
    // --- 1. 헤드라인 (1_Headlines) ---
    public GameObject HeadlinePanel;
    public TypingEffect headlineTypingEffect;
    [Header("Headline Text")]
    [TextArea(3, 5)]
    public string headlineSentence;
    
    // --- 2. 뉴스 보도 (2_News) ---
    public GameObject NewsPanel;
    public Animator anchorAnimator;
    public TypingEffect newsDialogueTypingEffect;
    [TextArea(3, 10)]
    public string[] newsDialogueSentences;

    // --- 3. 엔딩 분기 (Ending Panels) ---
    public GameObject Ending1Panel; // Yes 버튼 선택 시
    public GameObject Ending2Panel; // No 버튼 선택 시

    // --- 4. 최종 진실 (Truth Panel) ---
    public GameObject TruthPanel;

    private int currentPhase = 0; // 0: Headline, 1: News
    private int sentenceIndex = 0;
    private bool isYesSelected = false; // Yes/No 선택 결과 저장

    void Start()
    {
        // 모든 패널 비활성화 후, LastQuestPanel만 활성화
        HeadlinePanel.SetActive(false);
        NewsPanel.SetActive(false);
        Ending1Panel.SetActive(false);
        Ending2Panel.SetActive(false);
        TruthPanel.SetActive(false);
        
        LastQuestPanel.SetActive(true);
        currentPhase = -1; // 초기 상태: 퀘스트 선택 대기

        if (CannotRideSpaceShipText != null)
        {
            CannotRideSpaceShipText.text = "";
        }
    }

    void Update()
    {
        // 뉴스(Phase 1) 또는 헤드라인(Phase 0) 재생 중일 때만 Spacebar 처리
        if (currentPhase == 0 || currentPhase == 1)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                HandleSpacebarPress();
            }
        }
    }

    // --- LastQuestPanel 버튼 이벤트 핸들러 ---
    public void OnYesButtonClicked()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager 인스턴스를 찾을 수 없습니다.");
            return;
        }

        bool isConditionMet = GameManager.isLotteryToBroker || GameManager.isOrganSold;

        if (isConditionMet)
        {
            // 조건 충족: 엔딩 흐름 시작
            isYesSelected = true;
            
            // 경고 텍스트 숨기기 (혹시 이전에 표시되었다면)
            // if (CannotRideSpaceShipText != null)
            // {
            //     CannotRideSpaceShipText.text = "";
            // }
            
            ProceedToHeadlines();
        }
        else
        {
            // 조건 미충족: 경고 메시지 표시
            if (CannotRideSpaceShipText != null)
            {
                CannotRideSpaceShipText.text = "당신은 우주선을 탈 수 없습니다.";
                Debug.Log("우주선 탑승 불가: 조건 미충족.");
                // LastQuestPanel은 활성 상태로 유지
            }
            else
            {
                Debug.LogError("CannotRideSpaceShipText가 Inspector에 할당되지 않았습니다.");
            }
        }
    }
    public void OnNoButtonClicked()
    {
        // if (CannotRideSpaceShipText != null)
        // {
        //     CannotRideSpaceShipText.text = "";
        // }

        isYesSelected = false;
        ProceedToHeadlines();
    }

    void ProceedToHeadlines()
    {
        LastQuestPanel.SetActive(false);
        
        // 1_Headlines 재생 시작
        HeadlinePanel.SetActive(true);
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
        if (currentPhase == 0) // Headline Panel (1_Headlines)
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
        else if (currentPhase == 1) // News Panel (2_News)
        {
            if (newsDialogueTypingEffect.IsTyping)
            {
                newsDialogueTypingEffect.SkipTyping(newsDialogueSentences[sentenceIndex]);
            }
            else
            {
                sentenceIndex++;
                
                if (sentenceIndex < newsDialogueSentences.Length)
                {
                    // 다음 문장 재생
                    newsDialogueTypingEffect.StartTyping(newsDialogueSentences[sentenceIndex]);
                }
                else
                {
                    // 2_News dialogue 종료 -> 엔딩 분기로 이동
                    TransitionToEnding();
                }
            }
        }
    }

    void TransitionToNewsReport()
    {
        currentPhase = 1;
        
        NewsPanel.SetActive(true);
        HeadlinePanel.SetActive(false);

        if (anchorAnimator != null) anchorAnimator.enabled = true;

        if (newsDialogueSentences.Length > 0)
        {
            sentenceIndex = 0;
            newsDialogueTypingEffect.StartTyping(newsDialogueSentences[sentenceIndex]);
        }
    }

    void TransitionToEnding()
    {
        NewsPanel.SetActive(false);
        currentPhase = 2; // 엔딩 재생 페이즈

        if (isYesSelected)
        {
            Ending1Panel.SetActive(true);
            // EndingManager 스크립트가 Ending1Panel에 붙어있다고 가정
            Ending1Panel.GetComponent<EndingManager>()?.StartEndingScenario();
        }
        else
        {
            Ending2Panel.SetActive(true);
            // EndingManager 스크립트가 Ending2Panel에 붙어있다고 가정
            Ending2Panel.GetComponent<EndingManager>()?.StartEndingScenario();
        }
    }

    // EndingManager 스크립트에서 호출될 공통 진입 메서드
    public void ProceedToTruth()
    {
        // Ending 패널들 비활성화 (어느 쪽이든)
        Ending1Panel.SetActive(false);
        Ending2Panel.SetActive(false);

        // Truth 패널 활성화
        TruthPanel.SetActive(true);
        currentPhase = 3; // Truth 재생 페이즈

        // Truth 패널에도 EndingManager 스크립트가 붙어있다고 가정
        TruthPanel.GetComponent<EndingManager>()?.StartEndingScenario();
    }

    // TruthManager 스크립트에서 호출될 최종 종료 메서드
    public void GoToCreditScene()
    {
        SceneManager.LoadScene("Credit"); // 'Credit' 씬 이름으로 변경하세요.
    }
}