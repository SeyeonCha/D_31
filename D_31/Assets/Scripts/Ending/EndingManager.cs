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
        public Sprite spriteA; // GameManager.isOrganSold == true (장기 매매 O) 일 때 사용
        public Sprite spriteB; // GameManager.isOrganSold == false (장기 매매 X) 일 때 사용
    }

    [Header("Dependencies")]
    public FinalManager finalManager;
    public TypingEffect dialogueTypingEffect;
    public Image imageObjectA; // A 이미지 컴포넌트 연결 (true일 때 활성화)
    public Image imageObjectB; // B 이미지 컴포넌트 연결 (false일 때 활성화)

    // 비율 유지를 위해 AspectRatioFitter 컴포넌트 참조 추가 (각각 A, B 이미지용)
    private AspectRatioFitter aspectRatioFitterA;
    private AspectRatioFitter aspectRatioFitterB;

    [Header("Scenario Settings")]
    public ScenarioVisual[] scenarioVisuals;

    [Header("Next Action")]
    public bool isFinalEnding = false;

    private int sentenceIndex = 0;
    private bool isScenarioActive = false;

    // --- Awake에서 AspectRatioFitter 준비 ---
    void Awake()
    {
        // A 이미지 AspectRatioFitter 준비
        SetupAspectRatioFitter(imageObjectA, ref aspectRatioFitterA);
        // B 이미지 AspectRatioFitter 준비
        SetupAspectRatioFitter(imageObjectB, ref aspectRatioFitterB);
    }

    // AspectRatioFitter 설정을 위한 헬퍼 함수
    private void SetupAspectRatioFitter(Image imageObject, ref AspectRatioFitter aspectRatioFitter)
    {
        if (imageObject != null)
        {
            // Image 오브젝트에 AspectRatioFitter가 없으면 추가
            aspectRatioFitter = imageObject.gameObject.GetComponent<AspectRatioFitter>();
            if (aspectRatioFitter == null)
            {
                aspectRatioFitter = imageObject.gameObject.AddComponent<AspectRatioFitter>();
            }

            // Aspect Ratio Fitter 모드를 'Fit In Parent'로 설정
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

            // --- 1. 이미지 선택 및 변경 로직 (수정된 부분) ---

            // GameManager의 isOrganSold 상태 확인 및 이미지 분기
            bool isOrganSold = GameManager.isOrganSold;

            // --- 디버그 로그 추가 시작 ---
            Debug.Log($"[Ending {gameObject.name}] Phase: {sentenceIndex}, isOrganSold: {isOrganSold}");
            if (isOrganSold)
            {
                Debug.Log($"SpriteA (Used): {currentVisual.spriteA}");
            }
            else
            {
                Debug.Log($"SpriteB (Used): {currentVisual.spriteB}");
            }
            // --- 디버그 로그 추가 끝 ---

            if (isOrganSold)
            {
                // 장기 매매 O (true) : spriteA를 imageObjectA에 표시하고 B는 비활성화
                UpdateImage(imageObjectA, currentVisual.spriteA, aspectRatioFitterA, "imageObjectA (OrganSold=True)");
                UpdateImage(imageObjectB, null, aspectRatioFitterB, "imageObjectB (Inactive)");
            }
            else
            {
                // 장기 매매 X (false) : spriteB를 imageObjectB에 표시하고 A는 비활성화
                UpdateImage(imageObjectA, null, aspectRatioFitterA, "imageObjectA (Inactive)");
                UpdateImage(imageObjectB, currentVisual.spriteB, aspectRatioFitterB, "imageObjectB (OrganSold=False)");
            }

            // --- 2. 텍스트 재생 로직 ---
            dialogueTypingEffect.StartTyping(currentVisual.sentence);
        }
    }

    // 이미지 업데이트를 위한 헬퍼 함수: 스프라이트를 설정하고 비율을 조정하며, null이면 비활성화 처리
    private void UpdateImage(Image imageObject, Sprite sprite, AspectRatioFitter fitter, string debugName)
    {
        if (imageObject != null)
        {
            if (sprite != null)
            {
                imageObject.sprite = sprite;
                // 스프라이트가 null이 아니면 이미지 활성화
                imageObject.gameObject.SetActive(true);

                // --- 디버그 로그 추가 (활성화 직후 상태 확인) ---
                Debug.Log($"[DEBUG] {debugName} -> Sprite Set, GameObject Active: {imageObject.gameObject.activeSelf}");

                if (fitter != null)
                {
                    // 스프라이트의 가로/세로 비율 계산 및 적용
                    float aspectRatio = sprite.rect.width / sprite.rect.height;
                    fitter.aspectRatio = aspectRatio;
                }
            }
            // else
            // {
            //     // 스프라이트가 없으면 이미지 비활성화
            //     //imageObject.gameObject.SetActive(false);
            //     //imageObject.sprite = null;
            // }
        }
        else
        {
            Debug.LogWarning(gameObject.name + ": " + debugName + " 필드에 Image 컴포넌트가 연결되지 않았습니다.");
        }
    }


    // HandleSpacebarPress와 FinishScenario는 이전과 동일
    void HandleSpacebarPress()
    {
        if (sentenceIndex >= scenarioVisuals.Length)
        {
            FinishScenario();
            return;
        }
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