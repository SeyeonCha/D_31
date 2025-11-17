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

    public PaperManager paperManager;

    private Image buttonImage; // 버튼의 Image 컴포넌트

    void Awake()
    {
        Debug.Log("스크랩핸들러 어웨이크 실행됨");
        buttonImage = GetComponent<Image>();

 
    }
    public void GetSourceTitle(int isScrapped, int cId, int uId)
    {
        
        classId = cId;
        clicked = isScrapped;
        uniqueId = uId; // 👈 추가함!
        Debug.Log($"GetSourceTitle 실행됨 : {clicked}");

        if (clicked == 0) {
            buttonImage.sprite = before_scrap_img;
        }
        else {
            buttonImage.sprite = after_scrap_img;
        }
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {

        buttonImage.sprite = after_scrap_img;
        // if (paperManager != null)
        // {
            // paperManager.ScrapButtonClicked();

        // }
    }
}
