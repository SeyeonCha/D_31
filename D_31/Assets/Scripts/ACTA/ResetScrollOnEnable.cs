using UnityEngine;
using UnityEngine.UI;

public class ResetScrollOnEnable : MonoBehaviour
{
    private ScrollRect parentScrollRect; // 상위 ScrollRect를 캐시할 변수

    void Awake()
    {
        // 1. 이 객체 자신부터 부모(조상) 방향으로 올라가면서 ScrollRect 컴포넌트를 찾아 캐시합니다.
        parentScrollRect = GetComponentInParent<ScrollRect>();

        if (parentScrollRect == null)
        {
            // 경고: 스크립트가 붙은 GameObject의 상위(부모)에 ScrollRect가 없습니다.
            Debug.LogError($"'{gameObject.name}'의 상위 객체들에서 ScrollRect를 찾지 못했습니다. 스크립트 위치를 확인하세요!");
        }
    }

    // 이 자식 패널이 활성화될 때마다 호출됩니다.
    void OnEnable()
    {
        if (parentScrollRect != null)
        {
            // 2. 캐시된 상위 ScrollRect의 위치를 맨 위(1f)로 설정하여 초기화합니다.
            parentScrollRect.verticalNormalizedPosition = 1f;

            // Debug.Log($"'{gameObject.name}' 패널 활성화 시 상위 스크롤을 초기화했습니다.");
        }
    }
}