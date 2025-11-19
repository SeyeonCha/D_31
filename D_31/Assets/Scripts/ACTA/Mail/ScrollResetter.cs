using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // ScrollRect 사용을 위해 필요

public class ScrollResetter : MonoBehaviour
{
    // 인스펙터에 Scroll View 오브젝트에 붙어있는 ScrollRect 컴포넌트를 연결합니다.
    public ScrollRect scrollView; 

    private void OnEnable()
    {
        // 널 체크 (NullReferenceException 방지)
        if (scrollView == null)
        {
            Debug.LogError("ScrollRect 컴포넌트가 연결되지 않았습니다!");
            return;
        }

        // 스크롤 위치를 맨 위로 설정하는 함수 호출
        ResetScrollPosition();
    }

    private void ResetScrollPosition()
    {
        // ===============================================================
        // 💡 핵심: Normalized Position을 1.0f로 설정
        // ===============================================================
        
        // 1. 세로 스크롤 (Vertical Scroll) 위치를 맨 위로 설정
        // (0.0f = 맨 아래, 1.0f = 맨 위)
        scrollView.verticalNormalizedPosition = 1.0f;
        
        // 2. 가로 스크롤 (Horizontal Scroll) 위치를 맨 왼쪽으로 설정 (선택 사항)
        // (0.0f = 맨 왼쪽, 1.0f = 맨 오른쪽)
        scrollView.horizontalNormalizedPosition = 0.0f; 
        
        Debug.Log("스크롤 위치가 맨 위(1.0f)로 초기화되었습니다.");
    }
}
