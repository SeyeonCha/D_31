using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// 이것 comment 프리팹이랑 reply 프리팹에 붙이기
// ComuManager에서 부모 UI 가져오기 & 프리팹 생성하기 & 
// 여기에다가 sourceTitle.data.comments[i]로부터 데이터 받아오기
public class CommentHandler : MonoBehaviour
{
    // 데이터 받아서 프리팹 UI에 저장하는 스크립트
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI contentText;

    public CommentData CurrentData { get; private set; }

    public void SetupUI(CommentData data)
    {
        CurrentData = data;

        // 텍스트 업데이트
        nameText.text = data.name;
        contentText.text = data.content;



    }

}
