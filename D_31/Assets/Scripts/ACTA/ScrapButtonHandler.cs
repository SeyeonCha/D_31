using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrapButtonHandler : MonoBehaviour, IPointerClickHandler
{
    private int classId;
    private int clicked;
    private int uniqueId; // 👈 추가함!

    [SerializeField]
    private Sprite before_scrap_img; // 클릭 전 스크랩버튼 이미지
    [SerializeField]
    private Sprite after_scrap_img; // 클릭 후 스크랩버튼 이미지
    private Image buttonImage; // 버튼의 Image 컴포넌트

    // 1. 제목을 저장할 변수 추가
    private string sourceTitle;

    void Awake()
    {
        Debug.Log("스크랩핸들러 어웨이크 실행됨");
        buttonImage = GetComponent<Image>();

 
    }
    public void GetSourceTitle(int isScrapped, int cId, int uId, string title)
    {
        
        classId = cId;
        clicked = isScrapped;
        uniqueId = uId; // 👈 추가함!
        sourceTitle = title; // 제목 저장 [추가]

        Debug.Log($"GetSourceTitle 실행됨 : {clicked}, Title: {sourceTitle}");

        if (clicked == 0) {
            buttonImage.sprite = before_scrap_img;
        }
        else {
            buttonImage.sprite = after_scrap_img;
        }
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        // 이미 스크랩된 상태가 아닐 때만 추가 (중복 방지)
        if (clicked == 0)
        {
            clicked = 1; // 클릭 상태로 전환
            buttonImage.sprite = after_scrap_img;

            // GameManager 리스트에 제목 추가 [추가]
            if (GameManager.Instance != null)
            {
                GameManager.Instance.todayScrappedTitles.Add(sourceTitle);
                Debug.Log($"오늘의 스크랩 리스트에 추가됨: {sourceTitle}");
            }
        }
    }
}
