using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
   
    
    public static GameManager Instance {get; private set;}
    public static int DayEnded = 0;
    private const int NUM_CLASSES = 5;
    private const string COUNT_KEY_PREFIX = "Class_";
 
    public static bool isOrganSold = false; // 장기 매매 여부
    public static bool isLotteryToBroker = false; // 로또 당첨금을 브로커에게 줬는지 여부
    public static bool isMailSent = false;


    public NewsDataLoader newsDataLoader;
    public ComuDataLoader comuDataLoader;
    public CafeDataLoader cafeDataLoader;
    public PaperDataLoader paperDataLoader;
    public ReelsDataLoader reelsDataLoader;

    public RecoSystem recoSystem;

    public List<List<int>> CurationData;
    [Header("Audio Settings")]
    public AudioClip missionCompleteSound;
    private AudioSource audioSource;
    private bool audioPlayedForMission = false;

    public bool missionCompleted = false;

    

    private void Awake()
    {
        // 싱글톤 패턴
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();

        // 데이터 로더들 연결
        newsDataLoader = GetComponent<NewsDataLoader>();
        comuDataLoader = GetComponent<ComuDataLoader>();
        cafeDataLoader = GetComponent<CafeDataLoader>();
        paperDataLoader = GetComponent<PaperDataLoader>();
        reelsDataLoader = GetComponent<ReelsDataLoader>();

        recoSystem = GetComponent<RecoSystem>();
        audioPlayedForMission = false;

        // 데이31 준비
        LoadDayData();

        CurationData = new List<List<int>>()
        {
            new List<int> {3,4,3,4}, // 0 : 속보 뉴스
            new List<int> {0,2,2,1,1}, // 1 : 뉴스 기사
            new List<int> {1, 2, 3, 4}, // 2 : 논문 
            new List<int> {4, 3, 2, 1}, // 3 : 릴스
            new List<int> {0, 1, 1, 1, 1, 2, 3}, // 4 : 카페 초기화 클래스s
            new List<int> {0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 3, 3, 4, 4} // 5: 커뮤니티
        };

        ResetCount();

    }

    public void SetMissionCompleted(bool state)
    {
        // 상태 변경
        missionCompleted = state;
        
        // 오디오 재생 로직
        if (missionCompleted && !audioPlayedForMission)
        {
            if (audioSource != null && missionCompleteSound != null)
            {
                audioSource.PlayOneShot(missionCompleteSound);
                audioPlayedForMission = true; // 재생했음을 표시
                Debug.Log("Mission Complete Audio Played.");
            }
            else
            {
                Debug.LogWarning("Mission Complete Audio or AudioSource not set up properly.");
            }
        }
        else if (!missionCompleted)
        {
            // 미션이 완료되지 않은 상태로 되돌아가는 경우 (필요 시 주석 해제)
            // audioPlayedForMission = false; 
        }
    }
    
    public void ToNextDay() 
    {
        if (DayEnded == 3)
        {
            SceneManager.LoadScene("Final");
        }
        else
        {
            Debug.Log($"ToNextDay() 실행됨, DayEnded : {DayEnded}, {missionCompleted}");

            DayEnded +=1 ;
            // 데이 30 준비
            LoadDayData();
            Debug.Log($"30일 데이터 로더 실행됨, DayEnded : {DayEnded}");
            CurationData = recoSystem.CurationCalculator();
            Debug.Log($"CurationCalculator 실행됨, DayEnded : {DayEnded}");
            Debug.Log($"CurationData : {CurationData[0]}"+
                        $"CurationData : {CurationData[1]}"+
                        $"CurationData : {CurationData[2]}"+
                        $"CurationData : {CurationData[3]}"+
                        $"CurationData : {CurationData[4]}"+
                        $"CurationData : {CurationData[5]}");
            ResetCount();
            SceneManager.LoadScene("#03News"); // 다음 데이의 뉴스로 넘어가면 됨. 
        }
        


    }
    public void LoadDayData()
    {
        if (DayEnded == 0) // 데이31의 데이터를 로드
        {
            newsDataLoader.LoadCsvData(newsDataLoader.titleDataFile31);
            comuDataLoader.LoadCsvData(comuDataLoader.titleDataFile31);
            cafeDataLoader.LoadCsvData(cafeDataLoader.titleDataFile31);
            paperDataLoader.LoadCsvData(paperDataLoader.titleDataFile31);
            reelsDataLoader.LoadCsvData(reelsDataLoader.titleDataFile31);
        }
        else if (DayEnded == 1) // 데이30의 데이터를 로드
        {
            Debug.Log($"30일 데이터 읽기!");
            newsDataLoader.LoadCsvData(newsDataLoader.titleDataFile30);
            comuDataLoader.LoadCsvData(comuDataLoader.titleDataFile30);
            cafeDataLoader.LoadCsvData(cafeDataLoader.titleDataFile30);
            paperDataLoader.LoadCsvData(paperDataLoader.titleDataFile30);
            reelsDataLoader.LoadCsvData(reelsDataLoader.titleDataFile30);
        }
        else if (DayEnded == 2)
        {
            newsDataLoader.LoadCsvData(newsDataLoader.titleDataFile14);
            comuDataLoader.LoadCsvData(comuDataLoader.titleDataFile14);
            cafeDataLoader.LoadCsvData(cafeDataLoader.titleDataFile14);
            paperDataLoader.LoadCsvData(paperDataLoader.titleDataFile14);
            reelsDataLoader.LoadCsvData(reelsDataLoader.titleDataFile14);
        }
        else if (DayEnded == 3)
        {
            newsDataLoader.LoadCsvData(newsDataLoader.titleDataFile4);
            comuDataLoader.LoadCsvData(comuDataLoader.titleDataFile4);
            cafeDataLoader.LoadCsvData(cafeDataLoader.titleDataFile4);
            paperDataLoader.LoadCsvData(paperDataLoader.titleDataFile4);
            reelsDataLoader.LoadCsvData(reelsDataLoader.titleDataFile4);
        }
    }
    public void ResetCount() // PlayerPrefs 값 0으로 초기화 함수
    {
        Debug.Log("ResetCount 실행");
        for (int i=0;i<NUM_CLASSES;i++)
            {
                PlayerPrefs.SetInt(COUNT_KEY_PREFIX + i + "_Count",0); // 추천 시스템 스크랩 수 클래스별 카운트
            }
        PlayerPrefs.SetInt("Total_Count",0);
        PlayerPrefs.Save();
    }
}
