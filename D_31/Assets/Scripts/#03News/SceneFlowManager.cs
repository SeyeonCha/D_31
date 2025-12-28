using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
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

    [Header("Day Panel")]
    public GameObject DayPanel;
    public TextMeshProUGUI DayText;
    public float fadeDuration = 1.0f; // 페이드 효과 시간

    public GameObject newsContent;
    public Animator anchorAnimator;
    public TypingEffect dialogueTypingEffect;

    [TextArea(3, 10)]
    public string[] dialogueSentences;
    public string nextSceneName;

    private int currentPhase = 0; // 0: 헤드라인, 1: 뉴스 대화, 2: 최종 팝업 대기
    private int sentenceIndex = 0;

    private bool isOrganSoldConditionMet;
    private AudioSource audioSource;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    string GetDaysRemainingText()
    {
        int dayEnded = GameManager.DayEnded;
        int daysRemaining = 0;

        switch (dayEnded)
        {
            case 0:
                daysRemaining = 31;
                break;
            case 1:
                daysRemaining = 30;
                break;
            case 2:
                daysRemaining = 14;
                break;
            case 3:
                daysRemaining = 1;
                break;
            default:
                daysRemaining = 0; // 예외 처리
                break;
        }

        return $"D-{daysRemaining}";
    }

    IEnumerator FadeInText(TextMeshProUGUI text, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Clamp01(timer / duration);
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            yield return null;
        }
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1f); // 확실히 100%
    }

    // **[추가]** CanvasGroup을 페이드인 시키는 코루틴
    IEnumerator FadeInPanel(GameObject panel, float duration)
    {
        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = panel.AddComponent<CanvasGroup>();
        }
        
        canvasGroup.alpha = 0f;
        panel.SetActive(true);
        float timer = 0f;
        
        while (timer < duration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(timer / duration);
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    // **[추가]** CanvasGroup을 페이드아웃 시키는 코루틴
    IEnumerator FadeOutPanel(GameObject panel, float duration, System.Action onComplete = null)
    {
        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();
        if (canvasGroup == null) yield break;

        float timer = duration;
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(timer / duration);
            yield return null;
        }
        
        panel.SetActive(false);
        canvasGroup.alpha = 1f; // 다음 활성화를 위해 리셋
        onComplete?.Invoke();
    }

    void Start()
    {
        HeadlinePanel.SetActive(true);
        NewsPanel.SetActive(false);

         // FinalPopupPanel은 시작 시 비활성화합니다.
        if (FinalPopupPanel != null)
        {
            FinalPopupPanel.SetActive(false);
            if(FinalPopupPanel.GetComponent<CanvasGroup>() == null) FinalPopupPanel.AddComponent<CanvasGroup>();
        }

        if (DayPanel != null)
        {
            DayPanel.SetActive(false);
            if (DayPanel.GetComponent<CanvasGroup>() == null) DayPanel.AddComponent<CanvasGroup>();
            if (DayText != null)
            {
                // DayText는 처음에는 투명하게 설정
                DayText.color = new Color(DayText.color.r, DayText.color.g, DayText.color.b, 0f);
            }
        }
        
        // 🚨 조건 확인 및 저장
        // GameManager는 정적 클래스이므로 직접 접근합니다.
        isOrganSoldConditionMet = (GameManager.isOrganSold == true && GameManager.DayEnded == 3);

        if (headlineTypingEffect != null)
        {
            headlineTypingEffect.OnTypingComplete += HandleHeadlineTypingComplete;
        }
        
        ProceedToHeadlines();

        //headlineTypingEffect.StartTyping(headlineTypingEffect.GetComponent<TMP_Text>().text);
    }

    void OnDestroy()
    {
        if (headlineTypingEffect != null)
        {
            headlineTypingEffect.OnTypingComplete -= HandleHeadlineTypingComplete;
        }
    }

    void HandleHeadlineTypingComplete()
    {
        // 현재 단계가 헤드라인 단계(0)일 때만 오디오를 멈춥니다.
        if (currentPhase == 0)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
                Debug.Log("Headline typing complete. Audio stopped.");
            }
        }
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
            if (audioSource.clip != null && !audioSource.isPlaying)
            {
                audioSource.Play();
            }

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
                        ActivateFinalPanel(); // 조건 충족 -> Final Panel
                    }
                    else
                    {
                        TransitionToDayPanel(); // 조건 미충족 -> Day Panel
                    }
                }
            }
        }
        else if (currentPhase == 2) // 🚨 최종 팝업 대기 단계
        {
            // **[수정]** 최종 패널 페이드아웃 후 Day Panel로 전환
            StopAllCoroutines(); // 혹시 모를 페이드인 코루틴 정지
            StartCoroutine(FadeOutPanel(FinalPopupPanel, fadeDuration, TransitionToDayPanel));
        }
        else if (currentPhase == 3) // **[추가]** Day 패널 단계
        {
            // **[추가]** Day 패널 페이드아웃 후 다음 씬으로 전환
            StopAllCoroutines(); // 혹시 모를 페이드인 코루틴 정지
            StartCoroutine(FadeOutPanel(DayPanel, fadeDuration, () => SceneManager.LoadScene(nextSceneName)));
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
            StartCoroutine(FadeInPanel(FinalPopupPanel, fadeDuration));
            
            Debug.Log("[SceneFlowManager] Final Popup Panel activated. Waiting for spacebar input to load next scene.");
        }
        else
        {
            TransitionToDayPanel();
        }
    }
    void TransitionToDayPanel()
    {
        if (DayPanel != null && DayText != null)
        {
            currentPhase = 3; // Day 패널 단계로 전환
            NewsPanel.SetActive(false);
            
            DayPanel.SetActive(true);
            
            // 텍스트 설정 및 페이드인 시작
            DayText.text = GetDaysRemainingText();
            StartCoroutine(FadeInText(DayText, fadeDuration));
            
            Debug.Log("[SceneFlowManager] Day Panel activated. Text: " + DayText.text);
        }
        else
        {
            // DayPanel이 없으면 다음 씬으로 바로 이동
            SceneManager.LoadScene(nextSceneName);
        }
    }
}