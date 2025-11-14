using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using System.Text.RegularExpressions; 

public class CafeDataLoader : MonoBehaviour
{
    [SerializeField] 
    private TextAsset titleDataFile; // csv 파일 

    private Dictionary<int, List<CafeData>> CafeDataMap;
    public Dictionary<int, List<CafeData>> Data
    {
        get { return CafeDataMap; }
    }

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
        
        CafeDataMap = new Dictionary<int, List<CafeData>>(); // 데이터 틀 생성

        string fullText = titleDataFile.text;
        // 여기서 큰따옴표 안에 있는 \n을 <br> 로 바꿔야 해

        string[] rows = fullText.Split(new char[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 1; i < rows.Length; i++) // 두번째 행부터. 
        {
            // Debug.Log($"{rows[i]}");

            string[] columns = ParseCsvLine(rows[i]);
            // Debug.Log($"{columns[0]}\n{columns[1]}\n{columns[2]}\n{columns[3]}\n{columns[4]}\n{columns[5]}\n{columns[6]}");

            if (columns.Length >= 8 &&
                int.TryParse(columns[0], out int uniqueId) &&
                int.TryParse(columns[1], out int classId) && 
                int.TryParse(columns[4], out int views) &&
                int.TryParse(columns[5], out int like))
            {
                string title = columns[2]; // 따옴표 자동 제거됨
                string writer = columns[3];
                string content = columns[6];

                // 댓글 데이터 가져오기

                List<List<string>> comments = new List<List<string>>(); // <name, comments>를 원소로 하는 List

                for (int j = 7;j<columns.Length; j+=2)
                {
                    List<string> commentPair = new List<string> { columns[j], columns[j+1] }; // 작성자, 댓글 pair
                    
                    comments.Add(commentPair);
                }

                CafeData data = new CafeData(uniqueId, classId, title, writer, views, like, content, comments); // 게시물 하나짜리 데이터

                if (!CafeDataMap.ContainsKey(classId)) // 클래스 별로 나누어서 저장
                    CafeDataMap[classId] = new List<CafeData>();

                CafeDataMap[classId].Add(data);
            }
        }

        Debug.Log($"CSV 데이터 맵 구축 완료. 총 {rows.Length - 1}개 행을 로드했습니다.\n");
        Debug.Log($"{CafeDataMap[1][0].title}\n{CafeDataMap[1][0].content}");

    }
}
