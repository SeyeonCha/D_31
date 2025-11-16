using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayScrapData : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI displayTextMesh; // 디스플레이용 Text 오브젝트 (뱃지 텍스트)

    private const int NUM_CLASSES = 5;
    private const string COUNT_KEY_PREFIX = "Class_";

    void Start()
    {
        UpdateDisplay(); // 시작하자마자 값 띄우기. 
    }
    
    public void ScrapCounter(int classId) 
    {
        // 플랫폼별 매니저의 ScrapButtonClicked 마지막에서 호출됨
        
        // 해당 클래스의 스크랩 수 + 1
        string countKey = COUNT_KEY_PREFIX + classId + "_Count";
        int currentCount = PlayerPrefs.GetInt(countKey,0);
        currentCount++;
        PlayerPrefs.SetInt(countKey,currentCount);

        // 토탈 스크랩 수도  + 1
        int totalCount = PlayerPrefs.GetInt("Total_Count",0);
        totalCount++;
        PlayerPrefs.SetInt("Total_Count",totalCount);

        // 클래스별 스크랩 수 디버깅
        Debug.Log(
            $"{0} counts: {PlayerPrefs.GetInt(COUNT_KEY_PREFIX + 0 + "_Count", 0)}, "+ 
            $"{1} counts: {PlayerPrefs.GetInt(COUNT_KEY_PREFIX + 1 + "_Count", 0)}, "+
            $"{2} counts: {PlayerPrefs.GetInt(COUNT_KEY_PREFIX + 2 + "_Count", 0)}, "+
            $"{3} counts: {PlayerPrefs.GetInt(COUNT_KEY_PREFIX + 3 + "_Count", 0)}, "+
            $"{4} counts: {PlayerPrefs.GetInt(COUNT_KEY_PREFIX + 4 + "_Count", 0)}\n" + 
            $"Total Count : {PlayerPrefs.GetInt("Total_Count",0)}"
            
        );

        PlayerPrefs.Save();
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        
        int total_scrap = PlayerPrefs.GetInt("Total_Count",0);
        if (displayTextMesh != null)
        {
            displayTextMesh.text = $"{total_scrap}"; // 현재 PlayerPrefs에 있는 total_count 값을 디스플레이함
        }
        else
        {
            Debug.Log("displayTextMesh not found...");
        }
    }
}
