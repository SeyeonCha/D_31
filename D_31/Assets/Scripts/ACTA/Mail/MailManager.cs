using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MailManager : MonoBehaviour
{
    public Transform content;
    public GameObject itemPrefab;

    public GameObject contentText;
    public TypingEffect contentTypingEffect;


    public void Awake()
    {
        AddMailToTop("답장1", "2049-11-11");
        AddMailToTop("답장2", "2049-11-11");
    }

    private void Update()
    {
        if (contentTypingEffect.IsTyping) // 타이핑 중이면
        {
            if (Input.GetKeyDown(KeyCode.Space)) // 키가 눌렸을 때
            {
                contentTypingEffect.SkipTyping(contentTypingEffect.GetComponent<TMP_Text>().text);
                // 현재 텍스트 타이핑을 스킵
            }
        }
    }
    
    public void AddMailToTop(string title, string time) // 메일 리스트에 메일 추가하는 함수
    {
        GameObject newItem = Instantiate(itemPrefab, content);

        MailUI mailUI = newItem.GetComponent<MailUI>();
        mailUI.SetupUI(title, time);
        newItem.transform.SetAsFirstSibling();
    }

    public void StartMailTyping(GameObject content_text) // 타이핑 적용할 텍스트 게임오브젝트 입력 -> 메일 타이핑 시작
    {
        contentTypingEffect = content_text.GetComponent<TypingEffect>();
        contentTypingEffect.StartTyping(contentTypingEffect.GetComponent<TMP_Text>().text);

    }

}
