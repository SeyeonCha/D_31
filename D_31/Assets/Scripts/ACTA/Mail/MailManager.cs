using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MailManager : MonoBehaviour
{
    public Transform content;
    public GameObject itemPrefab;

    public GameObject contentText;
    public TypingEffect contentTypingEffect;

    public GameObject sceneTargetObject; // Broker Mail

    public GameObject BrokerMailTitle_0; // 요청하신 건에 대한 답변 -> 패널임. 
    public GameObject BrokerMailTitle_1; // 시간이 없습니다. 빨리 결정하세요
    public GameObject BrokerMailTitle_2; // 신분상승 비용 깎아주겠다

    public BrokerMailUI brokerMail;
    public int mode = 0;


    // public BrokerMailUI brokerMail;


    public void OnEnable()
    {
        Debug.Log($"Mail Manager Awaked : {mode}");
        if (GameManager.DayEnded == 2)
        {
            if (mode == 0) 
            {
                BrokerMailTitle_0.SetActive(true);

            }
            else if (mode == 3)
            {
                BrokerMailTitle_1.SetActive(true);
            }
        }
        
        
    }

    private void Update()
    {
        if (contentTypingEffect.IsTyping) // 타이핑 중이면
        {
            if (Input.GetKeyDown(KeyCode.Space)) // 키가 눌렸을 때
            {
                contentTypingEffect.SkipTyping(contentTypingEffect.GetComponent<TMP_Text>().text);
                // 현재 텍스트 타이핑을 스킵
            }
        }
    }
    
    // public void AddMailToTop(string title, string time) // 메일 리스트에 메일 추가하는 함수
    // {
    //     // // MailUI mailUI = newItem.GetComponent<MailUI>();
    //     // mailUI.SetupUI(title, time);
    //     // mailUI.SetupButton(sceneTargetObject.name);
    //     // newItem.transform.SetAsFirstSibling();
    // }

    public void StartMailTyping(GameObject content_text) // 타이핑 적용할 텍스트 게임오브젝트 입력 -> 메일 타이핑 시작
    {
        contentTypingEffect = content_text.GetComponent<TypingEffect>();
        contentTypingEffect.StartTyping(contentTypingEffect.GetComponent<TMP_Text>().text);

    }
    // public void MailAlarm()
    // {
    //     Invoke("ActivateAlarm",10f);
    //     // alarmText.SetActive(true);
    //     // alarmText.SetActive(true);
    // }
    // private void ActivateAlarm()
    // {
    //     alarmText.SetActive(true);
    //     alarmActivated = true;
    // }


}
