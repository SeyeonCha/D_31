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
    private TextMeshProUGUI text;

    // 패널 제어를 위한 변수와 함수 추가
    private PanelHoverDetector panelDetector; // 타겟 패널에 붙일 스크립트 참조

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
            targetPanel.SetActive(false);
        }
    }

    // 마우스 진입 (텍스트 위)
    public void OnPointerEnter(PointerEventData eventData)
    {
        text.fontStyle = FontStyles.Bold;
        if (targetPanel != null)
        {
            targetPanel.SetActive(true);
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
        // Inspector에 등록된 UnityEvent 실행
        onClickEvent?.Invoke();
    }
}