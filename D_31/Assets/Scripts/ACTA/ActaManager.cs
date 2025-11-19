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
        // 메일 알람 키는 함수 (데이14는 악타 키자마자 바로 메일답장 있음)
        // if (GameManager.DayEnded == 2)
        if (true)
        {
            ActivateAlarm(); 
            Debug.Log("Alarm Activated");
        }
        
        curation_data = GameManager.Instance.CurationData;

        // // ** 나머지 플랫폼의 데이터로더도 여기서 실행
        TitleUpdator.NewsDataMap = GameManager.Instance.newsDataLoader.Data;
        TitleUpdator.CafeDataMap = GameManager.Instance.cafeDataLoader.Data;
        TitleUpdator.ComuDataMap = GameManager.Instance.comuDataLoader.Data;
        TitleUpdator.PaperDataMap = GameManager.Instance.paperDataLoader.Data;
        TitleUpdator.ReelsDataMap = GameManager.Instance.reelsDataLoader.Data;
        // Debug.Log("게임매니저 awake는 돌아가냐.. 2"); // 돌아감

        
        TitleUpdator.UpdateTitles();
        Debug.Log("게임매니저 awake는 돌아가냐.. 3"); // 안돌아감
        
        
        
        

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
    public void After30s_ActivateAlarm()
    {
        Invoke("ActivateAlarm",10f);
        // alarmText.SetActive(true);
        // alarmText.SetActive(true);
    }
    public void ActivateAlarm()
    {
        alarmText.SetActive(true);
        alarmActivated = true;
    }
    public void DeactivateAlarm()
    {
        alarmText.SetActive(false);
        alarmActivated = false;
    }
    
    

}
