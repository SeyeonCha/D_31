using System.Collections; // 코루틴을 사용하기 위해 추가
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MainManager : MonoBehaviour
{
    [SerializeField]
    private GameObject StartButton;

    [SerializeField]
    private GameObject ExitButton;

    [SerializeField]
    private GameObject CreditPanel;

    private AudioSource audioSource;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
    }

    void Start()
    {
        // 1920x1080 해상도로 설정
        int width = 1920;
        int height = 1080;

        // FullScreenMode.FullScreenWindow: Mac에서 Retina 해상도를 가장 잘 지원하는 모드입니다.
        // 빨간 줄 방지를 위해 UnityEngine.FullScreenMode라고 전체 이름을 적어주는 것이 안전합니다.
        Screen.SetResolution(width, height, UnityEngine.FullScreenMode.FullScreenWindow);
        
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