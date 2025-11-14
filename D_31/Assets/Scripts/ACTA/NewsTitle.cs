using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// 제목 버튼 컴포넌트로 붙여서, 해당 제목의 데이터를 저장할거임. 

public class NewsTitle : MonoBehaviour, IPointerClickHandler
{
    public NewsData data; // 해당 제목에 해당하는 데이터
    private TextMeshProUGUI titleText; // 제목 텍스트
    // public NewsManager newsManager;

    public GameObject post_panel; // 뉴스 패널
    public GameObject ScrapButton; // 뉴스 스크랩 버튼


    void Start()
    {
        titleText = GetComponent<TextMeshProUGUI>(); // 제목 텍스트 찾기

        // 자식 텍스트 찾았는지 확인
        if (titleText == null)
        {
            Debug.LogError("자식 오브젝트에서 TextMeshProUGUI 컴포넌트를 찾을 수 없습니다.", this);
            return;
        }
        Debug.Log($"제목 텍스트 텍스트 찾음 : {titleText.text}");
        
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

        // 댓글도 불러오기 (프리팹 생성 & 데이터 참조해서 닉네임/내용 채워넣기)
        // data.comments
        if (post_panel == null) 
        {
            Debug.Log("왜 뉴스 패널을 못찾지?");
        }
        else 
        {
            NewsManager newsManager = post_panel.GetComponent<NewsManager>();
            if (newsManager == null) 
            {
                Debug.Log("뉴스 메니저 못찾음");
            }
            else {
                Debug.Log("뉴스 매니저 찾음");

                // 패널 켜기
                post_panel.SetActive(true);

                // 패널 UI 채우기
                newsManager.GetSourceTitle(this);
                
                // 스크랩 버튼 설정
                ScrapButtonHandler ScrapButtonHandler = ScrapButton.GetComponent<ScrapButtonHandler>();
                ScrapButtonHandler.GetSourceTitle(data.isScrapped, data.classId);

                
            }
            
        }
        // UI 업데이트
        

        

    }


}
