using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ReelsTitle : MonoBehaviour, IPointerClickHandler
{
    public ReelsData data; // 해당 제목에 해당하는 데이터
    private TextMeshProUGUI titleText; // 제목 텍스트

    [Header("Thumbnail Settings")]
    [Tooltip("릴스 미리보기 이미지를 표시할 Image 컴포넌트")]
    public Image reelsPreviewImage; // 릴스 미리보기 이미지 컴포넌트 추가

    public GameObject post_panel; // 릴스 패널
    public GameObject ScrapButton; // 릴스 스크랩 버튼

    private bool isImageLoaded = false;
    private const string IMAGE_BASE_PATH = "Reels_thumbnail"; // 리소스의 기본 폴더 경로


    void Start()
    {
        titleText = GetComponent<TextMeshProUGUI>(); // 제목 텍스트 찾기

        // 자식 텍스트 찾았는지 확인
        if (titleText == null)
        {
            Debug.LogError("자식 오브젝트에서 TextMeshProUGUI 컴포넌트를 찾을 수 없습니다.", this);
            return;
        }
        // Debug.Log($"제목 텍스트 텍스트 찾음 : {titleText.text}");
    }

    /// GameManager.DayEnded 값에 따라 폴더 이름을 반환
    private string GetDayFolderName()
    {
        int dayIndex = GameManager.DayEnded;
        switch (dayIndex)
        {
            case 0: return "Reels_D-31";
            case 1: return "Reels_D-30";
            case 2: return "Reels_D-14";
            case 3: return "Reels_D-4";
            default: return "Reels_D-31"; // 기본값
        }
    }

    void Update()
    {
        // 자식 텍스트를 해당 data의 title로 변경
        if (titleText != null && data != null)
        {
            titleText.text = data.title;
            // Debug.Log("텍스트 바꾸기 완료. ");

            // 썸네일 이미지 설정
            if (reelsPreviewImage != null && !isImageLoaded)
            {
                LoadImage(data.uniqueId);
                isImageLoaded = true;
            }
        }

        else if (data == null)
        {
            Debug.Log("데이터를 왜 못찾냐..");
        }
    }

    private void LoadImage(int imageId)
    {
        string folderName = GetDayFolderName();
        // 최종 경로 예: "Reels_thumbnail/Reels_D-31/1"
        string imagePath = $"{IMAGE_BASE_PATH}/{folderName}/{imageId}";
        
        // Resources.Load를 사용하여 Sprite 로드
        Sprite loadedSprite = Resources.Load<Sprite>(imagePath);

        if (reelsPreviewImage != null)
        {
            if (loadedSprite != null)
            {
                reelsPreviewImage.sprite = loadedSprite;
                reelsPreviewImage.enabled = true; // 이미지 설정 후 활성화
                Debug.Log($"[ReelsTitle] Thumbnail loaded successfully for ID {imageId} from: {imagePath}");
            }
            else
            {
                reelsPreviewImage.enabled = false;
                Debug.LogError($"[ReelsTitle] Failed to load Thumbnail for ID {imageId}. " + 
                               $"Please ensure the image file is named '{imageId}' and is located under Resources/{IMAGE_BASE_PATH}/{folderName}.");
            }
        }
        else
        {
            Debug.LogError("[ReelsTitle] reelsPreviewImage component is not assigned in the Inspector.");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 댓글도 불러오기 (프리팹 생성 & 데이터 참조해서 닉네임/내용 채워넣기)
        // data.comments
        if (post_panel == null)
        {
            Debug.Log("왜 뉴스 패널을 못찾지?");
        }
        else
        {
            ReelsManager reelsManager = post_panel.GetComponent<ReelsManager>();
            if (reelsManager == null)
            {
                Debug.Log("뉴스 메니저 못찾음");
            }
            else
            {
                Debug.Log("뉴스 매니저 찾음");

                // 패널 켜기
                post_panel.SetActive(true);

                // 패널 UI 채우기
                reelsManager.GetSourceTitle(this);

                // 스크랩 버튼 설정
                ScrapButtonHandler ScrapButtonHandler = ScrapButton.GetComponent<ScrapButtonHandler>();
                ScrapButtonHandler.GetSourceTitle(data.isScrapped, data.classId, data.uniqueId);
            }
        }
    }
}