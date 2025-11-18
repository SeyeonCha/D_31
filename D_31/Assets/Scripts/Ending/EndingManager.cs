using UnityEngine;
using TMPro;
using UnityEngine.UI; // Image 및 AspectRatioFitter 컴포넌트를 사용하기 위해 필요

public class EndingManager : MonoBehaviour
{
    // [Serializable]을 붙여야 Unity Inspector에 커스텀 구조체가 노출됩니다.
    [System.Serializable]
    public struct ScenarioVisual
    {
        [TextArea(1, 5)]
        public string sentence; 
        public Sprite sprite;   
    }

    [Header("Dependencies")]
    public FinalManager finalManager; 
    public TypingEffect dialogueTypingEffect;
    public Image imageObject; // 스프라이트를 바꿀 Image 컴포넌트 연결

    // 비율 유지를 위해 AspectRatioFitter 컴포넌트 참조 추가
    private AspectRatioFitter aspectRatioFitter; 

    [Header("Scenario Settings")]
    public ScenarioVisual[] scenarioVisuals; 

    [Header("Next Action")]
    public bool isFinalEnding = false; 

    private int sentenceIndex = 0;
    private bool isScenarioActive = false;
    
    // --- Awake 또는 Start에서 AspectRatioFitter 준비 ---
    void Awake()
    {
        if (imageObject != null)
        {
            // Image 오브젝트에 AspectRatioFitter가 없으면 추가
            aspectRatioFitter = imageObject.gameObject.GetComponent<AspectRatioFitter>();
            if (aspectRatioFitter == null)
            {
                aspectRatioFitter = imageObject.gameObject.AddComponent<AspectRatioFitter>();
            }

            // Aspect Ratio Fitter 모드를 'Fit In Parent'로 설정하는 것이 일반적
            aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
        }
    }

    // FinalManager에서 호출하여 시나리오 재생 시작 (나머지는 이전과 동일)
    public void StartEndingScenario()
    {
        if (finalManager == null)
        {
            Debug.LogError("FinalManager가 연결되지 않았습니다. Inspector에서 연결해주세요.");
            return;
        }

        isScenarioActive = true;
        sentenceIndex = 0;
        
        if (scenarioVisuals.Length > 0)
        {
            PlayCurrentSentence(); 
        }
        else
        {
            FinishScenario();
        }
    }

    void Update()
    {
        if (isScenarioActive && Input.GetKeyDown(KeyCode.Space))
        {
            HandleSpacebarPress();
        }
    }
    
    // 현재 인덱스의 문장을 출력하고 이미지 변경
    private void PlayCurrentSentence()
    {
        if (sentenceIndex < scenarioVisuals.Length)
        {
            ScenarioVisual currentVisual = scenarioVisuals[sentenceIndex];
            
            // 1. 이미지 변경 및 비율 설정 로직
            if (imageObject != null && currentVisual.sprite != null)
            {
                imageObject.sprite = currentVisual.sprite;
                
                // ✅ 수정된 부분: AspectRatioFitter를 사용하여 비율 유지
                if (aspectRatioFitter != null)
                {
                    // 스프라이트의 가로/세로 비율 계산 및 적용
                    float aspectRatio = currentVisual.sprite.rect.width / currentVisual.sprite.rect.height;
                    aspectRatioFitter.aspectRatio = aspectRatio;
                }
            }
            else if (imageObject == null)
            {
                Debug.LogWarning(gameObject.name + ": imageObject 필드에 Image 컴포넌트가 연결되지 않았습니다.");
            }
            
            // 2. 텍스트 재생 로직
            dialogueTypingEffect.StartTyping(currentVisual.sentence);
        }
    }

    // HandleSpacebarPress와 FinishScenario는 이전과 동일
    void HandleSpacebarPress()
    {
        string currentSentence = scenarioVisuals[sentenceIndex].sentence;

        if (dialogueTypingEffect.IsTyping)
        {
            dialogueTypingEffect.SkipTyping(currentSentence);
        }
        else
        {
            sentenceIndex++;
            
            if (sentenceIndex < scenarioVisuals.Length)
            {
                PlayCurrentSentence(); 
            }
            else
            {
                FinishScenario();
            }
        }
    }

    void FinishScenario()
    {
        isScenarioActive = false;
        
        if (isFinalEnding)
        {
            finalManager.GoToCreditScene();
        }
        else
        {
            finalManager.ProceedToTruth();
        }
    }
}