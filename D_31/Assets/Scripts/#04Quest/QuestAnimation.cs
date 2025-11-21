using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using DG.Tweening;

public class QuestAnimation : MonoBehaviour
{
    public CanvasGroup questPanelCanvasGroup;
    
    public RectTransform titleTextRect; // 타이틀 텍스트의 RectTransform 컴포넌트
    public TextMeshProUGUI titleTMP; // 타이틀 텍스트
    public RectTransform contentTextRect; // 내용 텍스트의 RectTransform
    
    public TextMeshProUGUI contentTMP; // 콘텐츠 텍스트
    public RectTransform collapseTextRect; // 내용 텍스트의 RectTransform
    public TextMeshProUGUI collapseTMP; // 축소 안내 텍스트

    public Vector2 targetTitlePosition = new Vector2(-550,270); // 패널 왼쪽 상단
    public float targetTitleScale = 0.5f; // 작아질 크기

    public float fadeInDuration = 0.5f;
    public float duration = 0.5f; // 애니메이션 시간

    public RectTransform questPanelRect; // 패널 자체
    
    public float collapseDuration = 0.3f; // 축소 애니메이션 시간
    public Vector2 targetCollapsePosition = new Vector2(550, 200); // 오른쪽 상단 목표 위치 (예시)
    public float targetCollapseScale = 0.3f;

    private bool canCollapse = false; // 애니메이션 완료 상태.


    // 퀘스트 데이별로 넘기기 위한 코드
    // 인스펙터 창에서 퀘스트 내용 수정 가능
    [Header("Quest Text")]
    [TextArea(3, 5)]
    public string[] ListcontentTMP;


    void Start()
    {
        // 씬 시작시 퀘스트 박스 전체 숨김 
        // if (questPanelCanvasGroup != null)
        // {
        //     questPanelCanvasGroup.alpha = 0f;
        // }

        // 게임 시작 시 첫 번째 대화 표시
        if (ListcontentTMP.Length > 0)
        {
            contentTMP.text = ListcontentTMP[GameManager.DayEnded];
            // contentTMP.text = ListcontentTMP[0]; // 👈 임시 코드
        }
                
        contentTMP.alpha = 0f; // 시작할 때 내용 숨기기
        collapseTMP.alpha = 0f; // 시작할 때 내용 숨기기
        titleTextRect.localScale = Vector3.one; // 제목 크기 초기화
        Invoke("StartQuestAnimation", 0.7f);
    }

    
    void Update()
    {
        if (canCollapse && Input.GetKeyDown(KeyCode.Space))
        {
            StartCollapseAnimation();
        }
    }

    public void StartQuestAnimation()
    {
        // 텍스트 내용 설정
        // titleTMP.text = questTitle;
        // contentTMP.text = questContent;

        Sequence fullSequence = DOTween.Sequence();


        // 타이틀 텍스트 시작 위치와 크기
        titleTextRect.anchoredPosition = Vector2.zero;
        titleTextRect.localScale = Vector3.one;

        // 퀘스트 박스 등장 애니메이션
        fullSequence.Append(
            questPanelCanvasGroup.DOFade(1f, fadeInDuration)
            .SetEase(Ease.OutQuad)
        );
        // 퀘스트 박스 등장 후 2초 대기
        fullSequence.AppendInterval(0.7f);

        // 타이틀 텍스트 이동 및 크기 변경 애니메이션. 
        fullSequence.Append(
            titleTextRect.DOAnchorPos(targetTitlePosition, duration)
            .SetEase(Ease.OutQuad) // targetPosition으로 duration동안 이동하는 애니메이션 추가
        );
        fullSequence.Join(
            titleTextRect.DOScale(targetTitleScale * Vector3.one, duration)
            .SetEase(Ease.OutQuad) // targetScale로 duration동안 크기변화하는 애니매이션을 이동 애니메이션과 동시에 재생
        );

        // 2. 퀘스트 내용 텍스트 등장 애니메이션
        fullSequence.AppendCallback(() => // 트윈 완료 직후 중괄호 코드 실행. 
        {
            contentTextRect.localScale = Vector3.one; // 콘텐츠 텍스트 크기는 원래대로
            // 페이드 인
            contentTMP.DOFade(1f, duration * 0.5f) // targetAlpha값으로 duration*0.5f만큼 알파값 변화 애니메이션. 
                .SetEase(Ease.OutQuad);
            // 축소 텍스트 등장. 
            collapseTextRect.localScale = Vector3.one;
            collapseTMP.DOFade(1f, duration * 0.5f)
                .SetEase(Ease.OutQuad);
        });

        // 이 다음에 스페이스바를 누르면 퀘스트 패널이 함께 축소하여 화면의 오른 쪽 상단으로 이동하도록 하고 싶어



        // 3. 퀘스트 등장 애니메이션 시작
        fullSequence.Play();

        fullSequence.OnComplete(() =>
        {
            canCollapse = true;

        });
    }
    
    public void StartCollapseAnimation()
    {
        canCollapse = false;
        collapseTMP.alpha = 0f;

        Sequence collapseSequence = DOTween.Sequence();

        // 패널 사이즈 축소
        collapseSequence.Append(
            questPanelRect.DOScale(0.6f*Vector3.one, 0.5f)
            .SetEase(Ease.OutQuad)
        );
        // // 타이틀 크기 축소
        // collapseSequence.Join(
        //     titleTextRect.DOScale(Vector3.one, 1f)
        //     .SetEase(Ease.OutQuad)
        // );
        // 내용 크기 축소
        collapseSequence.Join(
            contentTextRect.DOScale(1.1f * Vector3.one, 0.5f)
            .SetEase(Ease.OutQuad)
        );
        // // 축소텍스트 크기 축소
        // collapseSequence.Join(
        //     collapseTextRect.DOScale(0.8f * Vector3.one, 1f)
        //     .SetEase(Ease.OutQuad)
        // );

        // 동시에 패널 전체 오른쪽 상단으로 이동
        collapseSequence.Append(
            questPanelRect.DOAnchorPos(targetCollapsePosition, collapseDuration)
            .SetEase(Ease.OutQuad)
        );
        
    }
}
