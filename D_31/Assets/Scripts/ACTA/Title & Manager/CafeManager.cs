using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CafeManager : MonoBehaviour
{
    // public GameObject NewsPanel; // 뉴스 제목 클릭시 켜질 패널

    private CafeTitle sourceTitle;

    private int classId;
    private int clicked;

    public TextMeshProUGUI title;
    public TextMeshProUGUI writer;
    // public TextMeshProUGUI views;
    public TextMeshProUGUI like;
    public TextMeshProUGUI content;

    // 댓글 UI 
    public List<TextMeshProUGUI> comment1;
    public List<TextMeshProUGUI> comment2;
    public List<TextMeshProUGUI> comment3;

    private List<List<TextMeshProUGUI>> comments;



    void Awake()
    {
        comments = new List<List<TextMeshProUGUI>>() {comment1, comment2, comment3};

    }

    public void GetSourceTitle(CafeTitle stitle)
    {
        // 데이터의 정보 받아오기
        sourceTitle = stitle;

        classId = sourceTitle.data.classId;
        clicked = sourceTitle.data.isScrapped;

        // 패널 UI 텍스트들 채우기
        title.text = sourceTitle.data.title;
        writer.text = sourceTitle.data.writer;
        // views.text = sourceTitle.data.views.ToString();
        like.text = sourceTitle.data.like.ToString();
        content.text = sourceTitle.data.content.Replace("<n>","\n");
        
        for (int i = 0; i<3;i++)
        {
            comments[i][0].text = sourceTitle.data.comments[i][0];
            comments[i][1].text = sourceTitle.data.comments[i][1];
        }


        // 패널 켜기
        // NewsPanel.SetActive(true);

    }
    // 스크랩 버튼이 눌리면 불러와질 함수 정의
    public void ScrapButtonClicked()
    {
        sourceTitle.data.isScrapped = 1;
        Debug.Log($"SCRAPPED : {sourceTitle.data.isScrapped}, class : {classId}");

        GameManager.Instance.displayer.ScrapCounter(classId);

    }
}
