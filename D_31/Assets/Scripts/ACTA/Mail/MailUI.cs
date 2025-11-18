using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; // Button 컴포넌트 사용을 위해 필요

// 메일 리스트 프리팹에 붙을 스크립트 -> UI 조정
public class MailUI : MonoBehaviour
{
    // 데이터 받아서 프리팹 UI에 저장하는 스크립트
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI timeText;

    // 이 버튼이 활성화할 대상 오브젝트의 이름을 저장할 변수
    public string targetObjectName = "BrokerMail";

    // 버튼 컴포넌트 참조
    private Button button;

    void Awake()
    {
        button = titleText.gameObject.GetComponent<Button>();
    }
    // 이 함수를 외부에서 호출하여 대상 오브젝트를 설정하고 리스너를 추가합니다.
    public void SetupButton(string nameOfObjectToActivate)
    {
        targetObjectName = nameOfObjectToActivate;

        // 기존의 모든 리스너를 제거하고 새로운 리스너를 추가합니다.
        // button.onClick.RemoveAllListeners();
        // button.onClick.AddListener(ActivateTargetObject); 
    }
    // 버튼 클릭 시 호출될 실제 함수
    // public void ActivateTargetObject()
    // {
    //     // 1. 씬에서 이름으로 대상 오브젝트를 찾습니다.
    //     GameObject targetObject = GameObject.Find(targetObjectName);

    //     if (targetObject != null)
    //     {
    //         // 2. 오브젝트 활성화
    //         targetObject.SetActive(true);
    //         Debug.Log($"버튼 클릭: '{targetObjectName}' 오브젝트 활성화됨.");
    //     }
    //     else
    //     {
    //         Debug.LogError($"오류: 씬에서 '{targetObjectName}' 오브젝트를 찾을 수 없습니다.");
    //     }
    // }

    public void SetupUI(string title, string time)
    {
        // 텍스트 업데이트

        titleText.text = title;
        timeText.text = time;

    }
}
