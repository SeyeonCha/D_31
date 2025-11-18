using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdImageLoader : MonoBehaviour
{
    // 메인 광고 이미지 필드
    [Header("Main Advertisement Images")]
    [Tooltip("광고 이미지를 표시할 UI Image 컴포넌트 3개를 할당하세요.")]
    public Image adImage1;
    public Image adImage2;
    public Image adImage3;
    
    // 팝업 광고 필드
    [Header("Popup Advertisement")]
    [Tooltip("팝업 전체 패널 GameObject를 할당하세요.")]
    public GameObject popupPanel;
    [Tooltip("팝업 내 이미지를 표시할 Image 컴포넌트를 할당하세요.")]
    public Image popupImage;
    
    private const string POPUP_AD_PATH = "AD_image/AD_popup";

    private const string AD_BASE_PATH = "AD_image";
    private readonly List<string> adFileNames = new List<string> { "ad1", "ad2", "ad3" };

    void Start()
    {
        // 씬 로드 시 메인 광고 이미지를 로드합니다.
        LoadAdvertisements();

        // 팝업 광고를 랜덤 시간 후 활성화하도록 설정합니다.
        SetupPopupAdTimer();
    }
    
    /// GameManager.DayEnded 값에 따라 메인 광고 폴더 이름을 반환합니다.
    private string GetDayFolderName()
    {
        int dayIndex = GameManager.DayEnded; 
        switch (dayIndex)
        {
            case 0: return "AD_D-31";
            case 1: return "AD_D-30";
            case 2: return "AD_D-14";
            case 3: return "AD_D-4";
            default: return "AD_D-31";
        }
    }

    /// 현재 DayEnded에 맞는 광고 이미지를 Resources에서 로드하여 UI에 할당
    private void LoadAdvertisements()
    {
        string folderName = GetDayFolderName();
        
        // 이미지를 할당할 Image 컴포넌트 목록
        List<Image> adImages = new List<Image> { adImage1, adImage2, adImage3 };

        for (int i = 0; i < adFileNames.Count; i++)
        {
            Image targetImage = adImages[i];
            
            if (targetImage == null)
            {
                Debug.LogWarning($"[AdImageLoader] adImage{i+1} is not assigned in the Inspector. Skipping.");
                continue;
            }

            string fileName = adFileNames[i];
            // 최종 경로 예: "AD_image/AD_D-31/ad1"
            string imagePath = $"{AD_BASE_PATH}/{folderName}/{fileName}";
            
            // Resources.Load를 사용하여 Sprite 로드
            Sprite loadedSprite = Resources.Load<Sprite>(imagePath);

            if (loadedSprite != null)
            {
                targetImage.sprite = loadedSprite;
                targetImage.enabled = true;
                Debug.Log($"[AdImageLoader] Ad Image loaded successfully from: {imagePath}");
            }
            else
            {
                targetImage.enabled = false;
                Debug.LogError($"[AdImageLoader] Failed to load Ad Image from: {imagePath}. " + 
                               $"Please ensure the image file '{fileName}' is located under Assets/Resources/{AD_BASE_PATH}/{folderName}.");
            }
        }
    }
    
    /// 팝업 광고 활성화 타이머를 설정
    private void SetupPopupAdTimer()
    {
        if (popupPanel == null)
        {
            Debug.LogError("[AdImageLoader] Popup Panel is not assigned. Cannot set timer.");
            return;
        }

        // 팝업을 기본적으로 비활성화
        popupPanel.SetActive(false);

        // 5초에서 15초 사이의 랜덤 시간을 설정
        float randomDelay = Random.Range(10f, 40f);
        
        // 랜덤 시간 후에 ShowPopupAd 함수를 호출
        Invoke("ShowPopupAd", randomDelay);
        Debug.Log($"[AdImageLoader] Popup Ad will show in {randomDelay:F2} seconds.");
    }

    /// 팝업 광고를 활성화하고 해당 날짜의 이미지를 로드
    private void ShowPopupAd()
    {
        if (popupPanel == null || popupImage == null)
        {
            Debug.LogError("[AdImageLoader] Popup Panel or Image component is missing. Aborting popup show.");
            return;
        }

        // 1. 이미지 로드
        LoadPopupAdImage();
        
        // 2. 팝업 패널 활성화
        popupPanel.SetActive(true);
        Debug.Log("[AdImageLoader] Popup Ad activated.");
    }
    
    /// DayEnded 값에 따라 팝업 이미지를 로드
    private void LoadPopupAdImage()
    {
        if (popupImage == null) return;

        // DayEnded 값을 파일 이름으로 사용합니다 (0, 1, 2, 3...)
        string fileName = GameManager.DayEnded.ToString();
        // 최종 경로 예: "AD_image/AD_popup/0" (유니티는 확장자를 생략함)
        string imagePath = $"{POPUP_AD_PATH}/{fileName}";
        
        Sprite loadedSprite = Resources.Load<Sprite>(imagePath);

        if (loadedSprite != null)
        {
            popupImage.sprite = loadedSprite;
            popupImage.enabled = true;
            Debug.Log($"[AdImageLoader] Popup Image loaded successfully from: {imagePath}");
        }
        else
        {
            popupImage.enabled = false;
            Debug.LogError($"[AdImageLoader] Failed to load Popup Image from: {imagePath}. " + 
                           $"Please ensure the image file '{fileName}.png' is located under Assets/Resources/{POPUP_AD_PATH}.");
        }
    }
    
    // 팝업을 닫기 위한 공용 함수 (버튼에 연결)
    public void ClosePopupAd()
    {
        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
            Debug.Log("[AdImageLoader] Popup Ad closed.");
        }
    }
}