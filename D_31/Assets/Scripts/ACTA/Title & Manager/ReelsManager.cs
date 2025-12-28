using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class ReelsManager : MonoBehaviour
{
    [SerializeField]
    private DisplayScrapData displayer;

    private ReelsTitle sourceTitle;

    private int classId;
    private int clicked;
    private int uniqueId;

    [Header("Video Player & UI")]
    public VideoPlayer videoPlayer;

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

    public void GetSourceTitle(ReelsTitle stitle)
    {
        ClearComments();
        // 데이터의 정보 받아오기
        sourceTitle = stitle;

        // sourceData = sourceTitle.data;
        classId = sourceTitle.data.classId;
        clicked = sourceTitle.data.isScrapped;
        uniqueId = sourceTitle.data.uniqueId;

        // 리소스 폴더에서 영상을 찾아 재생합니다.
        LoadVideoFromResources();

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

    private void LoadVideoFromResources()
    {
        if (videoPlayer == null)
        {
            Debug.LogError("[ReelsManager] Video Player가 할당되지 않았습니다.");
            return;
        }

        string folderName = GetDayFolderName();
        // 경로 예시: Reels_video/Reels_D-4/1
        // Resources.Load는 확장자를 적지 않습니다.
        string resourcePath = $"Reels_video/{folderName}/{uniqueId}";

        VideoClip clip = Resources.Load<VideoClip>(resourcePath);

        if (clip != null)
        {
            videoPlayer.clip = clip;
            videoPlayer.Play();
            Debug.Log($"[ReelsManager] 영상 재생 시작: {resourcePath}");
        }
        else
        {
            Debug.LogError($"[ReelsManager] 영상을 찾을 수 없습니다: {resourcePath}");
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
        if (sourceTitle.data.isScrapped == 0)
        {
            sourceTitle.data.isScrapped = 1;
            Debug.Log($"SCRAPPED : {sourceTitle.data.isScrapped}, class : {classId}");

            displayer.ScrapCounter(classId);
        }

    }
}
