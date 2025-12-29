using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainPanel;      // 현재 화면의 메인 UI 패널
    public GameObject diaryPanel;     // 일기장 UI 패널
    public GameObject lottoPopupPanel; 

    [Header("UI Buttons")]
    public GameObject mainButton;
    public GameObject exitButton;
    public GameObject endDayButton;    // 미션 완료 시 나타나는 버튼
    public GameObject goToBrokerButton;
    public GameObject classUpgradePanel;

    [Header("Audio")]
    [Tooltip("배경 음악이 있는 카메라의 AudioSource를 할당하세요.")]
    public AudioSource bgmSource; // 카메라에 있는 AudioSource 연결용

    void Start()
    {
        bool isOrganSold = GameManager.isOrganSold;
        int currentDay = GameManager.DayEnded;

        // 초기 패널 설정
        if (diaryPanel != null) diaryPanel.SetActive(false);
        if (mainPanel != null) mainPanel.SetActive(true);
        if (goToBrokerButton != null) goToBrokerButton.SetActive(false);
        if (classUpgradePanel != null) classUpgradePanel.SetActive(false);
        
        // 로또 당첨 조건 확인
        if (isOrganSold == false && currentDay == 3)
        {
            if (lottoPopupPanel != null)
            {
                lottoPopupPanel.SetActive(true);
                if (goToBrokerButton != null) goToBrokerButton.SetActive(true);
            }
        }
        else
        {
            if (lottoPopupPanel != null) lottoPopupPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (GameManager.Instance != null && endDayButton != null)
        {
            bool isMissionComplete = GameManager.Instance.missionCompleted;
            int currentDay = GameManager.DayEnded;

            // 계급 상승 패널 활성화 조건
            if (classUpgradePanel != null && currentDay == 3)
            {
                bool isOrganOrBrokerPath = GameManager.isOrganSold || GameManager.isLotteryToBroker;

                if (isMissionComplete && isOrganOrBrokerPath)
                {
                    if (!classUpgradePanel.activeSelf)
                    {
                        classUpgradePanel.SetActive(true);
                    }
                }
            }
            
            // 미션 완료 시 '일기 쓰기(EndDayButton)' 버튼 활성화
            if (endDayButton.activeSelf != isMissionComplete)
            {
                endDayButton.SetActive(isMissionComplete);
            }
        }
    }

    // --- 클릭 이벤트 함수들 ---

    // 1. [핵심] EndDayButton을 눌렀을 때 실행 (일기장 열기)
    public void OnEndDayButtonClicked()
    {
        if (mainPanel != null) mainPanel.SetActive(false);   // 메인 화면 끄기
        if (diaryPanel != null) diaryPanel.SetActive(true);  // 일기장 화면 켜기

        if (bgmSource != null && bgmSource.isPlaying)
        {
            bgmSource.Stop();
            Debug.Log("[QuestManager] BGM Stopped.");
        }

        Debug.Log("[QuestManager] Opened Diary Panel.");
    }

    // 2. [핵심] DiaryPanel 안에 있는 NextDayButton을 눌렀을 때 실행 (다음 날로)
    public void OnNextDayButtonClicked()
    {
        if (GameManager.Instance != null)
        {
            // 미션 완료 상태 리셋
            GameManager.Instance.missionCompleted = false; 
            
            Debug.Log("[QuestManager] Moving to Next Day via Diary.");
            
            // 실제 다음 날 이동 로직 호출
            GameManager.Instance.ToNextDay();
        }
    }

    public void OnGoToBrokerButtonClicked()
    {
        if (GameManager.Instance != null)
        {
            GameManager.isLotteryToBroker = true;
            if (goToBrokerButton != null) goToBrokerButton.SetActive(false);
        }
    }

    public void ReturnToMain() => SceneManager.LoadScene("#01Main");

    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}