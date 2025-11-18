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
    
    //[SerializeField]
    //private TextMeshProUGUI HeadlineText;
    
    [SerializeField] 
    private Image newsImage;

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

        sceneFlowManager.headlineSentence = IntroNewsDataMap[d][0].Replace("<n>","\n"); // 헤드라인 텍스트 입력

        string AnchorText = IntroNewsDataMap[d][1]; // 앵커라인 텍스트

        string[] delimiter = new string[] { "<n>" };
        string[] result = AnchorText.Split(delimiter, System.StringSplitOptions.RemoveEmptyEntries);

        sceneFlowManager.dialogueSentences = result;

        ChangeNewsImage(d); // 뉴스 이미지 로드 및 변경
    }

    private void ChangeNewsImage(int day)
    {
        if (newsImage == null)
        {
            Debug.LogError("뉴스 이미지를 표시할 Image 컴포넌트가 연결되지 않았습니다!");
            return;
        }

        string imagePath = $"news_screen/{day}";
        Sprite newSprite = Resources.Load<Sprite>(imagePath);

       if (newSprite != null)
        {
            // 로드 성공 시 이미지 변경
            newsImage.sprite = newSprite;
            newsImage.SetNativeSize();

            Debug.Log($"뉴스 이미지 경로: {imagePath}, 크기를 네이티브 사이즈로 설정했습니다.");
        }
        else
        {
            Debug.LogError($"Resources 폴더에서 경로: '{imagePath}'의 스프라이트를 로드할 수 없습니다. 이미지 파일이 '{imagePath}' 경로에 있는지, 확장자 없이 이름이 '{day}'인지 확인하세요.");
        }
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
