using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleButtonUpdator : MonoBehaviour
{
    // 뉴스 Title
    public List<NewsTitle> B_Titles;
    public List<NewsTitle> N_Titles;
    private static List<int> BN_idxs = new List<int> {0, 0, 0, 0, 0};

    // 카페 Title
    public List<CafeTitle> Cafe_Titles;
    private static List<int> Cafe_idxs = new List<int> {0, 0, 0, 0, 0};

    // 커뮤 Title
    public List<ComuTitle> Comu_Titles;
    private static List<int> Comu_idxs = new List<int> {0, 0, 0, 0, 0};

    // 논문 Title
    public List<PaperTitle> Paper_Titles;
    private static List<int> Paper_idxs = new List<int> {0, 0, 0, 0, 0};

    // 릴스 Title
    public List<ReelsTitle> R_Titles;
    private static List<int> R_idxs = new List<int> {0, 0, 0, 0, 0};

    private int targetIndex;
    private int targetClass;

    public Dictionary<int, List<NewsData>> NewsDataMap;
    public Dictionary<int, List<CafeData>> CafeDataMap;
    public Dictionary<int, List<ComuData>> ComuDataMap;
    public Dictionary<int, List<PaperData>> PaperDataMap;
    public Dictionary<int, List<ReelsData>> ReelsDataMap;

    List<int> B_curationData;
    List<int> N_curationData;
    List<int> Cafe_curationData;
    List<int> Comu_curationData;
    List<int> Paper_curationData;
    List<int> Reels_curationData;

    // void Awake()
    // {
    //     // 뉴스 데이터 전체 가져오기
    //     NewsDataMap = GameManager.Instance.newsDataLoader.Data;
    //     if (NewsDataMap == null)
    //     {
    //         Debug.Log($"NewsDatMap not linked to TItle button updater..");
    //     }
    //     Debug.Log($"NewsDataMap uploaded in TitleButtonUpdator : {NewsDataMap[0][0].title}");
        
    //     // 카페 데이터 전체 가져오기
    //     CafeDataMap = GameManager.Instance.cafeDataLoader.Data;
    //     if (CafeDataMap == null)
    //     {
    //         Debug.Log($"CafeDatMap not linked to TItle button updater..");
    //     }
    //     Debug.Log($"CafeDataMap uploaded in TitleButtonUpdator : {CafeDataMap[0][0].title}");
    // }

    // void Update()
    // {
    //     UpdateTitles();
    // }

    public void UpdateTitles()
    {
        // Breaking 제목 show
        B_curationData = GameManager.Instance.curation_data[0];
        Debug.Log($"Breaking 큐레이션해야되는 클래스 개수(4개?) : {B_curationData.Count}");
        for (int i = 0; i < 4; i++)
        {
            targetClass = B_curationData[i];
            targetIndex = BN_idxs[targetClass]++;
            Debug.Log($" NewsDataMap[{targetClass}][{targetIndex}]의 제목 : {NewsDataMap[targetClass][targetIndex].title}");
            B_Titles[i].data = NewsDataMap[targetClass][targetIndex];

        }


        // News 제목 show
        N_curationData = GameManager.Instance.curation_data[1];
        Debug.Log($"News 큐레이션해야되는 클래스 개수(5개?) : {N_curationData.Count}");
        for (int i = 0; i < 5; i++)
        {
            targetClass = N_curationData[i];
            targetIndex = BN_idxs[targetClass]++;
            N_Titles[i].data = NewsDataMap[targetClass][targetIndex];
        }

        // Cafe 제목 show
        Cafe_curationData = GameManager.Instance.curation_data[2];
        Debug.Log($"Cafe 큐레이션해야되는 클래스 개수(7개?) : {Cafe_curationData.Count}");
        for (int i = 0; i < Cafe_Titles.Count; i++)
        {
            targetClass = Cafe_curationData[i];
            targetIndex = Cafe_idxs[targetClass]++;
            Cafe_Titles[i].data = CafeDataMap[targetClass][targetIndex];
        }

        // 커뮤니티 제목 show
        Comu_curationData = GameManager.Instance.curation_data[3];
        Debug.Log($"Comu 큐레이션해야되는 클래스 개수(16개?) : {Comu_curationData.Count}");
        for (int i = 0; i < Comu_Titles.Count; i++)
        {
            targetClass = Comu_curationData[i];
            targetIndex = Comu_idxs[targetClass]++;
            Comu_Titles[i].data = ComuDataMap[targetClass][targetIndex];
        }

        // 논문 제목 show
        Paper_curationData = GameManager.Instance.curation_data[4];
        Debug.Log($"Paper 큐레이션해야되는 클래스 개수(4개?) : {Paper_curationData.Count}");
        for (int i = 0; i < Paper_Titles.Count; i++)
        {
            targetClass = Paper_curationData[i];
            targetIndex = Paper_idxs[targetClass]++;
            Paper_Titles[i].data = PaperDataMap[targetClass][targetIndex];
        }

        // 릴스 제목 show
        Reels_curationData = GameManager.Instance.curation_data[5];
        Debug.Log($"News 큐레이션해야되는 클래스 개수(5개?) : {Reels_curationData.Count}");
        for (int i = 0; i < R_Titles.Count; i++)
        {
            targetClass = Reels_curationData[i];
            targetIndex = R_idxs[targetClass]++;
            R_Titles[i].data = ReelsDataMap[targetClass][targetIndex];
            Debug.Log($"릴스 제목 show : {R_Titles[i].data.title}");
        }
    }

}
