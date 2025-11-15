using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using System.Text.RegularExpressions; 

public class PaperDataLoader : MonoBehaviour
{
    [SerializeField] 
    private TextAsset titleDataFile; // csv 파일 

    private Dictionary<int, List<PaperData>> PaperDataMap;
    public Dictionary<int, List<PaperData>> Data
    {
        get { return PaperDataMap; }
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
    
    public void LoadCsvData()
    {
        if (titleDataFile == null)
        {
            Debug.LogError("titleData CSV 파일이 GameManager에 연결되지 않았습니다!");
            return;
        }
        
        PaperDataMap = new Dictionary<int, List<PaperData>>(); // 데이터 틀 생성

        string fullText = titleDataFile.text;
        // 여기서 큰따옴표 안에 있는 \n을 <br> 로 바꿔야 해

        string[] rows = fullText.Split(new char[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
        Debug.Log($"rows.Length : {rows.Length}");
        for (int i = 1; i < rows.Length; i++) // 두번째 행부터. 
        {
            Debug.Log($"{rows[i]}");

            string[] columns = ParseCsvLine(rows[i]);
            Debug.Log($"columne 개수{columns.Length}");

            if (columns.Length >= 6 &&
                int.TryParse(columns[0], out int uniqueId) &&
                int.TryParse(columns[1], out int classId) && 
                int.TryParse(columns[4], out int year))
            {
                string title = columns[2]; // 따옴표 자동 제거됨
                string author = columns[3];
                string AI_T = columns[5];
                string AI_F = columns[6];


                PaperData data = new PaperData(uniqueId, classId, title, author, year, AI_T, AI_F); // 게시물 하나짜리 데이터

                if (!PaperDataMap.ContainsKey(classId)) // 클래스 별로 나누어서 저장
                    PaperDataMap[classId] = new List<PaperData>();

                PaperDataMap[classId].Add(data);
                Debug.Log($"논문 data 추가 : {data.title}, {data.classId}");
            }
        }

        Debug.Log($"CSV 데이터 맵 구축 완료. 총 {rows.Length - 1}개 행을 로드했습니다.\n");
        // Debug.Log($"{PaperDataMap[1][0].title}\n{PaperDataMap[1][0].AI_T}");

    }
}
