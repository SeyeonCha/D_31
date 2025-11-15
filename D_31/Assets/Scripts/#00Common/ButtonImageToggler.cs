using UnityEngine;
using UnityEngine.UI;

public class ButtonImageToggler : MonoBehaviour
{
    private Button thisButton;

    public Sprite onSprite;
    public Sprite offSprite;

    // 현재 버튼의 상태를 추적하는 변수 (true: ON, false: OFF)
    private bool isOn = false;

    void Awake()
    {
        thisButton = GetComponent<Button>();
        if (thisButton == null)
        {
            Debug.LogError("ButtonImageToggler: 이 스크립트는 Button 컴포넌트에 붙어있어야 합니다!", this);
            enabled = false;
            return;
        }

        // 초기 상태를 offSprite로 설정합니다. (원하는 경우 onSprite로 시작할 수도 있습니다)
        SetButtonImage(isOn); // isOn이 false이므로 offSprite로 시작
    }

    // 이 함수는 버튼의 OnClick 이벤트에 연결됩니다.
    public void ToggleButtonImage()
    {
        // 현재 상태를 반전시킵니다. (false -> true, true -> false)
        isOn = !isOn;

        // 새로운 상태에 따라 버튼 이미지를 업데이트합니다.
        SetButtonImage(isOn);
    }

    // 버튼 이미지를 설정하는 헬퍼 함수
    private void SetButtonImage(bool state)
    {
        if (thisButton != null && thisButton.image != null)
        {
            if (state && onSprite != null) // 상태가 ON이고 onSprite가 있으면
            {
                thisButton.image.sprite = onSprite;
            }
            else if (!state && offSprite != null) // 상태가 OFF이고 offSprite가 있으면
            {
                thisButton.image.sprite = offSprite;
            }
            // else {
            //     Debug.LogWarning("ButtonImageToggler: onSprite 또는 offSprite가 할당되지 않았습니다.");
            // }
        }
    }
}