using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Vimeo.Player;
using Vimeo;

public class ReelsManager : MonoBehaviour
{
    [SerializeField]
    private DisplayScrapData displayer;

    private ReelsTitle sourceTitle;

    private int classId;
    private int clicked;
    private int uniqueId;

    [Header("Vimeo Player & UI")]
    public VimeoPlayer vimeoPlayer;

    public TextMeshProUGUI title1;
    public TextMeshProUGUI title2;
    public TextMeshProUGUI youtuber;
    public TextMeshProUGUI subs;
    public TextMeshProUGUI views;
    public TextMeshProUGUI like;     

    // 댓글 UI (이름, 내용, 좋아요수)
    public Transform CommentParent; // 부모 오브젝트
    public GameObject CommentPrefab; // 댓글 프리팹

    private List<ReelsCommentData> comments;

    private void Awake()
    {
        // comments = new List<List<TextMeshProUGUI>>() {comment1, comment2, comment3, comment4, comment5};
        
    }

    public void GetSourceTitle(ReelsTitle stitle)
    {
        ClearComments();
        // 데이터의 정보 받아오기
        sourceTitle = stitle;

        // sourceData = sourceTitle.data;
        classId = sourceTitle.data.classId;
        clicked = sourceTitle.data.isScrapped;
        uniqueId = sourceTitle.data.uniqueId;

        // 비디오 ID를 데이터 관리자에서 조회하고 로드합니다.
        StartCoroutine(WaitForVimeoDataAndLoad());

        // 패널 UI 텍스트들 채우기
        if (youtuber == null) {
            Debug.Log("리포터 UI가 없음");
        }
        else{
            Debug.Log($"리포터 데이터 있긴함 : {sourceTitle.data.youtuber}");
            youtuber.text = "@" + sourceTitle.data.youtuber;
        }
        title1.text = sourceTitle.data.title;
        title2.text = sourceTitle.data.title;
        subs.text = "구독자 " + sourceTitle.data.subs.ToString();
        views.text = "조회수 " + sourceTitle.data.views.ToString() + "회";
        like.text = sourceTitle.data.like.ToString(); 

        // 댓글 UI 채우기
        comments = sourceTitle.data.comments;
        int c_num = sourceTitle.data.c_num;

        for (int i = 0; i<c_num; i++) // 댓글 개수만큼 반복
        {   
            ReelsCommentData comment = comments[i]; // 소스 타이틀의 i번째 CommentData

            GameObject clone = Instantiate(CommentPrefab, CommentParent); // 댓글 프리팹 생성
            ReelsCommentHandler commentUI = clone.GetComponent<ReelsCommentHandler>(); // 댓글 프리팹에 붙은 핸들러 가져오기
            commentUI.SetupUI(comment);
        }
    }

    // GameManager.DayEnded 값에 따라 폴더 이름 (프로젝트 이름) 반환
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

    // VimeoDataManager 로드를 기다린 후, 저장된 데이터에서 ID를 찾아 영상 로드
    private IEnumerator WaitForVimeoDataAndLoad()
    {
        // VimeoDataManager 인스턴스가 생성되고 데이터 로드가 완료될 때까지 기다립니다.
        // VimeoDataManager가 싱글톤 패턴으로 초기화되도록 보장합니다.
        yield return new WaitUntil(() => VimeoDataManager.Instance != null && VimeoDataManager.Instance.IsDataLoaded);

        string targetTitle = uniqueId.ToString();
        string targetFolderName = GetDayFolderName();

        // VimeoDataManager에서 영상 제목과 폴더 이름으로 ID를 조회합니다.
        int videoId = VimeoDataManager.Instance.GetVideoId(targetFolderName, targetTitle);

        if (videoId > 0)
        {
            LoadVimeoVideo(videoId);
        }
        else
        {
            Debug.LogError($"[ReelsManager] ID Not Found: Folder '{targetFolderName}', Title '{targetTitle}'. Cannot load video.");
        }
    }
    
    // Vimeo ID를 사용하여 영상 로드
    private void LoadVimeoVideo(int videoId)
    {
        if (vimeoPlayer == null)
        {
            Debug.LogError("[ReelsManager] Vimeo Player component is not assigned.");
            return;
        }

        vimeoPlayer.LoadVideo(videoId);
        Debug.Log($"[ReelsManager] Load request sent for ID: {videoId}");
    }

    private void ClearComments()
    {
        if (CommentParent == null) return;

        // 컨테이너의 모든 자식 오브젝트를 파괴
        for (int i = CommentParent.childCount - 1; i >= 0; i--)
        {
            Destroy(CommentParent.GetChild(i).gameObject);
        }
    }
    
    // 스크랩 버튼이 눌리면 불러와질 함수 정의
    public void ScrapButtonClicked()
    {
        sourceTitle.data.isScrapped = 1;
        Debug.Log($"SCRAPPED : {sourceTitle.data.isScrapped}, class : {classId}");

        displayer.ScrapCounter(classId);

    }
}
