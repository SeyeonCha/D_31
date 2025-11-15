using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class PaperManager : MonoBehaviour
{
    private PaperTitle sourceTitle;

    private int classId;
    private int clicked;

    public TextMeshProUGUI title;
    public TextMeshProUGUI author;
    public TextMeshProUGUI year;
    public TextMeshProUGUI AI_T;
    public TextMeshProUGUI AI_F; 

    // 👇 길이 반영 딜레이 문제 때문에 아래 코드 추가됨
    public RectTransform contentRectTransform;
    public RectTransform newsPanelRectTransform;


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
        AI_T.text = sourceTitle.data.AI_T.Replace("<n>","\n");
        // AI_F.text = sourceTitle.data.AI_F.Replace("<n>","\n");

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
    // public void ScrapButtonClicked() // 왜 이게 스크롤 하면 불러와지냐고,, 
    // {
    //     if (sourceTitle == null)
    //     {
    //         // Debug.Log($"논문 sourceTitle 이 없음");
    //     }
    //     else if (sourceTitle.data.isScrapped ==0)
    //     {
    //         sourceTitle.data.isScrapped = 1;
    //         Debug.Log($"title : {sourceTitle.data.title} scrapped : {sourceTitle.data.isScrapped}");
    //     }

    // }
}
