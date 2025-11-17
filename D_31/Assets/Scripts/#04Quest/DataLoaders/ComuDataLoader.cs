using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using System.Text.RegularExpressions;

public class ComuDataLoader : MonoBehaviour
{
    [SerializeField] 
    public TextAsset titleDataFile31; // 31일 csv 파일 
    [SerializeField] 
    public TextAsset titleDataFile30; // 30일 csv 파일 
    // [SerializeField] 
    // public TextAsset titleDataFile14; // 14일 csv 파일 
    // [SerializeField] 
    // public TextAsset titleDataFile4; // 4일 csv 파일 

    private Dictionary<int, List<ComuData>> ComuDataMap;
    public Dictionary<int, List<ComuData>> Data
    {
        get { return ComuDataMap; }
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

    public void LoadCsvData(TextAsset titleDataFile)
    {
        if (titleDataFile == null)
        {
            Debug.LogError("titleData CSV 파일이 GameManager에 연결되지 않았습니다!");
            return;
        }
        
        ComuDataMap = new Dictionary<int, List<ComuData>>(); // 데이터 틀 생성

        string fullText = titleDataFile.text;

        string[] rows = fullText.Split(new char[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
        Debug.Log($"rows.Length : {rows.Length}");
        for (int i = 1; i < rows.Length; i++) // 두번째 행부터. 
        {
            // Debug.Log($"row[{i}] : {rows[i]}");

            string[] columns = ParseCsvLine(rows[i]);
            // Debug.Log($"{columns[0]}\n{columns[1]}\n{columns[2]}\n{columns[3]}\n{columns[4]}\n{columns[5]}\n{columns[6]}");

            if (columns.Length >= 8 &&
                int.TryParse(columns[0], out int uniqueId) &&
                int.TryParse(columns[1], out int classId) && 
                int.TryParse(columns[4], out int like) &&
                int.TryParse(columns[6], out int c_num))
            {
                string title = columns[2]; // 따옴표 자동 제거됨
                string writer = columns[3];
                string content = columns[5];

                // 댓글 데이터 가져오기
            
                List<CommentData> comments = new List<CommentData>();  // 댓글 리스트, CommentData : name, content, n_reply, rname, rcontent

                for (int j = 0; j < c_num;j++) // 댓글 개수만큼 반복
                {
                    int name_idx = 7 + j * 3;// 7 -> 10 -> 13 또는 7 -> 10 또는 7
                    if (name_idx + 2 < columns.Length) // 대댓글 수가 존재하는지
                    {
                        if (columns[name_idx+2] == "1")
                        {
                            comments.Add(new CommentData(columns[name_idx], columns[name_idx+1], 1, columns[name_idx+3], columns[name_idx+4])); // 대댓글도 저장
                        }
                        else if (columns[name_idx+2] == "0")
                        {
                            comments.Add(new CommentData(columns[name_idx], columns[name_idx+1])); // 이름이랑 내용만 저장
                        }
                        
                    }
                    else {
                        Debug.Log("대댓글 개수 열이 없음..");
                    }

                }

                ComuData data = new ComuData(uniqueId, classId, title, writer, like, content, c_num, comments); // 게시물 하나짜리 데이터

                if (!ComuDataMap.ContainsKey(classId)) // 클래스 별로 나누어서 저장
                    ComuDataMap[classId] = new List<ComuData>();

                ComuDataMap[classId].Add(data);
                // Debug.Log($"comu data 추가 : {data.title}");
            }
        }

        Debug.Log($"CSV 데이터 맵 구축 완료. 총 {ComuDataMap.Sum(kvp => kvp.Value.Count)}개 행을 로드했습니다.\n");
        // Debug.Log($"{ComuDataMap[1][0].title}\n{ComuDataMap[1][0].content}");

    }
}
