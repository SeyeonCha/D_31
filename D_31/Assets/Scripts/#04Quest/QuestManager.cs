using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("활성화할 로또 팝업 패널을 할당하세요.")]
    public GameObject lottoPopupPanel; 

    [SerializeField]
    private GameObject MainButton;

    [SerializeField]
    private GameObject ExitButton;

    [Header("Conditional Buttons")]
    [Tooltip("미션 완료 시 활성화할 다음 날로 넘어가는 버튼을 할당하세요.")]
    public GameObject EndDayButton;

    [Header("Send Lotto")]
    [Tooltip("브로커에게 송금하기를 선택할 때 누르는 버튼을 할당하세요.")]
    public GameObject GoToBrokerButton;

    // 계급 상승 축하 패널
    public GameObject ClassUpgradePanel;

    void Start()
    {
        bool isOrganSold = GameManager.isOrganSold;
        int currentDay = GameManager.DayEnded;

        if (GoToBrokerButton != null) GoToBrokerButton.SetActive(false);
        if (ClassUpgradePanel != null) ClassUpgradePanel.SetActive(false);
        
        // 🚨 조건 확인: isOrganSold가 false이고 DayEnded가 3일 때 (로또 당첨 조건)
        if (isOrganSold == false && currentDay == 3)
        {
            if (lottoPopupPanel != null)
            {
                lottoPopupPanel.SetActive(true);
                Debug.Log("[LottoManager] Lotto Popup Panel activated. Condition met (Organ Sold and Day 4).");

                if (GoToBrokerButton != null)
                {
                    GoToBrokerButton.SetActive(true);
                }
            }
            else
            {
                Debug.LogError("[LottoManager] Lotto Popup Panel is not assigned in the Inspector.");
            }
        }
        else
        {
            Debug.Log($"[LottoManager] Lotto Popup condition not met. (Sold: {isOrganSold}, Day: {currentDay})");
            
            // 조건이 충족되지 않으면 패널을 비활성화 상태로 시작합니다.
            if (lottoPopupPanel != null)
            {
                 lottoPopupPanel.SetActive(false);
            }
        }
    }

    void Update()
    {
        // **[추가]** GameManager가 유효하고 EndDayButton이 할당되었을 때만 처리
        if (GameManager.Instance != null && EndDayButton != null)
        {
            bool isMissionComplete = GameManager.Instance.missionCompleted;
            int currentDay = GameManager.DayEnded;

            // 1. [핵심 추가 기능] 계급 상승 패널 활성화 조건 검사
            if (ClassUpgradePanel != null && currentDay == 3)
            {
                bool isOrganOrBrokerPath = GameManager.isOrganSold || GameManager.isLotteryToBroker;

                if (isMissionComplete && isOrganOrBrokerPath)
                {
                    // 미션 완료 상태이고, 장기 매매 경로 또는 로또-브로커 경로를 탔을 때
                    if (!ClassUpgradePanel.activeSelf)
                    {
                        ClassUpgradePanel.SetActive(true);
                        Debug.Log("[QuestManager] Class Upgrade Panel activated. Mission Complete and Path taken.");

                        // Class Upgrade Panel이 활성화되면 EndDayButton은 일단 비활성화 (패널에 있는 버튼으로 진행 유도)
                        // EndDayButton.SetActive(false);
                    }
                }
                else if (ClassUpgradePanel.activeSelf)
                {
                    // 조건이 충족되지 않았는데 패널이 활성화되어 있다면 비활성화 (혹시 모를 오류 방지)
                    ClassUpgradePanel.SetActive(false);
                }
            }
            
            // EndDayButton의 현재 활성화 상태와 missionCompleted 상태를 비교
            if (EndDayButton.activeSelf != isMissionComplete)
            {
                // 상태가 다를 경우에만 SetActive 호출 (성능 최적화)
                EndDayButton.SetActive(isMissionComplete);
                Debug.Log($"[LottoManager] EndDayButton updated to: {isMissionComplete}");
            }
        }
    }

    // 브로커에게 송금하기 선택 시
    public void OnGoToBrokerButtonClicked()
    {
        if (GameManager.Instance != null)
        {
            // 1. GameManager의 isLotteryToBroker를 true로 변경
            GameManager.isLotteryToBroker = true;
            Debug.Log("[QuestManager] GameManager.isLotteryToBroker set to TRUE.");

            // 버튼 자체도 비활성화 (선택 완료)
            if (GoToBrokerButton != null)
            {
                GoToBrokerButton.SetActive(false);
            }

            // 4. (추가) isLotteryToBroker가 true가 되었으므로, 이제 미션 완료 상태를 true로 설정하여
            //    Update()에서 ClassUpgradePanel이 활성화되도록 유도합니다.
            // GameManager.Instance.missionCompleted = true;
        }
        else
        {
            Debug.LogError("[QuestManager] GameManager.Instance is null when trying to set isLotteryToBroker.");
        }
    }

    // 메인메뉴 돌아가기 함수
    public void ReturnToMain()
    {
        SceneManager.LoadScene("#01Main");
    }

    // 게임 종료 함수
    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    // 다음 날로 넘어가는 버튼 클릭 함수
    public void EndDayButtonClicked()
    {
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.missionCompleted == true)
            {
                // 미션 완료 상태를 다시 false로 리셋
                GameManager.Instance.missionCompleted = false; 
                
                Debug.Log("ToNextDay() 실행 in EndDayButton");
                
                // GameManager의 다음 날로 넘어가는 함수 호출
                GameManager.Instance.ToNextDay();
            }
            else
            {
                Debug.LogWarning("[LottoManager] EndDayButtonClicked pressed but mission is not completed.");
            }
        }
        else
        {
            Debug.LogError("[LottoManager] GameManager.Instance is null.");
        }
        
    }
}