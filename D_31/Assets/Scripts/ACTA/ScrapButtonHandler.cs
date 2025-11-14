using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrapButtonHandler : MonoBehaviour, IPointerClickHandler
{
    private int classId;
    private int clicked;

    [SerializeField]
    private Sprite before_scrap_img; // 클릭 전 스크랩버튼 이미지
    [SerializeField]
    private Sprite after_scrap_img; // 클릭 후 스크랩버튼 이미지

    private Image buttonImage; // 버튼의 Image 컴포넌트

    void Awake()
    {
        Debug.Log("스크랩핸들러 어웨이크 실행됨");
        buttonImage = GetComponent<Image>();

 
    }
    public void GetSourceTitle(int isScrapped, int cId)
    {
        
        classId = cId;
        clicked = isScrapped;
        Debug.Log($"스크랩버튼 clicked : {clicked}");

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
    }
}
