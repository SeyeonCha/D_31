using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using Vimeo;

public class ReelsTitle : MonoBehaviour, IPointerClickHandler
{
    public ReelsData data; // 해당 제목에 해당하는 데이터
    private TextMeshProUGUI titleText; // 제목 텍스트

    [Header("Reels UI Components")]
    // 인스펙터에서 할당할 미리보기 이미지
    public Image reelsPreviewImage;

    public GameObject post_panel; // 릴스 패널
    public GameObject ScrapButton; // 릴스 스크랩 버튼

    private bool isImageLoaded = false; 

    private Texture2D downloadedTexture = null; 

    private string GetDayFolderName()
    {
        int dayIndex = GameManager.DayEnded;
        switch (dayIndex)
        {
            case 0: return "Reels_D-31";
            case 1: return "Reels_D-30";
            case 2: return "Reels_D-14";
            case 3: return "Reels_D-4";
            default: return "Reels_D-31"; 
        }
    }

    void Start()
    {
        titleText = GetComponent<TextMeshProUGUI>(); // 제목 텍스트 찾기

        // 자식 텍스트 찾았는지 확인
        if (titleText == null)
        {
            Debug.LogError("자식 오브젝트에서 TextMeshProUGUI 컴포넌트를 찾을 수 없습니다.", this);
            return;
        }
    }

    void Update()
    {
        // 자식 텍스트를 해당 data의 title로 변경
        if (titleText != null && data != null)
        {
            titleText.text = data.title; 
            // Debug.Log("텍스트 바꾸기 완료. ");

            // 이미지 로드 (한 번만 실행)
            if (reelsPreviewImage != null && !isImageLoaded)
            {
                // 이미지 로드 코루틴 시작
                StartCoroutine(LoadThumbnailImage()); 
                isImageLoaded = true;
            }
        }
        else if (data == null)
        {
            Debug.Log("데이터를 왜 못찾냐..");
        }
    }

    private void OnDestroy()
    {
        if (downloadedTexture != null)
        {
            // Sprite가 사용하는 텍스처를 파괴하여 메모리를 해제합니다.
            Destroy(downloadedTexture);
            downloadedTexture = null;
        }
    }

    private IEnumerator LoadThumbnailImage()
    {
        // 1. VimeoDataManager 로드 완료 대기
        // 데이터가 로드되지 않았다면 대기합니다.
        yield return new WaitUntil(() => VimeoDataManager.Instance != null && VimeoDataManager.Instance.IsDataLoaded);

        if (data == null || string.IsNullOrEmpty(data.title))
        {
            Debug.LogError("[ReelsTitle] Data or Title is missing for thumbnail lookup.");
            yield break;
        }
        
        string folderName = GetDayFolderName();
        string videoTitle = data.title;
        
        // 2. 썸네일 URL 조회
        string thumbnailUrl = VimeoDataManager.Instance.GetVideoThumbnailUrl(folderName, videoTitle);

        if (string.IsNullOrEmpty(thumbnailUrl))
        {
            Debug.LogWarning($"[ReelsTitle] Thumbnail URL not found for '{videoTitle}' in folder '{folderName}'.");
            reelsPreviewImage.enabled = false;
            yield break;
        }

        // 3. UnityWebRequestTexture를 사용하여 이미지 다운로드
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(thumbnailUrl))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"[ReelsTitle] Failed to download thumbnail from {thumbnailUrl}. Error: {www.error}");
                reelsPreviewImage.enabled = false;
            }
            else
            {
                // 다운로드 성공 시 Sprite 생성 및 할당
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                // Sprite.Create(Texture2D texture, Rect rect, Vector2 pivot, float pixelsPerUnit)
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero, 100f);
                
                if (reelsPreviewImage != null)
                {
                    reelsPreviewImage.sprite = sprite;
                    reelsPreviewImage.enabled = true;
                    Debug.Log($"[ReelsTitle] Thumbnail loaded successfully for '{videoTitle}'.");
                }
            }
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
            else {
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
        // UI 업데이트
    }
}
