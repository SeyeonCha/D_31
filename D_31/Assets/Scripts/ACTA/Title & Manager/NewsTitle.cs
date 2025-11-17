using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// 제목 버튼 컴포넌트로 붙여서, 해당 제목의 데이터를 저장할거임. 

public class NewsTitle : MonoBehaviour, IPointerClickHandler
{
    public NewsData data; // 해당 제목에 해당하는 데이터
    private TextMeshProUGUI titleText; // 제목 텍스트
    // public NewsManager newsManager;
    public Image newsPreviewImage;

    public GameObject post_panel; // 뉴스 패널
    public GameObject ScrapButton; // 뉴스 스크랩 버튼

    private bool isImageLoaded = false; 
    private const string IMAGE_BASE_PATH = "news_image";


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

    private string GetImageBasePath()
    {
        int dayIndex = GameManager.DayEnded;
        string folderName;
        
        switch (dayIndex)
        {
            case 0: folderName = "News_D-31"; break;
            case 1: folderName = "News_D-30"; break;
            case 2: folderName = "News_D-14"; break;
            case 3: folderName = "News_D-4"; break;
            default: folderName = "News_D-31"; break; // 기본값
        }

        return $"{IMAGE_BASE_PATH}/{folderName}";
    }

    void Update()
    {
        if (data != null)
        {
            // 1. 제목 텍스트 설정
            if (titleText != null)
            {
                titleText.text = data.title; 
            }

            // 2. 이미지 설정
            if (newsPreviewImage != null && !isImageLoaded)
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
            NewsManager newsManager = post_panel.GetComponent<NewsManager>();
            if (newsManager == null) 
            {
                Debug.Log("뉴스 메니저 못찾음");
            }
            else {
                Debug.Log("뉴스 매니저 찾음");

                // 패널 켜기
                post_panel.SetActive(true);

                // 패널 UI 채우기
                newsManager.GetSourceTitle(this);
                
                // 스크랩 버튼 설정
                ScrapButtonHandler ScrapButtonHandler = ScrapButton.GetComponent<ScrapButtonHandler>();
                ScrapButtonHandler.GetSourceTitle(data.isScrapped, data.classId, data.uniqueId); // 👈 수정함!
            }
            
        }
    }

    // NewsData의 ID를 기반으로 이미지를 로드하여 UI에 설정하는 함수
    private void LoadImage(int imageId)
    {
        string fullFolderPath = GetImageBasePath();
        
        string imagePath = $"{fullFolderPath}/{imageId}";
        
        // Resources.Load를 사용하여 Sprite 로드
        Sprite loadedSprite = Resources.Load<Sprite>(imagePath);

        if (loadedSprite != null)
        {
            newsPreviewImage.sprite = loadedSprite;
            newsPreviewImage.enabled = true; // 이미지 설정 후 활성화
            Debug.Log($"[NewsImage] Preview image loaded successfully for ID {imageId} from: {imagePath}");
        }
        else
        {
            // 로드 실패 시
            newsPreviewImage.enabled = false;
            Debug.LogError($"[NewsImage] Failed to load Preview Image for ID {imageId}. " + 
                           $"Please ensure the image file is named '{imageId}' and is located under Resources/{IMAGE_BASE_PATH}.");
        }
    }


}
