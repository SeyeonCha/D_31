using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class NewsManager : MonoBehaviour
{
    // public GameObject NewsPanel; // 뉴스 제목 클릭시 켜질 패널

    private NewsTitle sourceTitle;

    private int classId;
    private int clicked;

    public TextMeshProUGUI title;
    public TextMeshProUGUI reporter;
    public TextMeshProUGUI like1;
    public TextMeshProUGUI dislike1;
    public TextMeshProUGUI like2;       // 👈 추가함!
    public TextMeshProUGUI dislike2;    // 👈 추가함!
    public TextMeshProUGUI content;

    // 댓글 UI 
    public List<TextMeshProUGUI> comment1;
    public List<TextMeshProUGUI> comment2;
    public List<TextMeshProUGUI> comment3;
    public List<TextMeshProUGUI> comment4;

    private List<List<TextMeshProUGUI>> comments;
    // public TextMeshProUGUI comment1;
    // public TextMeshProUGUI name2;
    // public TextMeshProUGUI comment2;
    // public TextMeshProUGUI name3;
    // public TextMeshProUGUI comment3;
    // public TextMeshProUGUI name4;
    // public TextMeshProUGUI comment4;

    // 👇 길이 반영 딜레이 문제 때문에 아래 코드 추가됨
    public RectTransform contentRectTransform;
    public RectTransform newsPanelRectTransform;

    private void Awake()
    {
        comments = new List<List<TextMeshProUGUI>>() {comment1, comment2, comment3, comment4};
        
        // 해당 패널의 자식 오브젝트 UI 텍스트 가져오기
        // reporter = transform.Find("reporter").GetComponent<TextMeshProUGUI>();
        // like = transform.Find("like").GetComponent<TextMeshProUGUI>();
        // dislike = transform.Find("dislike").GetComponent<TextMeshProUGUI>();
        // content = transform.Find("content").GetComponent<TextMeshProUGUI>();

        // if (reporter == null || like == null || dislike == null || content == null) {
        //     Debug.Log("패널의 자식 UI들을 못찾음..");
        // }
        // else {
        //     Debug.Log("패널의 자식 UI들을  모두 찾음!");
        // }

    }

    public void GetSourceTitle(NewsTitle stitle)
    {
        // InitUI();
        // 데이터의 정보 받아오기
        sourceTitle = stitle;
        // sourceData = sourceTitle.data;
        classId = sourceTitle.data.classId;
        clicked = sourceTitle.data.isScrapped;

        // 패널 UI 텍스트들 채우기
        if (reporter == null) {
            Debug.Log("리포터 UI가 없음");
        }
        else{
            Debug.Log($"리포터 데이터 있긴함 : {sourceTitle.data.reporter}");
            reporter.text = sourceTitle.data.reporter;
        }
        title.text = sourceTitle.data.title;
        like1.text = sourceTitle.data.like.ToString();
        dislike1.text = sourceTitle.data.dislike.ToString();
        like2.text = sourceTitle.data.like.ToString();          // 👈 추가함!
        dislike2.text = sourceTitle.data.dislike.ToString();    // 👈 추가함!
        content.text = sourceTitle.data.content.Replace("<n>","\n");

        for (int i = 0; i<4;i++)
        {
            comments[i][0].text = sourceTitle.data.comments[i][0];
            comments[i][1].text = sourceTitle.data.comments[i][1];
        }

        // 👇 길이 반영 딜레이 문제 때문에 아래 코드 추가됨
        if (newsPanelRectTransform != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(newsPanelRectTransform);
        }

        if (contentRectTransform != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentRectTransform);
        }

        // 패널 켜기
        // NewsPanel.SetActive(true);

    }
    // 스크랩 버튼이 눌리면 불러와질 함수 정의
    public void ScrapButtonClicked()
    {
        sourceTitle.data.isScrapped = 1;
        Debug.Log($"sourceTitle.data.isScrpped : {sourceTitle.data.isScrapped}");

    }
}
