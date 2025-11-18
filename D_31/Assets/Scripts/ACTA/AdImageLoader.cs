using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdImageLoader : MonoBehaviour
{
    // 인스펙터에서 할당할 3개의 광고 이미지 컴포넌트
    [Header("Advertisement Images")]
    [Tooltip("광고 이미지를 표시할 UI Image 컴포넌트 3개를 할당하세요.")]
    public Image adImage1;
    public Image adImage2;
    public Image adImage3;
    
    // 이미지 파일이 Resources 폴더 아래에 위치하는 기본 경로
    private const string AD_BASE_PATH = "AD_image";
    
    // 로드할 이미지 파일 이름 목록
    private readonly List<string> adFileNames = new List<string> { "ad1", "ad2", "ad3" };

    void Start()
    {
        // 씬 로드 시 광고 이미지 로드를 시작합니다.
        LoadAdvertisements();
    }
    
    /// <summary>
    /// GameManager.DayEnded 값에 따라 폴더 이름을 반환합니다.
    /// 0 -> AD_D-31, 1 -> AD_D-30, 2 -> AD_D-14, 3 -> AD_D-4
    /// </summary>
    private string GetDayFolderName()
    {
        // GameManager는 정적 클래스이므로 직접 접근합니다.
        int dayIndex = GameManager.DayEnded; 
        switch (dayIndex)
        {
            case 0: return "AD_D-31";
            case 1: return "AD_D-30";
            case 2: return "AD_D-14";
            case 3: return "AD_D-4";
            default: return "AD_D-31"; // 정의되지 않은 DayEnded 값에 대한 기본값
        }
    }

    /// <summary>
    /// 현재 DayEnded에 맞는 광고 이미지를 Resources에서 로드하여 UI에 할당합니다.
    /// </summary>
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
}