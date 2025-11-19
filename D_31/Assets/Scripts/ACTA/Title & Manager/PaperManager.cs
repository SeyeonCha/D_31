using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class PaperManager : MonoBehaviour
{
    [SerializeField]
    private DisplayScrapData displayer;

    private PaperTitle sourceTitle;

    private int classId;
    private int clicked;

    public TextMeshProUGUI title;
    public TextMeshProUGUI author;
    public TextMeshProUGUI year;
    public TextMeshProUGUI AI_T;
    public TextMeshProUGUI AI_F; 

    
    [Header("Paper Image Settings")] // 👈 추가함!
    // 'Image_paper' 프리팹을 할당할 필드
    public GameObject imagePrefab; 
    // 논문 이미지들이 생성될 부모 오브젝트 (Content 아래 RectTransform)
    public RectTransform imageContainer; 
    
    // 이미지 파일이 Resources 폴더 아래에 위치하는 기본 경로
    private const string BASE_PATH_PREFIX = "Paper_image";
    
    public void GetSourceTitle(PaperTitle stitle)
    {
        // 데이터의 정보 받아오기
        sourceTitle = stitle;
        if (sourceTitle == null)
        {
            Debug.Log($"논문 sourceTitle X");
        }
        else{
            Debug.Log($"논문 sourceTitle O : {sourceTitle.data.title},{sourceTitle.data.isScrapped}");
        }
        // sourceData = sourceTitle.data;
        classId = sourceTitle.data.classId;
        clicked = sourceTitle.data.isScrapped;

        author.text = sourceTitle.data.author;
        title.text = sourceTitle.data.title;
        year.text = sourceTitle.data.year.ToString();
        AI_T.text = "[AI 요약봇]이 논문을 요약했습니다. \n" + sourceTitle.data.AI_T.Replace("<n>","\n");
        // AI_F.text = sourceTitle.data.AI_F.Replace("<n>","\n");

        // 논문 이미지 로드 및 생성
        LoadPaperImages(sourceTitle.data.uniqueId);

        // // 이미지와 텍스트가 동적으로 추가되었으므로 레이아웃 강제 업데이트
        // if (imageContainer != null)
        // {
        //     LayoutRebuilder.ForceRebuildLayoutImmediate(imageContainer);
            
        //     // 이미지 컨테이너의 부모 (스크롤 뷰의 Content)도 업데이트해야 할 수도 있습니다.
        //     // 여기서는 imageContainer 자체가 Content 역할을 한다고 가정합니다.
        //     // 만약 상위 RectTransform이 있다면 추가적인 Rebuilder 호출이 필요할 수 있습니다.
        // }
    }

     private string GetDayFolderName()
    {
        int dayIndex = GameManager.DayEnded;
        switch (dayIndex)
        {
            case 0: return "Paper_D-31";
            case 1: return "Paper_D-30";
            case 2: return "Paper_D-14";
            case 3: return "Paper_D-4";
            default: return "Paper_D-31"; // 정의되지 않은 DayEnded 값에 대한 기본값
        }
    }

    /// 기존에 생성된 이미지 오브젝트를 모두 제거
    private void ClearImages()
    {
        if (imageContainer == null) return;

        // 컨테이너의 모든 자식 오브젝트를 파괴
        for (int i = imageContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(imageContainer.GetChild(i).gameObject);
        }
    }

    /// 논문의 uniqueId를 기반으로 이미지를 로드하고 생성
    private void LoadPaperImages(int uniqueId)
    {
        ClearImages();

        string folderName = GetDayFolderName();
        string imageBasePath = $"{BASE_PATH_PREFIX}/{folderName}";
        
        string unindexedPath = $"{imageBasePath}/{uniqueId}";
        Sprite unindexedSprite = Resources.Load<Sprite>(unindexedPath);

        if (unindexedSprite != null)
        {
            // 단일 파일 로드 성공 (예: "2")
            InstantiateImage(unindexedSprite, unindexedPath);
            Debug.Log($"[PaperManager] Single image loaded successfully for ID {uniqueId} from: {unindexedPath}");
            return; // 단일 파일이므로 여기서 종료
        }

        // 2. Indexed 파일 로드 시도 (예: "1_1", "1_2", ...)
        bool foundIndexedImage = false;
        for (int index = 1; index < 20; index++) // 최대 20장까지 로드 시도
        {
            string fileName = $"{uniqueId}_{index}";
            string imagePath = $"{imageBasePath}/{fileName}";
            Sprite loadedSprite = Resources.Load<Sprite>(imagePath);

            if (loadedSprite != null)
            {
                InstantiateImage(loadedSprite, imagePath);
                foundIndexedImage = true;
            }
            else
            {
                // 로드 실패 시: 연속된 인덱스 파일이 끝났다고 판단
                if (index == 1 && !foundIndexedImage)
                {
                    Debug.LogWarning($"[PaperManager] No images found for ID {uniqueId}. Checked: {unindexedPath} and {imagePath}");
                }
                else if (foundIndexedImage)
                {
                    Debug.Log($"[PaperManager] Finished loading indexed images for ID {uniqueId}. Total: {index - 1} images.");
                }
                break;
            }
        }
    }
    
    /// 로드된 Sprite를 사용하여 Image_paper 프리팹을 생성하고 설정
    private void InstantiateImage(Sprite sprite, string path)
    {
        if (imagePrefab == null || imageContainer == null)
        {
            Debug.LogError("[PaperManager] Image Prefab or Image Container is not assigned.");
            return;
        }

        // 프리팹 생성
        GameObject imageObject = Instantiate(imagePrefab, imageContainer);
        Image imgComponent = imageObject.GetComponent<Image>();

        if (imgComponent != null)
        {
            imgComponent.sprite = sprite;
            //imgComponent.SetNativeSize(); // 원본 이미지 크기로 설정 (필요에 따라 LayoutGroup 설정에 맞게 변경 가능)
            
            // 만약 이미지 크기가 컨테이너 너비를 초과한다면, 여기서 크기 조절 로직을 추가해야 합니다.
            // 예를 들어, RectTransform의 width를 부모 너비에 맞추고 height는 비례하게 설정
            // (Layout Group을 사용하는 경우 SetNativeSize() 대신 레이아웃 설정을 따릅니다.)
        }
        else
        {
            Debug.LogError($"[PaperManager] Image_paper prefab is missing an Image component. Path: {path}");
            Destroy(imageObject);
        }
    }
    
    // 스크랩 버튼이 눌리면 불러와질 함수 정의
    public void ScrapButtonClicked()
    {
        if (sourceTitle.data.isScrapped == 0)
        {
            sourceTitle.data.isScrapped = 1;
            Debug.Log($"SCRAPPED : {sourceTitle.data.isScrapped}, class : {classId}");

            displayer.ScrapCounter(classId);
        }

    }
}
