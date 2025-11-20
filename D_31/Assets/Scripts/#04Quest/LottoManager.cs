using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LottoManager : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("활성화할 로또 팝업 패널을 할당하세요.")]
    public GameObject lottoPopupPanel; 

    [SerializeField]
    private GameObject MainButton;

    [SerializeField]
    private GameObject ExitButton;

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
    
    // 이 스크립트가 붙은 씬이 활성화될 때 한 번 실행됩니다.
    void Start()
    {
        // GameManager 클래스에 isOrganSold와 DayEnded가 public static 변수로 선언되어 있다고 가정합니다.
        
        bool isOrganSold = GameManager.isOrganSold;
        int currentDay = GameManager.DayEnded;
        
        // 🚨 조건 확인: isOrganSold가 true이고 DayEnded가 3일 때
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
            // Debug.Log($"[LottoManager] Lotto Popup condition not met. (Sold: {isOrganSold}, Day: {currentDay})");
            
            // 조건이 충족되지 않으면 패널을 비활성화 상태로 시작합니다.
            if (lottoPopupPanel != null)
            {
                 lottoPopupPanel.SetActive(false);
            }
        }
    }
}