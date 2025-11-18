using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// 메일 프리팹에 붙을 스크립트 -> UI 조정
public class MailUI : MonoBehaviour
{
    // 데이터 받아서 프리팹 UI에 저장하는 스크립트
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI timeText;


    public void SetupUI(string title, string time)
    {
        // 텍스트 업데이트

        titleText.text = title;
        timeText.text = time;


    }
}
