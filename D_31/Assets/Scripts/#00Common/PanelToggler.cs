using UnityEngine;
using UnityEngine.UI; // Button과 Image 컴포넌트를 사용하기 위해 추가

public class PanelToggler : MonoBehaviour
{
    // Unity Inspector에서 활성화/비활성화할 패널 오브젝트를 드래그하여 연결합니다.
    public GameObject targetPanel;

    // 이 버튼의 Image 컴포넌트를 변경할 것이므로, 자기 자신의 Button 컴포넌트를 참조합니다.
    private Button thisButton;

    // 버튼이 켜졌을 때(패널 활성화 시) 보여줄 이미지
    public Sprite onSprite;
    // 버튼이 꺼졌을 때(패널 비활성화 시) 보여줄 이미지
    public Sprite offSprite;

    void Awake()
    {
        // 스크립트가 붙어있는 GameObject에서 Button 컴포넌트를 가져옵니다.
        thisButton = GetComponent<Button>();
        if (thisButton == null)
        {
            Debug.LogError("PanelToggler: 이 스크립트는 Button 컴포넌트에 붙어있어야 합니다!", this);
            enabled = false; // 스크립트 비활성화
            return;
        }

        // 초기 상태에 따라 버튼 이미지를 설정합니다.
        // 예를 들어, targetPanel이 처음부터 활성화되어 있다면 onSprite를 보여줍니다.
        if (targetPanel != null)
        {
            SetButtonImage(targetPanel.activeSelf);
        }
        else
        {
            Debug.LogError("PanelToggler: targetPanel이 할당되지 않았습니다!");
        }
    }

    // 이 함수는 버튼의 OnClick 이벤트에 연결됩니다.
    public void TogglePanel()
    {
        if (targetPanel != null)
        {
            bool isActive = targetPanel.activeSelf;
            targetPanel.SetActive(!isActive); // 패널의 활성화 상태를 반전시킵니다.

            // 패널의 새로운 활성화 상태에 따라 버튼 이미지를 업데이트합니다.
            SetButtonImage(!isActive); // !isActive는 패널의 "새로운" 상태를 의미합니다.
        }
        else
        {
            Debug.LogError("PanelToggler: targetPanel이 할당되지 않았습니다!");
        }
    }

    // 버튼 이미지를 설정하는 헬퍼 함수
    private void SetButtonImage(bool isOn)
    {
        if (thisButton != null && thisButton.image != null)
        {
            if (isOn && onSprite != null)
            {
                thisButton.image.sprite = onSprite;
            }
            else if (!isOn && offSprite != null)
            {
                thisButton.image.sprite = offSprite;
            }
            // else {
            //     Debug.LogWarning("PanelToggler: onSprite 또는 offSprite가 할당되지 않았습니다.");
            // }
        }
    }
}