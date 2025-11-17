using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReelsCommentHandler : MonoBehaviour
{
    // 데이터 받아서 프리팹 UI에 저장하는 스크립트
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI contentText;
    [SerializeField] private TextMeshProUGUI likeText;

    public ReelsCommentData CurrentData { get; private set; }

    public void SetupUI(ReelsCommentData data)
    {
        CurrentData = data;

        // 텍스트 업데이트
        nameText.text = data.name;
        contentText.text = data.content;
        likeText.text = data.like;


    }
}
