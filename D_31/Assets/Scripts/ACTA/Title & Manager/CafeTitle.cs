using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class CafeTitle : MonoBehaviour, IPointerClickHandler
{
    public CafeData data; // 해당 제목에 해당하는 데이터
    private TextMeshProUGUI titleText; // 제목 텍스트

    public GameObject post_panel;
    public GameObject ScrapButton; 

    void Start()
    {
        titleText = GetComponentInChildren<TextMeshProUGUI>(); // 제목 텍스트 찾기

        // 자식 텍스트 찾았는지 확인
        if (titleText == null)
        {
            Debug.LogError("자식 오브젝트에서 TextMeshProUGUI 컴포넌트를 찾을 수 없습니다.", this);
            return;
        }
        // Debug.Log($"자식오브젝트 텍스트 찾음 : {titleText.text}");
        
    }
    void Update()
    {
        // 자식 텍스트를 해당 data의 title로 변경
        if (titleText != null && data != null)
        {
            titleText.text = data.title; 
            // Debug.Log("텍스트 바꾸기 완료. ");
        }
        else if (data == null)
        {
            Debug.Log("데이터를 왜 못찾냐..");
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (post_panel == null) 
        {
            Debug.Log("왜 카페 패널을 못찾지?");
        }
        else 
        {
            CafeManager cafeManager = post_panel.GetComponent<CafeManager>();
            if (cafeManager == null) 
            {
                Debug.Log("뉴스 메니저 못찾음");
            }
            else {
                Debug.Log("뉴스 메니저 찾음");

                // 패널 켜기
                post_panel.SetActive(true);
                // 패널 UI 채우기
                cafeManager.GetSourceTitle(this);

                // 스크랩 버튼 설정
                ScrapButtonHandler ScrapButtonHandler = ScrapButton.GetComponent<ScrapButtonHandler>();
                ScrapButtonHandler.GetSourceTitle(data.isScrapped, data.classId);

                
            }
            
        }
        // UI 업데이트
        

        

    }
}
