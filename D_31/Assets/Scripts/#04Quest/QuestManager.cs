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

    void Start()
    {
        bool isOrganSold = GameManager.isOrganSold;
        int currentDay = GameManager.DayEnded;
        
        // 🚨 조건 확인: isOrganSold가 false이고 DayEnded가 3일 때 (로또 당첨 조건)
        if (isOrganSold == false && currentDay == 3)
        {
            if (lottoPopupPanel != null)
            {
                lottoPopupPanel.SetActive(true);
                Debug.Log("[LottoManager] Lotto Popup Panel activated. Condition met (Organ Sold and Day 4).");
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
            
            // EndDayButton의 현재 활성화 상태와 missionCompleted 상태를 비교
            if (EndDayButton.activeSelf != isMissionComplete)
            {
                // 상태가 다를 경우에만 SetActive 호출 (성능 최적화)
                EndDayButton.SetActive(isMissionComplete);
                Debug.Log($"[LottoManager] EndDayButton updated to: {isMissionComplete}");
            }
        }
    }

    // 게임 시작 함수
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