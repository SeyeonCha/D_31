using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Text.RegularExpressions; 

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    // CSV 데이터 로드 파트
    [SerializeField]
    public NewsDataLoader newsDataLoader;
    public CafeDataLoader cafeDataLoader;
    // **나머지 플랫폼의 데이터 로더도 정의. 

    [SerializeField]
    public TitleButtonUpdator TitleUpdator;

    // 큐레이션 관련
    public List<List<int>> curation_data;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // 씬이 바뀌어도 파괴되지 않도록 설정
        DontDestroyOnLoad(gameObject);

        // 큐레이션 데이터 초기화
        curation_data = new List<List<int>>()
        {
            new List<int> {3,4,3,4},
            new List<int> {0,2,2,1,1},
            new List<int> {0, 1, 1, 1, 1, 2, 3} // 카페 초기화 클래스s
            // **나머지 플랫폼의 초기 클래스 데이터도 여기 입력. 
        };

        // CSV 데이터 읽어오기
        newsDataLoader.LoadCsvData(); // 뉴스
        cafeDataLoader.LoadCsvData(); // 카페


        // ** 나머지 플랫폼의 데이터로더도 여기서 실행
        TitleUpdator.NewsDataMap = newsDataLoader.Data;
        TitleUpdator.CafeDataMap = cafeDataLoader.Data;

        TitleUpdator.UpdateTitles();

        // 제목 띄우기


    }
    

}
