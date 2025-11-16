using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ComuManager : MonoBehaviour
{
    private ComuTitle sourceTitle;

    private int classId;
    private int clicked;

    public TextMeshProUGUI title;
    public TextMeshProUGUI writer;
    public TextMeshProUGUI like;   
    public TextMeshProUGUI content;

    // 댓글 UI 
    // public List<TextMeshProUGUI> comment1;
    // public List<TextMeshProUGUI> comment2;
    // public List<TextMeshProUGUI> comment3;
    // public List<TextMeshProUGUI> comment4;

    // private List<List<TextMeshProUGUI>> comments;
    // public TextMeshProUGUI comment1;
    // public TextMeshProUGUI name2;
    // public TextMeshProUGUI comment2;
    // public TextMeshProUGUI name3;
    // public TextMeshProUGUI comment3;
    // public TextMeshProUGUI name4;
    // public TextMeshProUGUI comment4;

    // 👇 길이 반영 딜레이 문제 때문에 아래 코드 추가됨
    // public RectTransform contentRectTransform;
    // public RectTransform newsPanelRectTransform;

    private void Awake()
    {
        // comments = new List<List<TextMeshProUGUI>>() {comment1, comment2, comment3, comment4};

    }

    public void GetSourceTitle(ComuTitle stitle)
    {
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

        // for (int i = 0; i<4;i++)
        // {
        //     comments[i][0].text = sourceTitle.data.comments[i][0];
        //     comments[i][1].text = sourceTitle.data.comments[i][1];
        // }

        // 👇 길이 반영 딜레이 문제 때문에 아래 코드 추가됨
        // if (newsPanelRectTransform != null)
        // {
        //     LayoutRebuilder.ForceRebuildLayoutImmediate(newsPanelRectTransform);
        // }

        // if (contentRectTransform != null)
        // {
        //     LayoutRebuilder.ForceRebuildLayoutImmediate(contentRectTransform);
        // }

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
