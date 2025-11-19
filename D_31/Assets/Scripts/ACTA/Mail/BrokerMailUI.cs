using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

// 브로커 메일 패널에 붙음. 
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
    private Sprite bReply1_image; // 클릭 전 스크랩버튼 이미지
    public ContentScroller scroller;

    public GameObject linkButton ;
    

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
        thinkingRoutine = StartCoroutine(StartThinkingSequence(7f));

        
        // actaManager.alarmActivated = false;
        // actaManager.alarmText.SetActive(false);
        if (mailManager.mode == 0)
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

화성행 로켓 탑승을 위한 신분 승급은 공식적으로는 불가능한 일이지만
그 외의 방법이 전혀 없는 것은 아닙니다.

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

            
            Invoke("BrokerReplyOn1", 3f);
            thinkingRoutine = StartCoroutine(StartThinkingSequence(8f));
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
            mailManager.mode += 1;
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
        string broker_r = "";
        if (clickedButton == 1) // 할게요
        {
            broker_r = "http://www.onlyicouldlive.com/";
            linkButton.SetActive(true);
        }
        // else if (clickedButton == 2) // 안 할게요
        // {
        //     broker_r = "아쉽네요. 그러시죠.";
        // }
        else if (clickedButton == 3) // 고민
        {
            broker_r = "시간이 많지 않습니다. 빠른 판단 내리시기 바랍니다.";
            actaManager.After30s_ActivateAlarm();
            // Invoke("BrokerReplyOn3", 30f);
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
        
    }
    // private void BrokerReplyOn3()
    // {
    //     actaManager.MailAlarm();  // 알림 켜기

        

    // }
    

    
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
            ThinkingText.gameObject.SetActive(true);
            InstructionText.SetActive(true);
            Button3.SetActive(true);
            Button3.SetActive(false);
            Debug.Log($"[타이머 완료] {delay}초 후 PlayerThinking 패널 활성화.");

            IsThinking = true;
        }
    }
    

    
}
