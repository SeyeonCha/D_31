using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    // =========================================================
    // 시나리오 관련 필드
    // =========================================================
    [Header("Scenario Dialogue")]
    [Tooltip("대화 패널 전체 GameObject")]
    public GameObject dialoguePanel; 
    [Tooltip("대화 시스템을 처리하는 컴포넌트 (DialogueSystem 같은)")]
    public DialogueSystem dialogueSystem; // DialogueSystem 컴포넌트가 필요합니다.
    
    // 퀘스트 텍스트 필드를 questText1과 questText2로 명확히 분리
    [Tooltip("1차 대화 완료 후 업데이트할 퀘스트 텍스트")]
    public TextMeshProUGUI questText1;
    [Tooltip("2차 이미지 시퀀스 완료 후 업데이트할 퀘스트 텍스트")]
    public TextMeshProUGUI questText2;
    
    [Header("Scenario Data")]
    // [TextArea] 속성을 추가하여 인스펙터에서 여러 줄 편집이 용이하도록 합니다.
    [TextArea(4, 15)] 
    [Tooltip("순서대로 표시될 대화 문장들 작성하세요.")]
    public string[] scenarioSentences; 
    
    [Tooltip("1차 대화 완료 후 QuestText1에 표시될 메시지")]
    [TextArea(4, 15)]
    public string questUpdateMessage1;

    [Tooltip("2차 이미지 시퀀스 완료 후 QuestText2에 표시될 메시지")]
    [TextArea(4, 15)]
    public string questUpdateMessage2; 

    // 이미지 시퀀스 관련 필드
    [Header("Image Sequence")]
    [Tooltip("이미지를 표시할 Image 컴포넌트 (스크립트가 붙어있는 오브젝트에 있어야 함)")]
    public Image targetImage;
    
    private const string IMAGE_BASE_PATH = "cafeChat_image";
    private int currentImageIndex = 0;
    private const int MAX_IMAGE_INDEX = 9; // 'cafe_chat_9'가 마지막
    
    private bool isImageSequenceActive = false;

    
    void Awake()
    {
        // 팝업 패널은 비활성화 상태로 시작합니다.
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }

        if (targetImage == null)
        {
            targetImage = GetComponent<Image>();
        }
    }

    // Update 함수를 사용하여 스페이스바 입력을 감지하고 이미지 시퀀스를 진행합니다.
    void Update()
    {
        // 이미지 시퀀스가 활성화된 상태에서만 스페이스바 입력을 처리합니다.
        if (isImageSequenceActive && Input.GetKeyDown(KeyCode.Space))
        {
            HandleImageSequenceInput();
        }
    }

    /// 외부에서 호출하여 시나리오 대화를 시작합니다.
    public void StartChat()
    {
        StartCoroutine(StartScenarioChat());
    }

    /// 시나리오 대화를 시작하고 완료될 때까지 관리합니다.
    private IEnumerator StartScenarioChat()
    {
        // 필수 컴포넌트 점검 (questText 대신 questText1과 questText2 점검)
        if (dialoguePanel == null || dialogueSystem == null || questText1 == null || questText2 == null)
        {
            Debug.LogError("[ChatManager] Essential Dialogue components (Panel, System, Quest Text 1 or 2) are not assigned.");
            yield break;
        }

        // 1. 대화 패널 활성화
        dialoguePanel.SetActive(true);

        // 2. DialogueSystem에게 대화 시작을 요청하고 완료될 때까지 기다립니다.
        yield return dialogueSystem.StartDialogue(scenarioSentences); 

        // 3. 1차 대화 완료 후
        
        // 대화 패널 비활성화
        dialoguePanel.SetActive(false);
        
        // 퀘스트 텍스트 1차 업데이트 (questText1 사용)
        questText1.text = questUpdateMessage1;
        if (!questText1.gameObject.activeSelf)
        {
            questText1.gameObject.SetActive(true);
        }
        Debug.Log("[ChatManager] 1st Dialogue finished and Quest Text 1 updated.");
        
        // 4. 이미지 시퀀스 시작
        StartImageSequence();
    }

     /// <summary>
    /// 이미지 시퀀스를 시작하고 첫 번째 이미지를 로드합니다.
    /// </summary>
    private void StartImageSequence()
    {
        if (targetImage == null)
        {
            Debug.LogError("[ChatManager] Target Image component is not assigned for sequence.");
            return;
        }

        // 0번 이미지부터 시작
        currentImageIndex = 0;
        isImageSequenceActive = true;
        
        // 이미지 컴포넌트 활성화 (대화 패널이 꺼진 후 이미지가 보이도록)
        targetImage.gameObject.SetActive(true); 
        
        LoadNextImage();
        Debug.Log("[ChatManager] Image sequence started. Press SPACE to proceed.");
    }
    
    /// <summary>
    /// 스페이스바 입력 시 다음 이미지를 로드하거나 시퀀스를 종료합니다.
    /// </summary>
    private void HandleImageSequenceInput()
    {
        if (currentImageIndex < MAX_IMAGE_INDEX)
        {
            // 다음 인덱스로 이동
            currentImageIndex++;
            LoadNextImage();
        }
        else // 마지막 이미지 ('cafe_chat_9')를 본 후
        {
            EndImageSequence();
        }
    }

    /// <summary>
    /// 현재 DayEnded에 맞는 폴더 이름을 가져옵니다.
    /// </summary>
    private string GetDayFolderName()
    {
        int dayIndex = GameManager.DayEnded;
        switch (dayIndex)
        {
            case 0: return "Chat_D-31"; // 가정: DayEnded 0도 Chat_D-31 폴더를 사용
            case 1: return "Chat_D-30"; 
            case 2: return "Chat_D-14"; 
            case 3: return "Chat_D-4"; 
            default: return "Chat_D-31"; 
        }
    }

    /// <summary>
    /// 현재 인덱스와 DayEnded에 따라 이미지를 로드합니다.
    /// </summary>
    private void LoadNextImage()
    {
        string folderName = GetDayFolderName();
        
        // 파일 이름 형식: 'cafe_chat_N'
        string fileName = $"cafe_chat_{currentImageIndex}";
        
        // 최종 경로 예: "cafeChat_image/Chat_D-30/cafe_chat_1"
        string imagePath = $"{IMAGE_BASE_PATH}/{folderName}/{fileName}";
        
        Sprite loadedSprite = Resources.Load<Sprite>(imagePath);

        if (loadedSprite != null)
        {
            targetImage.sprite = loadedSprite;
            Debug.Log($"[ChatManager] Image loaded: {imagePath}");
        }
        else
        {
            Debug.LogError($"[ChatManager] Failed to load image from: {imagePath}. Check index {currentImageIndex}.");
            EndImageSequence(); // 로드 실패 시 강제 종료
        }
    }

    /// <summary>
    /// 이미지 시퀀스를 종료하고 2차 퀘스트 업데이트를 수행합니다.
    /// </summary>
    private void EndImageSequence()
    {
        isImageSequenceActive = false;
        targetImage.gameObject.SetActive(false);
        
        // 2차 퀘스트 텍스트 업데이트 (questText2 사용)
        if (questText2 != null)
        {
            questText2.text = questUpdateMessage2;
            if (!questText2.gameObject.activeSelf)
            {
                questText2.gameObject.SetActive(true);
            }
        }
        Debug.Log("[ChatManager] Image Sequence finished and 2nd Quest Text updated.");
    }
}