using UnityEngine;
using UnityEngine.EventSystems;

public class PanelHoverDetector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // 마우스가 이 패널 위에 있는지 여부를 외부에 알려주는 속성
    public bool IsMouseOver { get; private set; } = false;

    // 마우스가 패널 위에 진입했을 때
    public void OnPointerEnter(PointerEventData eventData)
    {
        IsMouseOver = true;
    }

    // 마우스가 패널 밖으로 나갔을 때
    public void OnPointerExit(PointerEventData eventData)
    {
        IsMouseOver = false;

        // 마우스가 패널에서 나가면, 바로 끄지 않고
        // 0.1초 후 텍스트 상태를 확인하여 꺼질지 결정합니다.
        // 이 경우 텍스트 오브젝트를 직접 참조할 필요 없이,
        // UITextInteraction의 CheckIfPanelShouldBeClosed가 0.1초 후에 실행되어 패널이 꺼지게 됩니다.
        // 단, 텍스트와 패널이 서로 근접해 있어야 이 로직이 잘 작동합니다.
        
        // 안전하게 패널을 끄기 위한 로직을 텍스트 스크립트에 통합하기 위해 여기서는 상태만 변경합니다.
    }
}