using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // ScrollRect를 사용하기 위해 필요

public class ContentScroller : MonoBehaviour
{
    // 인스펙터에서 ScrollRect 컴포넌트를 연결
    public ScrollRect targetScrollRect; 

    // 인스펙터에서 원하는 이동량 (픽셀 단위)을 설정 (예: 100f)
    public float movementAmount = 100f; 

    // Content RectTransform을 가져오는 함수
    private RectTransform GetContentRect()
    {
        if (targetScrollRect == null) return null;
        return targetScrollRect.content;
    }

    public void ScrollDownSlightly()
{
    RectTransform contentRect = GetContentRect();
    if (contentRect == null) return;

    // 현재 Y 위치에 movementAmount만큼 더함 (더 양수 값이 됨)
    float newY = contentRect.anchoredPosition.y + movementAmount;

    // 스크롤 가능한 최대 범위 계산
    float contentHeight = contentRect.rect.height;
    float viewportHeight = targetScrollRect.viewport.rect.height;
    float maxScrollY = Mathf.Max(0f, contentHeight - viewportHeight);
    
    // 스크롤 범위를 벗어나지 않도록 클램프 적용 (최대값: maxScrollY)
    float clampedY = Mathf.Min(maxScrollY, newY); 

    contentRect.anchoredPosition = new Vector2(
        contentRect.anchoredPosition.x,
        clampedY
    );
}
}
