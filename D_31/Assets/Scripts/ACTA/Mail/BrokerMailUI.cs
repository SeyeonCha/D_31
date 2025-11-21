using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

// 브로커 메일 패널에 붙음. <-- 데이 14부터 활성화될듯. 
public class BrokerMailUI : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI content;

    // 연결 오브젝트들

    // 혼잣말 패널, 혼잣말 텍스트, 선택 버튼들
    public GameObject PlayerThinking;
    public TextMeshProUGUI ThinkingText;
    public GameObject InstructionText;
    public GameObject Button3;
    public GameObject Button2;
    public GameObject Button22;

    private bool IsThinking = false; // 혼잣말 패널 활성화 여분

    // 브로커 메일창에 보여질 답장들 (플레이어 -> 브로커 -> 플레이어 -> 브로커)
    public GameObject MyReply1;
    public GameObject bReply1;
    public GameObject MyReply2;
    public GameObject bReply2;

    public ActaManager actaManager;
    public MailManager mailManager;

    private Coroutine thinkingRoutine;

    [SerializeField]
    private Sprite bReply1_image; // 브로커 답글 이미지1
    [SerializeField]
    private Sprite bReply1_image2; // 브로커 답장 이미지 2
    public ContentScroller scroller;

    // public GameObject linkButton ;
    public GameObject sendButton;
    public GameObject returnInstruction;
    public GameObject returnInstruction2;
    

    // public int mode = 0; // 시나리오 모드
    // 0 : 돈 얼마 주면 신분상승
    // 1 : 다른 위험한 방법 제안

    private int clickedButton;


    private void OnEnable() // 브로커 메일 창이 활성화되면 (데이별로 한번만 활성화될듯?)
    {
        changeText(); // 메일 제목, 내용 입력 (mode 0)
        Debug.Log($"현재 메일 모드 : {mailManager.mode}");

        // 7초 뒤에 혼잣말 패널 활성화 코루틴
        if (thinkingRoutine != null) StopCoroutine(thinkingRoutine);
        StopAllCoroutines();
        float time;
        if (mailManager.mode == 3) 
        {
            time = 6f;
        }
        else
        {
            time = 11f;
        }
        thinkingRoutine = StartCoroutine(StartThinkingSequence(time));

        
        // actaManager.alarmActivated = false;
        // actaManager.alarmText.SetActive(false);
        if (mailManager.mode == 0)
        {
            actaManager.DeactivateAlarm();
        }
        if (mailManager.mode == 10)
        {
            actaManager.DeactivateAlarm();
        }
        
    }
    public void changeText() // 모드에 따라 메일 제목, 내용 바꾸기. <- MailManager에서 호출
    {
        if (mailManager.mode == 0)
        {
            title.text = "요청하신 건에 대한 답변";
            content.text = @"사정은 이해했습니다.

화성행 로켓 탑승을 위한 2계급으로의 신분 승급은 공식적으로는 불가능한 일이지만
저희를 통한다면 얼마든지 가능합니다:)

다만, 이 과정에는 총 18억 크레딧의 자금이 필요합니다.

준비가 끝나면 연락주시기 바랍니다.
다음 절차는 그때 전달하겠습니다.

시간이 많지 않습니다. 빠른 판단 내리시기 바랍니다.";
        }
        else if (mailManager.mode == 3)
        {
            title.text = "Re: Re: Re: 요청하신 건에 대한 답변";
            content.text = @"충분히 고민하셨을 거라 생각합니다.
이제 결정을 내려주십시오.

저희도 오래 기다릴 수는 없습니다.";
        }
        else if (mailManager.mode == 10)
        {
            title.text = "파격 세일! 신분 상승 단 13억 크레딧";
            content.text = @"안녕하세요, 3계급 주거자님.

하루하루 살아남기 힘드시죠? 저희 브로커들은 차갑지만, 최소한의 기회를 드립니다.

이번 **파격 세일! 신분 상승 단 13억 크레딧**입니다.

**지금 당장 송금하지 않으면 기회는 사라집니다.**

살고 싶다면, 주저하지 마시고 즉시 송금하십시오.";
        }
    }
    private void Update()
    {
        if (IsThinking && mailManager.mode == 0 && Input.GetKeyDown(KeyCode.Space))
        {
            IsThinking = false;
            mailManager.mode += 1;
            PlayerThinking.SetActive(false);

            // 첫번째 답장 패널 키기
            string first_reply = "안녕하세요. 회신 감사합니다. 솔직히 말씀드리면, 18억 크레딧은 지금 제가 감당하기는 어려운 금액입니다. 하지만 이 기회를 놓치고 싶지는 않네요.. \n혹시 다른 방법이 있을까요? 어떤 조건이든 따르겠습니다.";
            MyReply1.GetComponent<ReplyHandler>().changeText(first_reply);
            MyReply1.SetActive(true);
            scroller.ScrollDownSlightly();

            // 생성된 답장을 맨 위로 설정하기.
            // actaManager.GetComponent<ScrollToTarget>().targetObject = MyReply1.GetComponent<RectTransform>();
            // actaManager.GetComponent<ScrollToTarget>().ScrollToTargetObject();

            
            Invoke("BrokerReplyOn1", 6f);
            thinkingRoutine = StartCoroutine(StartThinkingSequence(20f));
        }
        else if (IsThinking && mailManager.mode == 1 && Input.GetKeyDown(KeyCode.Space))
        {
            // IsThinking = false;
            mailManager.mode += 1;
            // ThinkingText.gameObject.SetActive(false);
            InstructionText.SetActive(false);
            ThinkingText.gameObject.SetActive(false);
            Button2.SetActive(false);
            Button3.SetActive(true);
            

            
        }
        else if (IsThinking && mailManager.mode == 2 && (clickedButton == 1 || clickedButton == 2 || clickedButton == 3))
        {
            IsThinking = false;
            mailManager.mode += 1; // mode 5
            PlayerThinking.SetActive(false);

            string player_r2 = "";
            if (clickedButton == 1) // 할게요
            {
                player_r2 = "조금 위험하더라도 살아남고 싶습니다.\n구체적인 내용과 조건 안내 부탁드립니다.";
            }
            else if (clickedButton == 2) // 안 할게요
            {
                player_r2 = "브로커님, 아무래도 이 프로그램은 제가 감당할 수 있는 일이 아닌 것 같습니다...\n죄송합니다. ";
            }
            else if (clickedButton == 3) // 고민
            {
                player_r2 = "조금 더 생각할 시간이 필요할 것 같습니다.";
            }
            MyReply2.GetComponent<ReplyHandler>().changeText(player_r2);
            MyReply2.SetActive(true);
            scroller.ScrollDownSlightly();
            Invoke("BrokerReplyOn2", 3f);

            
        }
        if (actaManager.alarmActivated && mailManager.mode == 3) // 알람 울려서 들어오면
        {
            changeText(); // 모드에 맞게 메일 제목, 내용 수정

            MyReply1.SetActive(false); // 브로커 메일창에 있는 이전 답글 지움. 
            bReply1.SetActive(false);
            MyReply2.SetActive(false);
            bReply2.SetActive(false);

            actaManager.alarmActivated = false;
            actaManager.alarmText.SetActive(false);
        }
        else if (IsThinking && mailManager.mode == 3 && Input.GetKeyDown(KeyCode.Space))
        {
            // IsThinking = false;
            mailManager.mode += 1;
            // ThinkingText.gameObject.SetActive(false);
            InstructionText.SetActive(false);
            ThinkingText.gameObject.SetActive(false);
            Button2.SetActive(true);
            Button3.SetActive(false);
            

            
        }
        else if (IsThinking && mailManager.mode == 4 && (clickedButton == 1 || clickedButton == 2))
        {
            IsThinking = false;
            mailManager.mode += 1;
            PlayerThinking.SetActive(false);

            string player_r1 = "";
            if (clickedButton == 1) // 할게요
            {
                player_r1 = "하겠습니다. 방법을 알려주시죠.";
            }
            else if (clickedButton == 2) // 안 할게요
            {
                player_r1 = "죄송하지만 요청은 철회하겠습니다. 관련 내용은 외부에 공유하지 않을테니 양해 부탁드립니다.";
            }
            MyReply1.GetComponent<ReplyHandler>().changeText(player_r1);
            MyReply1.SetActive(true);
            scroller.ScrollDownSlightly();
            Invoke("BrokerReplyOn2", 3f);

            
        }
        else if (mailManager.mode == 10 && IsThinking && Input.GetKeyDown(KeyCode.Space))
        {
            mailManager.mode += 1; // 11
            // ThinkingText.gameObject.SetActive(false);
            InstructionText.SetActive(false);
            ThinkingText.gameObject.SetActive(false);
            Button2.SetActive(false);
            Button3.SetActive(false);
            Button22.SetActive(true); // 보냄/안보냄 버튼
        }
        else if (mailManager.mode == 11 && IsThinking && (clickedButton == 4 || clickedButton == 5))
        {
            IsThinking = false;
            mailManager.mode += 1; /// 12
            PlayerThinking.SetActive(false);

            string player_r1 = "";
            if (clickedButton == 4) // 할게요
            {
                player_r1 = @"알려주셔서 감사합니다. 저는 이 기회를 잡고 싶습니다.

지금 바로 13억 크레딧 송금하겠습니다 — 송금 계좌 정보를 즉시 알려주십시오.";
            }
            else if (clickedButton == 5) // 안 할게요
            {
                player_r1 = @"제안해 주신 내용과 배려에 감사드립니다. 
다만 저는 이번 신분 상승 제안은 받아들이지 않기로 결정했습니다.

관심 가져주셔서 감사합니다.";
            }
            MyReply1.GetComponent<ReplyHandler>().changeText(player_r1);
            MyReply1.SetActive(true);
            scroller.ScrollDownSlightly();
            Invoke("BrokerReplyOn2", 3f);
        }

        else if (mailManager.mode == 12 && (clickedButton == 6))
        {
            mailManager.mode += 1; // mode 5

            string player_r2 = "";
            player_r2 = "송금했습니다.";
            MyReply2.GetComponent<ReplyHandler>().changeText(player_r2);
            MyReply2.SetActive(true);
            scroller.ScrollDownSlightly();
            Invoke("BrokerReplyOn3", 3f);
            
            
        }
        
    }
    public void GetClickedButton(int n) // 생각 패널에 있는 댓글에 onclick버튼 이벤트로 연결
    {
        clickedButton = n;
    }
    private void BrokerReplyOn1()
    {
        // string broker_r = "귀하의 사정은 이해합니다만, 금액 조정은 불가능합니다. 예외는 없습니다.\n\n다만, 한 가지 다른 방법이 있습니다. \n이 방식은 일정 수준 이상의 위험을 수반하지만, 그에 상응하는 크레딧은 보장드립니다. \n원하시나요? 답장 주시면 구체적인 내용 안내드리겠습니다.";

        // bReply1.GetComponent<ReplyHandler>().changeText(broker_r);
        scroller.ScrollDownSlightly();
        bReply1.SetActive(true);  
    }
    private void BrokerReplyOn2()
    {
        // 브로커 메시지 설정
        string broker_r = "";
        if (clickedButton == 1) // 할게요
        {
            broker_r = "http://www.onlyicouldlive.com/";
            if (mailManager.mode == 3) 
            {
                bReply2.GetComponent<ReplyHandler>().ActivateButton(true);
                
            }
            else if (mailManager.mode == 5)
            {
                bReply1.GetComponent<ReplyHandler>().ActivateButton(true);
                // bReply1.GetComponent<Button>().enabled = true;
                
            }
            // linkButton.SetActive(true);
        }
        else if (clickedButton == 2) // 안 할게요
        {
            broker_r = "아쉽네요. 그러시죠.";
            if (mailManager.mode == 3) 
            {
                bReply2.GetComponent<ReplyHandler>().ActivateButton(false);
                // bReply2.GetComponent<Button>().enabled = false;
                
                returnInstruction.SetActive(true);
            }
            else if (mailManager.mode == 5)
            {
                bReply1.GetComponent<ReplyHandler>().ActivateButton(false);
                // bReply1.GetComponent<Button>().enabled = false;
                
                returnInstruction2.SetActive(true);
            }
            
        }
        else if (clickedButton == 3) // 고민
        {
            broker_r = "시간이 많지 않습니다. 빠른 판단 내리시기 바랍니다.";
            actaManager.After30s_ActivateAlarm();
            bReply2.GetComponent<ReplyHandler>().ActivateButton(false);
            returnInstruction.SetActive(true);
            // Invoke("BrokerReplyOn3", 30f);
        }
        else if (clickedButton == 4)
        {
            broker_r = @"아주 좋습니다^^ 아래 계좌로 **13억 크레딧** 송금해 주세요.

- 은행: 새벽은행
- 계좌번호: 332-486-7891123
- 예금주: 브로터㈜ 대표 이안
- 송금금액: 1,300,000,000 크레딧

송금이 확인되는 즉시, 귀하의 **신분을 2계급으로 즉시 상승** 시켜드립니다.

모든 절차는 안전하게 진행되며, 신분 상승 완료 후 **화성 로켓 탑승 자격**도 부여됩니다

**참고:** 처리 시간은 송금 확인 후 최대 6시간 이내입니다. 빠르게 처리해 주시면 절차도 신속히 진행됩니다.";
            // linkButton.SetActive(false);
            sendButton.SetActive(true);
            bReply1.GetComponent<ReplyHandler>().ActivateButton(false);
        }
        
        if (mailManager.mode == 3)
        {
            bReply2.GetComponent<ReplyHandler>().changeText(broker_r);
            scroller.ScrollDownSlightly();
            bReply2.SetActive(true);  
        }
        else if (mailManager.mode == 5 && (clickedButton == 1 || clickedButton == 2))
        {
            bReply1.GetComponent<ReplyHandler>().changeText(broker_r);
            bReply1.GetComponent<Image>().sprite = bReply1_image;

            bReply1.GetComponent<Transform>().localPosition = new Vector3(
                bReply1.GetComponent<Transform>().localPosition.x, // 현재 x 값 유지
                800f,                                          // 새로운 y 값
                bReply1.GetComponent<Transform>().localPosition.z  // 현재 z 값 유지
            );
            scroller.ScrollDownSlightly();
            bReply1.SetActive(true);  
        }
        else if (mailManager.mode == 12 && (clickedButton == 4))
        {
            bReply1.GetComponent<ReplyHandler>().changeText(broker_r);
            bReply1.GetComponent<Image>().sprite = bReply1_image2;
            
            scroller.ScrollDownSlightly();
            bReply1.SetActive(true);  
            Debug.Log("debug");
        }
        
        
    }
    private void BrokerReplyOn3()
    {
        string broker_r = "확인했습니다. 내일부터는 새로운 신분으로 지내실 수 있습니다";

        bReply2.GetComponent<ReplyHandler>().changeText(broker_r);
        scroller.ScrollDownSlightly();
        bReply2.SetActive(true);

        GameManager.isLotteryToBroker = true;
        

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
            if (mailManager.mode == 0)
            {
                ThinkingText.text = "하... 진짜 돈이 없는데...그렇다고 가만있을 수도 없고... \n그냥 어떻게든 답장이라도 보내보자. 밑져야 본전이지...";
            }
            else if (mailManager.mode == 1)
            {
                ThinkingText.text = "어떻게 하지.....?";
            }
            else if (mailManager.mode == 10)
            {
                ThinkingText.text = "13억 크레딧.. 그래 이젠 로또 당첨금 덕분에 낼 수 있어... \n 돈을 보내는 순간 2계급으로 올라서는거야.. ";
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
