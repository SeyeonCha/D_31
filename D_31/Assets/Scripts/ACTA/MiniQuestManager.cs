using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MiniQuestManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI QuestText;
    
    [SerializeField]
    [TextArea(3, 5)]
    private string[] QuestTexts;

    // Start is called before the first frame update
    void Start()
    {
        if (QuestText == null)
        {
            QuestText = GetComponent<TextMeshProUGUI>();
        }

        UpdateDayText();
    }

    // void Update()
    // {
    //     UpdateDayText();
    // }

    public void UpdateDayText()
    {
        if (QuestText == null) return;
        
        // 1. GameManager의 DayEnded 값을 가져옵니다.
        int currentDay = GameManager.DayEnded;

        // 2. DayEnded 값이 배열의 범위를 벗어나지 않는지 확인합니다.
        if (currentDay >= 0 && currentDay < QuestTexts.Length)
        {
            // 3. 해당하는 인덱스의 텍스트로 UI를 업데이트합니다.
            QuestText.text = QuestTexts[currentDay];
        }
        else
        {
            // 값이 배열 범위를 벗어날 경우의 오류 메시지 (선택 사항)
            QuestText.text = "Error: Day " + currentDay + " 텍스트 없음!";
        }
    }
}
