using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleButtonUpdator : MonoBehaviour
{

    public List<NewsTitle> B_Titles;

    public List<NewsTitle> N_Titles;

    private static List<int> BN_idxs = new List<int> {0, 0, 0, 0, 0};

    // 나머지 플랫폼의 제목들도 저런 식으로 정의해주면 됨. 
    public List<CafeTitle> Cafe_Titles;

    private static List<int> Cafe_idxs = new List<int> {0, 0, 0, 0, 0};

    private int targetIndex;
    private int targetClass;

    public Dictionary<int, List<NewsData>> NewsDataMap;
    public Dictionary<int, List<CafeData>> CafeDataMap;

    List<int> B_curationData;
    List<int> N_curationData;
    List<int> Cafe_curationData;

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
    //     Cafe_curationData = GameManager.Instance.curation_data[2];
    //     Debug.Log($"Cafe 큐레이션해야되는 클래스 개수(7개?) : {Cafe_curationData.Count}");
    //     for (int i = 0; i < 3; i++)
    //     {
    //         targetClass = Cafe_curationData[i];
    //         targetIndex = Cafe_idxs[targetClass]++;
    //         Cafe_Titles[i].data = CafeDataMap[targetClass][targetIndex];
    //     }
    }

}
