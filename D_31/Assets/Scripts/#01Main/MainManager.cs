using System.Collections; // 코루틴을 사용하기 위해 추가
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    [SerializeField]
    private GameObject StartButton;

    [SerializeField]
    private GameObject ExitButton;

    [SerializeField]
    private GameObject CreditPanel; 

    void Start()
    {
        if (CreditPanel != null) CreditPanel.SetActive(false);

    }
    
    // 게임 시작 함수
    public void StartGame()
    {
        SceneManager.LoadScene("#02Intro");
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
}