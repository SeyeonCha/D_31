using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ComuManager : MonoBehaviour
{
    [SerializeField]
    private DisplayScrapData displayer;
    
    private ComuTitle sourceTitle;

    private int classId;
    private int clicked;

    public TextMeshProUGUI title;
    public TextMeshProUGUI writer;
    public TextMeshProUGUI like;   
    public TextMeshProUGUI content;

    public Transform CommentParent; // 부모 오브젝트
    public GameObject CommentPrefab; // 댓글 프리팹
    public GameObject ReplyPrefab; // 대댓글 프리팹

    public List<CommentData> comments; 



    // 👇 길이 반영 딜레이 문제 때문에 아래 코드 추가됨
    // public RectTransform contentRectTransform;
    // public RectTransform newsPanelRectTransform;

    private void Awake()
    {
        // comments = new List<List<TextMeshProUGUI>>() {comment1, comment2, comment3, comment4};

    }

    public void GetSourceTitle(ComuTitle stitle)
    {
        ClearComments();
        // InitUI();
        // 데이터의 정보 받아오기
        sourceTitle = stitle;
        // sourceData = sourceTitle.data;
        classId = sourceTitle.data.classId;
        clicked = sourceTitle.data.isScrapped;

        // 패널 UI 텍스트들 채우기
        if (writer == null) {
            Debug.Log("리포터 UI가 없음");
        }
        else{
            Debug.Log($"리포터 데이터 있긴함 : {sourceTitle.data.writer}");
            writer.text = sourceTitle.data.writer;
        }
        title.text = sourceTitle.data.title;
        like.text = "공감 " + sourceTitle.data.like.ToString();
        content.text = sourceTitle.data.content.Replace("<n>","\n");

        comments = sourceTitle.data.comments; // 소스 타이틀의 댓글 데이터 가져오기 : List<CommentData>
        int c_num = sourceTitle.data.c_num; // 댓글 개수 가져오기
        for (int i = 0; i<c_num; i++) // 댓글 개수만큼 반복
        {   
            CommentData comment = comments[i]; // 소스 타이틀의 i번째 CommentData

            GameObject clone = Instantiate(CommentPrefab, CommentParent); // 댓글 프리팹 생성
            CommentHandler commentUI = clone.GetComponent<CommentHandler>(); // 댓글 프리팹에 붙은 핸들러 가져오기
            commentUI.SetupUI(comment);

            if (comment.n_reply >= 1) // 대댓글이 한 개 이상이면
            {
                CommentData rcomment = comment.reply;
                
                GameObject r_clone = Instantiate(ReplyPrefab, CommentParent); // 대댓글 프리팹 생성
                CommentHandler replyUI = r_clone.GetComponent<CommentHandler>(); 
                replyUI.SetupUI(rcomment);

            }


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
        sourceTitle.data.isScrapped = 1;
        Debug.Log($"SCRAPPED : {sourceTitle.data.isScrapped}, class : {classId}");

        displayer.ScrapCounter(classId);

    }
}
