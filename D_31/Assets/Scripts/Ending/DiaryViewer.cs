using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DiaryViewer : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI diaryContentText; // 일기 내용 출력
    public TextMeshProUGUI questionText;     // [추가] "당신은... 생각이었는가?" 질문 텍스트

    [Header("Day Buttons")]
    public Button btnD31;
    public Button btnD30;
    public Button btnD14;
    public Button btnD1;

    private Image imgD31, imgD30, imgD14, imgD1;

    void Awake()
    {
        imgD31 = btnD31.GetComponent<Image>();
        imgD30 = btnD30.GetComponent<Image>();
        imgD14 = btnD14.GetComponent<Image>();
        imgD1 = btnD1.GetComponent<Image>();
    }

    void OnEnable()
    {
        // 1. 질문 텍스트 업데이트 [추가]
        UpdateQuestionText();

        // 2. 초기 일기 내용 표시
        ShowDiary("D-31");
    }

    // [추가] FinalManager의 선택에 따라 질문 텍스트의 [ ] 내용을 변경하는 함수
    private void UpdateQuestionText()
    {
        if (questionText == null) return;

        // 씬에서 FinalManager를 찾아 결과를 확인
        FinalManager finalManager = Object.FindObjectOfType<FinalManager>();
        
        if (finalManager != null)
        {
            // isYesSelected 값에 따른 텍스트 설정
            string selection = finalManager.IsYesSelected() ? "우주선 타기" : "우주선 타지 않기";

            questionText.text = $"당신은 <color=#FF0000>'{selection}'</color>를 선택했다.\n\n" +
                                "그러나, 그것은 정말 당신의 생각이었는가?\n\n" +
                                "미디어 환경 속에서 스스로를 검열하진 않았는가?\n\n" +
                                "다수의 의견에 그저 동조하기를 택하진 않았는가?";
        }
    }

    public void ShowDiary(string dayKey)
    {
        string content = PlayerPrefs.GetString(dayKey, "작성된 일기가 없습니다.");
        diaryContentText.text = content;

        UpdateButtonsTransparency(dayKey);
        Debug.Log($"[DiaryViewer] Displaying: {dayKey}");
    }

    private void UpdateButtonsTransparency(string selectedKey)
    {
        SetAlpha(imgD31, 0.4f); 
        SetAlpha(imgD30, 0.4f);
        SetAlpha(imgD14, 0.4f);
        SetAlpha(imgD1, 0.4f);

        switch (selectedKey)
        {
            case "D-31": SetAlpha(imgD31, 1.0f); break;
            case "D-30": SetAlpha(imgD30, 1.0f); break;
            case "D-14": SetAlpha(imgD14, 1.0f); break;
            case "D-1":  SetAlpha(imgD1, 1.0f); break;
        }
    }

    private void SetAlpha(Image img, float alpha)
    {
        if (img != null)
        {
            Color color = img.color;
            color.a = alpha;
            img.color = color;
        }
    }
}