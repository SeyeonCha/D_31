using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
// 브로커 메일 패널에 붙음. 
public class BrokerMailUI : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI content;

    // 연결 오브젝트들
    public GameObject PlayerThinking;
    public TextMeshProUGUI ThinkingText;
    public GameObject InstructionText;
    public GameObject Button3;
    public GameObject Button2;

    public GameObject MyReply1;
    public GameObject bReply1;
    public GameObject MyReply2;
    public GameObject bReply2;
    public GameObject bReply3;

    public ActaManager actaManager;

    private Coroutine thinkingRoutine;

    private bool IsThinking = false;

    private int mode = 0; // 0 : 돈 얼마 주면 신분상승, 1 : 다른 위험한 방법 제안
    // private Coroutine thinkingRoutine; // 키 입력 코루틴 참조를 위한 변수

    private int clickedButton;


    private void OnEnable() // 브로커 메일 창이 활성화되면
    {
        changeText(); // 메일 제목, 내용 입력

        if (thinkingRoutine != null) StopCoroutine(thinkingRoutine);
        StopAllCoroutines();
        thinkingRoutine = StartCoroutine(StartThinkingSequence(5f));
    }
    public void changeText() // 모드에 따라 메일 제목, 내용 바꾸기. <- MailManager에서 호출
    {
        if (mode == 0)
        {
            title.text = "요청하신 건에 대한 답변";
            content.text = @"사정은 이해했습니다.

화성행 로켓 탑승을 위한 신분 승급은 공식적으로는 불가능한 일이지만
그 외의 방법이 전혀 없는 것은 아닙니다.

다만, 이 과정에는 총 18억 크레딧의 자금이 필요합니다.

준비가 끝나면 연락주시기 바랍니다.
다음 절차는 그때 전달하겠습니다.

시간이 많지 않습니다. 빠른 판단 내리시기 바랍니다.";
        }
        else if (mode == 1)
        {
            title.text = "Re: Re: Re: 요청하신 건에 대한 답변";
            content.text = @"충분히 고민하셨을 거라 생각합니다.
이제 결정을 내려주십시오.

저희도 오래 기다릴 수는 없습니다.";
        }
    }
    private void Update()
    {
        if (IsThinking && mode == 0 && Input.GetKeyDown(KeyCode.Space))
        {
            IsThinking = false;
            mode += 1;
            PlayerThinking.SetActive(false);

            // 첫번째 답장 패널 키기
            string first_reply = "솔직히 말씀드리면, 18억 크레딧은 지금 제가 감당하기는 어려운 금액입니다... \n혹시 다른 방법이 있을까요? 정말로 간절합니다. \n답변 기다리겠습니다.";

            MyReply1.GetComponent<ReplyHandler>().changeText(first_reply);
            MyReply1.SetActive(true);
            Invoke("BrokerReplyOn1", 3f);
            thinkingRoutine = StartCoroutine(StartThinkingSequence(8f));
        }
        else if (IsThinking && mode == 1 && Input.GetKeyDown(KeyCode.Space))
        {
            // IsThinking = false;
            mode += 1;
            // ThinkingText.gameObject.SetActive(false);
            InstructionText.SetActive(false);
            Button2.SetActive(false);
            Button3.SetActive(true);
            

            
        }
        else if (IsThinking && mode == 2 && (clickedButton == 1 || clickedButton == 2 || clickedButton == 3))
        {
            IsThinking = false;
            mode += 1;
            PlayerThinking.SetActive(false);

            string player_r2 = "";
            if (clickedButton == 1) // 할게요
            {
                player_r2 = "조금 위험하더라도 살아남고 싶습니다.\n구체적인 내용 부탁드립니다.";
            }
            else if (clickedButton == 2) // 안 할게요
            {
                player_r2 = "죄송하지만 요청은 철회하겠습니다. 관련 내용은 외부에 공유하지 않을테니 양해 부탁드립니다.";
            }
            else if (clickedButton == 3) // 고민
            {
                player_r2 = "조금 더 생각할 시간이 필요할 것 같습니다. 조금만 기다려주시면 곧 답변드리겠습니다.";
            }
            MyReply2.GetComponent<ReplyHandler>().changeText(player_r2);
            MyReply2.SetActive(true);
            Invoke("BrokerReplyOn2", 3f);

            
        }
        if (actaManager.alarmActivated && mode == 3)
        {
            MyReply1.SetActive(false);
            bReply1.SetActive(false);
            MyReply2.SetActive(false);
            bReply2.SetActive(false);
            bReply3.SetActive(true);

        }
        
    }
    public void GetClickedButton(int n) // 생각 패널에 있는 댓글에 onclick버튼 이벤트로 연결
    {
        clickedButton = n;
    }
    private void BrokerReplyOn1()
    {
        string broker_r = "귀하의 사정은 이해합니다만, 금액 조정은 불가능합니다. 예외는 없습니다.\n\n다만, 한 가지 다른 방법이 있습니다. \n이 방식은 일정 수준 이상의 위험을 수반하지만, 그에 상응하는 크레딧은 보장드립니다. \n원하시나요? 답장 주시면 구체적인 내용 안내드리겠습니다.";

        bReply1.GetComponent<ReplyHandler>().changeText(broker_r);
        bReply1.SetActive(true);  
    }
    private void BrokerReplyOn2()
    {
        string broker_r = "";
        if (clickedButton == 1) // 할게요
        {
            broker_r = "링크 주소";
        }
        else if (clickedButton == 2) // 안 할게요
        {
            broker_r = "아쉽네요. 그러시죠.";
        }
        else if (clickedButton == 3) // 고민
        {
            broker_r = "시간이 많지 않습니다. 빠른 판단 내리시기 바랍니다.";
            Invoke("BrokerReplyOn3", 3f);
        }
        

        bReply2.GetComponent<ReplyHandler>().changeText(broker_r);
        bReply2.SetActive(true);  
    }
    private void BrokerReplyOn3()
    {
        actaManager.MailAlarm();

        // MyReply1.SetActive(false);
        // bReply1.SetActive(false);
        // MyReply2.SetActive(false);
        // bReply2.SetActive(false);

        // bReply3.GetComponent<ReplyHandler>().changeText(broker_r);
        // bReply3.SetActive(true);  
    }
    

    
    private IEnumerator StartThinkingSequence(float delay)
    {
        // 1. 지정된 시간(5초)만큼 기다립니다.
        yield return new WaitForSeconds(delay);

        // 2. 시간이 지나면 PlayerThinking을 활성화합니다.
        // Mode 0일 때만 활성화하도록 조건을 추가했습니다. (필요 없으면 제거 가능)
        if (PlayerThinking != null)
        {
            PlayerThinking.SetActive(true);
            if (mode == 0)
            {
                ThinkingText.text = "하... 진짜 돈이 없는데...그렇다고 가만있을 수도 없고... \n그냥 어떻게든 답장이라도 보내보자. 밑져야 본전이지...";
            }
            else if (mode == 1)
            {
                ThinkingText.text = "어떻게 하지.....?";
            }
            ThinkingText.gameObject.SetActive(true);
            InstructionText.SetActive(true);
            Button3.SetActive(true);
            Button3.SetActive(false);
            Debug.Log($"[타이머 완료] {delay}초 후 PlayerThinking 패널 활성화.");

            IsThinking = true;
        }
    }
    

    
}
