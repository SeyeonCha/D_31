using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;

public class UITextInteractionPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [System.Serializable]
    private class OnClickEvent : UnityEvent { }
    [SerializeField]
    private OnClickEvent onClickEvent;

    public GameObject targetPanel;
    // 🚨 수정됨: CafeTitle 참조 대신 CafeManager 참조를 사용합니다.
    [Tooltip("현재 로드된 카페 데이터를 관리하는 CafeManager")]
    public CafeManager cafeManager;
    private TextMeshProUGUI text;

    // 패널 제어를 위한 변수와 함수 추가
    private PanelHoverDetector panelDetector; // 타겟 패널에 붙일 스크립트 참조

    // 이 기능을 활성화할 특정 DayEnded 값과 uniqueId
    private const int REQUIRED_DAY = 1;
    private const int REQUIRED_ID = 6;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        
        if (targetPanel != null)
        {
            // 타겟 패널에서 PanelHoverDetector 스크립트를 찾아서 참조를 가져옵니다.
            panelDetector = targetPanel.GetComponent<PanelHoverDetector>();
            if (panelDetector == null)
            {
                Debug.LogError("타겟 패널에 PanelHoverDetector 스크립트가 없습니다. 패널에 이 스크립트를 추가해주세요.", targetPanel);
            }
            //targetPanel.SetActive(false);
        }

        // 🚨 수정됨: CafeManager를 부모에서 찾아 할당합니다.
        if (cafeManager == null)
        {
            cafeManager = GetComponentInParent<CafeManager>();
        }
    }

    // 현재 게임 상태가 시나리오 활성화 조건에 충족하는지 확인합니다.
    private bool CheckActivationCondition()
    {
        // 1. CafeManager 컴포넌트가 유효한지 확인
        if (cafeManager == null)
        {
            // Debug.LogWarning("[UITextInteraction] CafeManager reference is null.");
            return false;
        }

        // 2. uniqueId 값을 CafeManager에서 가져와 비교합니다.
        int currentUniqueId = cafeManager.CurrentUniqueId; // 🚨 CafeManager에서 최신 ID를 가져옵니다.

        bool conditionMet = 
            GameManager.DayEnded == REQUIRED_DAY && 
            currentUniqueId == REQUIRED_ID; // 👈 uniqueId 값 비교
            
        //Debug.Log($"[UITextInteraction] Checking condition: Day={GameManager.DayEnded}, CurrentID={currentUniqueId}. TargetID={REQUIRED_ID}. Met: {conditionMet}");

        return conditionMet;
    }

    // 마우스 진입 (텍스트 위)
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 🚨 조건 충족 시에만 패널 활성화 기능을 수행합니다.
        if (CheckActivationCondition())
        {
            text.fontStyle = FontStyles.Bold;
            if (targetPanel != null)
            {
                // 타겟 패널을 활성화합니다. (ChatManager의 OnEnable이 호출되어 시나리오 시작)
                targetPanel.SetActive(true);
                Debug.Log("[UITextInteraction] Condition met. Activating target panel.");
            }
        }
    }

    // 마우스 이탈 (텍스트 밖)
    public void OnPointerExit(PointerEventData eventData)
    {
        text.fontStyle = FontStyles.Normal;
        
        // 텍스트에서 마우스가 나가면, 바로 끄지 않고
        // 0.1초 후 패널 상태를 확인하여 꺼질지 결정합니다. (지연을 줘서 패널로 이동할 시간을 줍니다)
        Invoke("CheckIfPanelShouldBeClosed", 0.1f);
    }

    // 패널이 꺼져야 할지 확인하는 함수
    private void CheckIfPanelShouldBeClosed()
    {
        // 텍스트에도 마우스가 없고 (OnPointerExit이 호출되었으니 당연), 
        // 패널에도 마우스가 없다면 (panelDetector.isMouseOver가 false라면) 패널을 끕니다.
        if (targetPanel != null && panelDetector != null && !panelDetector.IsMouseOver)
        {
            targetPanel.SetActive(false);
        }
    }

    // UI 요소가 클릭되었을 때 호출
    public void OnPointerClick(PointerEventData eventData)
    {
        // 🚨 조건 충족 시에만 클릭 이벤트를 실행합니다.
        if (CheckActivationCondition())
        {
            // Inspector에 등록된 UnityEvent 실행
            onClickEvent?.Invoke();
            Debug.Log("[UITextInteraction] Condition met. Executing Click Event.");
        }
        else
        {
            Debug.Log("[UITextInteraction] Condition not met. Click event blocked.");
        }
    }
}