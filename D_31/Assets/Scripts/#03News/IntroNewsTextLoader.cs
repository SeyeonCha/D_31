using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using System.Text.RegularExpressions; 

public class IntroNewsTextLoader : MonoBehaviour
{
    [SerializeField] 
    public TextAsset CsvTextFile; // 31일 csv 파일 
    [SerializeField]
    private TextMeshProUGUI HeadlineText;

    private SceneFlowManager sceneFlowManager;

    private Dictionary<int, List<string>> IntroNewsDataMap;
    public Dictionary<int, List<string>> Data
    {
        get { return IntroNewsDataMap; }
    }

    private void Awake()
    {
        sceneFlowManager = GetComponent<SceneFlowManager>();
        
        LoadCsvData(); // 전체 데이터 저장

        int d = GameManager.DayEnded; // 게임매니저에서 데이 정보 가져오기

        HeadlineText.text = IntroNewsDataMap[d][0].Replace("<n>","\n"); // 헤드라인 텍스트 입력


        string AnchorText = IntroNewsDataMap[d][1]; // 앵커라인 텍스트

        string[] delimiter = new string[] { "<n>" };
        string[] result = AnchorText.Split(delimiter, System.StringSplitOptions.RemoveEmptyEntries);

        sceneFlowManager.dialogueSentences = result;
        
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
        if (CsvTextFile == null)
        {
            Debug.LogError("CsvTextFile 파일이 연결되지 않았습니다!");
            return;
        }
        IntroNewsDataMap = new Dictionary<int, List<string>>(); // 데이터 틀 생성 : <day: "HeadLine", "AnchorLine">
        string fullText = CsvTextFile.text;

        string[] rows = fullText.Split(new char[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
        Debug.Log($"행 개수 : {rows.Length}");
        for (int i = 1; i < rows.Length; i++) // 두번째 행부터. 
        {
            string[] columns = ParseCsvLine(rows[i]);
            int Day = i-1;
            string headLine = columns[1];
            string anchorLines = columns[2];

            List<string> Lines = new List<string>() {headLine, anchorLines};

            if (!IntroNewsDataMap.ContainsKey(Day)) // 클래스 별로 나누어서 저장
                IntroNewsDataMap[Day] = new List<string>();

            IntroNewsDataMap[Day] = Lines;

        }
    }
}
