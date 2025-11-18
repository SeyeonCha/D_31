using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Text.RegularExpressions; 

public class ActaManager : MonoBehaviour
{
    // public static ActaManager Instance {get; private set;}
    public GameObject MissionSuccessPopup; // 미션성공 팝업창

    public bool IsQuestCompleted = false; // 퀘스트 달성 여부
    public int DayEnded = 0; // 데이 수 계산. 

    public bool alarmActivated = false;

    public GameObject alarmText;

    // CSV 데이터 로드 파트
    // [SerializeField]
    // public NewsDataLoader newsDataLoader;
    // public CafeDataLoader cafeDataLoader;
    // public ComuDataLoader comuDataLoader;
    // public PaperDataLoader paperDataLoader;
    // public ReelsDataLoader reelsDataLoader;
    // **나머지 플랫폼의 데이터 로더도 정의. 

    // 제목 업데이터
    [SerializeField]
    public TitleButtonUpdator TitleUpdator;

    // 디스플레이어
    public DisplayScrapData displayer; 
    private TextMeshProUGUI displayTextMesh;

    // 큐레이션 관련
    public List<List<int>> curation_data;

    private const int NUM_CLASSES = 5;
    private const string COUNT_KEY_PREFIX = "Class_";

    private void Awake()
    {
        
        // if (Instance != null && Instance != this)
        // {
        //     Destroy(gameObject);
        //     return;
        // }
        // Instance = this;

        // 씬이 바뀌어도 파괴되지 않도록 설정
        // DontDestroyOnLoad(gameObject);

        // 큐레이션 데이터 초기화
        // curation_data = new List<List<int>>()
        // {
        //     new List<int> {3,4,3,4}, // 0 : 속보 뉴스
        //     new List<int> {0,2,2,1,1}, // 1 : 뉴스 기사
        //     new List<int> {0, 1, 1, 1, 1, 2, 3}, // 2 : 카페 초기화 클래스s
        //     new List<int> {0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 3, 3, 4, 4}, // 3: 커뮤니티
        //     new List<int> {1, 2, 3, 4}, // 4 : 논문 
        //     new List<int> {4, 3, 2, 1} // 5 : 릴스
        //     // **나머지 플랫폼의 초기 클래스 데이터도 여기 입력. 
        // };
        curation_data = GameManager.Instance.CurationData;

        // CSV 데이터 읽어오기
        // newsDataLoader.LoadCsvData(); // 뉴스
        // cafeDataLoader.LoadCsvData(); // 카페
        // comuDataLoader.LoadCsvData();
        // paperDataLoader.LoadCsvData();
        // reelsDataLoader.LoadCsvData();
        // Debug.Log("게임매니저 awake는 돌아가냐.. 1");// 돌아감

        // // ** 나머지 플랫폼의 데이터로더도 여기서 실행
        TitleUpdator.NewsDataMap = GameManager.Instance.newsDataLoader.Data;
        TitleUpdator.CafeDataMap = GameManager.Instance.cafeDataLoader.Data;
        TitleUpdator.ComuDataMap = GameManager.Instance.comuDataLoader.Data;
        TitleUpdator.PaperDataMap = GameManager.Instance.paperDataLoader.Data;
        TitleUpdator.ReelsDataMap = GameManager.Instance.reelsDataLoader.Data;
        // Debug.Log("게임매니저 awake는 돌아가냐.. 2"); // 돌아감

        
        TitleUpdator.UpdateTitles();
        Debug.Log("게임매니저 awake는 돌아가냐.. 3"); // 안돌아감

        // 제목 띄우기
        // 게임매니저 어웨이크가 개처 안돌아가는 문제
        // ResetCount();

    }
    private void Update()
    {
        if (PlayerPrefs.GetInt("Total_Count",0) >= 9) // 스크랩 수 충족시
        {
            // Debug.Log("퀘스트 달성!! 다음날 게시물 계산 실행");
            // 퀘스트 상태 : 달성!
            IsQuestCompleted = true;
            // EndDayButton.SetActive(true);

            // 퀘스트 완료하자마자 스크랩 카운트 데이터 저장
            PlayerPrefs.Save();

            MissionSuccessPopup.SetActive(true);
            GameManager.Instance.missionCompleted = true; // 이거 땜에 엔드 데이 버튼이 활성화 될거임. (안됨)
        }
    }
    public void MailAlarm()
    {
        Invoke("ActivateAlarm",30f);
        alarmActivated = true;
        // alarmText.SetActive(true);
    }
    private void ActivateAlarm()
    {
        alarmText.SetActive(true);
    }
    
    

}
