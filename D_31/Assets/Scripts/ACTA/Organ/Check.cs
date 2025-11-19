using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Check : MonoBehaviour
{
    private Image buttonImage; // 버튼의 Image 컴포넌트

    private void Awake()
    {
        buttonImage = GetComponent<Image>();
    }

    public void changeImage()
    {
        
        Color currentColor = buttonImage.color;
        currentColor.a = 1.0f;
        buttonImage.color = currentColor;

    }
}
