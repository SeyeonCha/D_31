using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using System.Text.RegularExpressions; 

public class NewsDataLoader : MonoBehaviour
{
    [SerializeField] 
    public TextAsset titleDataFile31; // 31일 csv 파일 
    [SerializeField] 
    public TextAsset titleDataFile30; // 30일 csv 파일 
    // [SerializeField] 
    // public TextAsset titleDataFile14; // 14일 csv 파일 
    // [SerializeField] 
    // public TextAsset titleDataFile4; // 4일 csv 파일 

    private Dictionary<int, List<NewsData>> NewsDataMap;
    public Dictionary<int, List<NewsData>> Data
    {
        get { return NewsDataMap; }
    }
    
    // private void Awake()
    // {
    //     LoadCsvData();
    // }
    
    private static string[] ParseCsvLine(string line)
    {
        // 따옴표로 감싸진 문자열 안의 쉼표는 무시하고 split
        var pattern = @",(?=(?:[^""]*""[^""]*"")*[^""]*$)";
        var result = Regex.Split(line, pattern);
        for (int i = 0; i < result.Length; i++)
        {
            // 앞뒤 공백 및 따옴표 제거
            result[i] = result[i].Trim().Trim('"');
        }
        return result;
    }
    
    public void LoadCsvData(TextAsset titleDataFile)
    {
        if (titleDataFile == null)
        {
            Debug.LogError("titleData CSV 파일이 GameManager에 연결되지 않았습니다!");
            return;
        }
        
        NewsDataMap = new Dictionary<int, List<NewsData>>(); // 데이터 틀 생성

        string fullText = titleDataFile.text;

        string[] rows = fullText.Split(new char[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        Debug.Log($"num of rows : {rows.Length}");
        for (int i = 1; i < rows.Length; i++) // 두번째 행부터. 
        {
            Debug.Log($"{rows[i]}");

            string[] columns = ParseCsvLine(rows[i]);
            // Debug.Log($"{columns[0]}\n{columns[1]}\n{columns[2]}\n{columns[3]}\n{columns[4]}\n{columns[5]}\n{columns[6]}");

            if (columns.Length >= 8 &&
                int.TryParse(columns[0], out int uniqueId) &&
                int.TryParse(columns[1], out int classId) && 
                int.TryParse(columns[4], out int like) &&
                int.TryParse(columns[5], out int dislike))
            {
                string title = columns[2]; // 따옴표 자동 제거됨
                string reporter = columns[3];
                string content = columns[6];

                // 댓글 데이터 가져오기

                List<List<string>> comments = new List<List<string>>(); // <name, comments>를 원소로 하는 List

                for (int j = 7;j<columns.Length; j+=2)
                {
                    List<string> commentPair = new List<string> { columns[j], columns[j+1] }; // 작성자, 댓글 pair
                    
                    comments.Add(commentPair);
                }

                NewsData data = new NewsData(uniqueId, classId, title, reporter, like, dislike, content, comments); // 게시물 하나짜리 데이터

                if (!NewsDataMap.ContainsKey(classId)) // 클래스 별로 나누어서 저장
                    NewsDataMap[classId] = new List<NewsData>();

                NewsDataMap[classId].Add(data);
            }
        }

        Debug.Log($"CSV 데이터 맵 구축 완료. 총 {NewsDataMap.Sum(kvp => kvp.Value.Count)}개 행을 로드했습니다.\n");
        Debug.Log($"{NewsDataMap[1][0].title}\n{NewsDataMap[1][0].content}");

    }
}
