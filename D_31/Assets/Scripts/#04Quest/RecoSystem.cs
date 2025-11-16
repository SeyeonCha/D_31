using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq; 

public class RecoSystem : MonoBehaviour
{
    private List<int> n_post_per_type = new List<int>(){4, 5, 4, 4}; // 타입별 게시물 개수
    private int n_types = 4; // 추천 시스템 적용 플랫폼 개수 = 4
    private const string COUNT_KEY_PREFIX = "Class_"; 
    private const int NUM_CLASSES = 5;
    private int total_scrap_count; // 추천 시스템 총 스크랩 수.
    private int k; // 다음 게시물 수 정하는 상수. 
    private int TOTAL_POSTS = 17;

    public List<List<int>> CafeCurationClasses;
    public List<List<int>> ComuCurationClasses;

    public void Awake()
    {
        // 데이별 카페 큐레이션 클래스들 설정
        CafeCurationClasses = new List<List<int>>()
        {
            new List<int>() {0, 1, 1, 1, 1, 2, 3},
            new List<int>() {0, 2, 2, 2, 2, 3, 4},
            new List<int>() {1, 2, 3, 3, 3, 4, 4},
            new List<int>() {2, 3, 3, 4, 4, 4, 4}
        };
        // 데이별 커뮤니티 큐레이션 클래스들 설정
        ComuCurationClasses = new List<List<int>>()
        {
            new List<int>() {0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 3, 3, 4, 4},
            new List<int>() {0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4},
            new List<int>() {0, 0, 1, 1, 2, 2, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4},
            new List<int>() {1, 1, 1, 1, 1, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4}
        };
    }

    public List<List<int>> CurationCalculator() // end day 버튼이 클릭되면 큐레이션 데이터 계산 & 반환
    {
        
        List<List<int>> CurationData = new List<List<int>>();

        total_scrap_count = PlayerPrefs.GetInt("Total_Count",1); // 9여야 정상임.
        k = TOTAL_POSTS/total_scrap_count; // 추천 시스템 스크랩 하나도 안하면 오류남. 
        
        // 1. 클래스별 다음 날 게시글 수 계산  ---> nextDayClassPosts : List<int>
        List<int> nextDayClassPosts = new List<int>();

        for (int i = 0; i < NUM_CLASSES; i++)
        {
            string countKey = COUNT_KEY_PREFIX + i + "_Count"; // prefs에서 데이터를 가져오기 위한 키워드
            // 클래스별 게시글 스크랩 수 가져와 다음날 해당 클래스의 게시글 수 계산하고 배열에 저장
            int scrapCount = PlayerPrefs.GetInt(countKey,0); 
            nextDayClassPosts.Add((int)(k * scrapCount));  // 소수점 이하 버리고 리스트에 저장. 
            
        }
        Debug.Log($"RECO 할당 게시물 수 : {nextDayClassPosts.Sum()}, 필요한 총 게시물 수 : {TOTAL_POSTS}");
        int assigned_sum = nextDayClassPosts.Sum();
        
        int n_randomAssign = 0;
        for (int i=0;i<(TOTAL_POSTS - assigned_sum);i++) // 할당 안된 게시물 수만큼 반복 -> 클래스 랜덤 할당
        { 
            // 0-4중 
            int randomNumber = Random.Range(0, 5); // 0, 1, 2, 3, 4 중 하나의 정수를 반환
            nextDayClassPosts[randomNumber]++;
            n_randomAssign++;
            
        }
        Debug.Log($"---> {n_randomAssign} times Assigned Randomly");

        Debug.Log($"REC_다음 날 클래스별 게시물 수 : " + string.Join(", ", nextDayClassPosts)+ $" *총 {nextDayClassPosts.Sum()}개");

        // 2. 클래스별 게시글 수를 플랫폼에 할당   --> 2차원 리스트 CurationData에 저장. 
        int assigned_posts_count  = 0; // 클래스 할당 완료한 총 게시글 수 저장 변수. 

        for (int i = 0; i<n_types;i++) // 플랫폼 수만큼 반복 (속보 -> 뉴스 -> 논문 -> 릴스)
        {
            // 0으로 초기화된 원소 5개의 배열 생성 : 이 플랫폼의 클래스별 게시물 개수 저장. 
            List<int> type_list = new List<int>(); // 게시물로 넣을 클래스Id의 리스트 ex. [1,2,2] -> 1-1개, 2-2개, 나머지 0개
            
            for (int j=0;j<n_post_per_type[i];j++) // 해당 유형의 게시글 수만큼 반복 ex. 뉴스속보는 4번 반복
            {
                if (assigned_posts_count >= TOTAL_POSTS) 
                {
                    break;
                }

                // 2-2. 유효 클래스 리스트 생성
                List<int> validIdx = new List<int>(); // 유효한 클래스 저장 리스트 생성
                for (int t=0; t < NUM_CLASSES; t++)
                {
                    if (nextDayClassPosts[t]>0) // 해당 클래스의 다음날 게시글 개수가 0이 아니면 
                    {
                        validIdx.Add(t); // 유효 인덱스에 클래스번호 추가
                    }
                } 

                if (validIdx.Count > 0)
                {
                    // 2-3. 인덱스 랜덤 선택, 해당 인덱스(클래스)의 개수 + 1 (이 플랫폼 내에 해당 클래스 게시물 수 + 1)
                    int randomIdx = validIdx[Random.Range(0,validIdx.Count)];

                    // type_list[randomIdx]++;
                    type_list.Add(randomIdx);
                    nextDayClassPosts[randomIdx]--; // 할당된 만큼 다음날 게시글 수 차감
                    assigned_posts_count++;

                }
                else 
                {
                    break;
                }
                
            }
            CurationData.Add(type_list);
            Debug.Log($"Platform {i} 게시물 별 클래스: " + string.Join(", ", type_list));


        }
        int currentDay = GameManager.DayEnded;
        CurationData.Add(CafeCurationClasses[currentDay]);
        CurationData.Add(ComuCurationClasses[currentDay]);


        return CurationData;
    }
}
