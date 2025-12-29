using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;

public class DiaryManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField diaryInputField; // 일기 입력창
    public TextMeshProUGUI instructText;   // 안내 메시지 텍스트
    public GameObject nextDayButton; // 다음 날로 넘어가는 버튼

    [Header("New UI Elements")]
    public TextMeshProUGUI dayText;        // 상단 날짜 표시용 (예: D-31) [추가]
    public TextMeshProUGUI scrapListText; // 게시물 리스트 표시용 [추가]

    [Header("Save Settings")]
    private string currentDayKey; // 현재 저장될 날짜 키 (예: D-31)

    void OnEnable()
    {
        // 패널이 켜질 때마다 안내 문구 초기화
        if (instructText != null) instructText.text = "";

        // 일기장을 열 때마다 '다음 날로' 버튼은 일단 비활성화 (저장을 유도)
        if (nextDayButton != null) nextDayButton.SetActive(false);
        
        // 현재 게임의 날짜에 맞는 키 설정
        SetCurrentDayKey();
        UpdateDayUI();      // UI 날짜 갱신 [추가]
        UpdateScrapList();   // 오늘 스크랩한 제목 리스트 갱신 [추가]
    }

    // GameManager의 DayEnded 값에 따라 저장 키를 결정합니다.
    void SetCurrentDayKey()
    {
        int day = GameManager.DayEnded;
        
        // 기획하신 날짜 순서에 맞게 매핑 (예시)
        switch (day)
        {
            case 0: currentDayKey = "D-31"; break;
            case 1: currentDayKey = "D-30"; break;
            case 2: currentDayKey = "D-14"; break;
            case 3: currentDayKey = "D-1"; break;
            default: currentDayKey = "UnknownDay"; break;
        }
    }

    // 상단 텍스트를 현재 데이(예: D-31)로 변경
    void UpdateDayUI()
    {
        if (dayText != null)
        {
            dayText.text = currentDayKey;
        }
    }

    // 오늘 저장한 게시물 제목을 나열
    void UpdateScrapList()
    {
        if (scrapListText == null) return;

        // GameManager에 추가한 리스트 참조
        List<string> titles = GameManager.Instance.todayScrappedTitles;

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("오늘 저장한 게시물:");

        // 최대 4개까지 표시
        for (int i = 0; i < 4; i++)
        {
            string titleText = (i < titles.Count) ? titles[i] : "없음";
            sb.AppendLine($"{i + 1}. [{titleText}]");
        }

        scrapListText.text = sb.ToString();
    }

    // Save 버튼에 연결할 함수
    public void SaveDiary()
    {
        if (diaryInputField == null) return;

        string content = diaryInputField.text;

        if (!string.IsNullOrEmpty(content))
        {
            // PlayerPrefs에 "D-31" 등의 키로 내용 저장
            PlayerPrefs.SetString(currentDayKey, content);
            PlayerPrefs.Save(); // 디스크에 즉시 기록

            if (instructText != null)
            {
                instructText.text = "저장하였습니다.";
                instructText.color = Color.green; // 성공 시 초록색 (선택사항)
            }

            // [핵심 추가] 저장 성공 시 '다음 날로' 버튼 활성화
            if (nextDayButton != null)
            {
                nextDayButton.SetActive(true);
            }
            
            Debug.Log($"[DiaryManager] {currentDayKey} 저장 완료 및 다음 날 버튼 활성화");
        }
        else
        {
            if (instructText != null)
            {
                instructText.text = "내용을 입력해주세요.";
                instructText.color = Color.red;
            }
        }
    }
}