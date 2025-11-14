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
    private CafeData sourceData;

    private int classId;
    private int clicked;
    private List<List<string>> comments;

    private TextMeshProUGUI writer;
    private TextMeshProUGUI views;
    private TextMeshProUGUI like;
    private TextMeshProUGUI content;



    void Start()
    {
        // 해당 패널의 자식 오브젝트 UI 텍스트 가져오기
        writer = transform.Find("writer").GetComponent<TextMeshProUGUI>();
        views = transform.Find("views").GetComponent<TextMeshProUGUI>();
        like = transform.Find("like").GetComponent<TextMeshProUGUI>();
        content = transform.Find("content").GetComponent<TextMeshProUGUI>();

        if (writer == null || views == null || like == null || content == null) {
            Debug.Log("패널의 자식 UI들을 못찾음..");
        }
        else {
            Debug.Log("패널의 자식 UI들을  모두 찾음!");
        }

    }

    public void GetSourceTitle(CafeTitle title)
    {
        // 데이터의 정보 받아오기
        sourceTitle = title;
        // sourceData = sourceTitle.data;
        classId = sourceTitle.data.classId;
        clicked = sourceTitle.data.isScrapped;

        // 패널 UI 텍스트들 채우기
        writer.text = sourceTitle.data.writer;
        views.text = "조회수 : " + sourceTitle.data.views.ToString();
        like.text = "좋아요수 : " + sourceTitle.data.like.ToString();
        content.text = sourceTitle.data.content.Replace("<n>","\n");
        


        // 패널 켜기
        // NewsPanel.SetActive(true);

    }
    // 스크랩 버튼이 눌리면 불러와질 함수 정의
    public void ScrapButtonClicked()
    {
        sourceTitle.data.isScrapped = 1;

    }
}
