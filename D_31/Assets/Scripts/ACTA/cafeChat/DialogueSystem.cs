using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    [Header("UI Components")]
    [Tooltip("대화 내용이 출력될 TextMeshProUGUI 컴포넌트")]
    public TextMeshProUGUI dialogueText;
    
    [Header("Settings")]
    [Tooltip("타이핑 효과 속도 (문자당 초)")]
    public float typingSpeed = 0.05f;

    private string[] sentences;
    private int sentenceIndex;
    private bool isTyping = false;
    private bool dialogueCompleted = false;

    // 대화 종료 시점을 알리기 위해 코루틴을 저장합니다.
    private Coroutine typingCoroutine;

    /// <summary>
    /// 외부(CafeManager)에서 호출하여 대화를 시작하는 함수입니다.
    /// </summary>
    /// <param name="newSentences">표시할 문장 배열</param>
    /// <returns>대화가 완료될 때까지 기다릴 코루틴</returns>
    public IEnumerator StartDialogue(string[] newSentences)
    {
        if (dialogueText == null)
        {
            Debug.LogError("[DialogueSystem] Dialogue Text UI component is not assigned.");
            yield break;
        }

        sentences = newSentences;
        sentenceIndex = 0;
        dialogueCompleted = false;
        
        // 첫 번째 문장을 표시합니다.
        DisplayNextSentence();

        // 대화가 완전히 끝날 때까지 대기합니다.
        yield return new WaitUntil(() => dialogueCompleted);
    }
    
    // Update는 사용자 입력을 처리하여 대화 진행을 관리합니다.
    void Update()
    {
        if (dialogueCompleted) return;

        // 스페이스바 입력만 처리하도록 수정했습니다.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandleInput();
        }
    }
    
    /// <summary>
    /// 사용자 입력을 처리하여 타이핑을 건너뛰거나 다음 문장으로 넘어갑니다.
    /// </summary>
    private void HandleInput()
    {
        if (isTyping)
        {
            // 타이핑 중이면 즉시 문장을 완료합니다.
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
            dialogueText.text = sentences[sentenceIndex];
            isTyping = false;
        }
        else
        {
            // 문장이 완료되었으면 다음 문장을 표시합니다.
            sentenceIndex++;
            DisplayNextSentence();
        }
    }

    /// <summary>
    /// 다음 문장을 표시하거나 대화를 종료합니다.
    /// </summary>
    private void DisplayNextSentence()
    {
        if (sentenceIndex >= sentences.Length)
        {
            // 모든 문장을 표시했습니다. 대화를 종료합니다.
            EndDialogue();
            return;
        }

        string currentSentence = sentences[sentenceIndex];
        
        // 이름/내용 분리 로직을 제거하고 전체 문장을 내용으로 사용합니다.
        string sentenceContent = currentSentence; 

        // 타이핑 코루틴 시작
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeSentence(sentenceContent));
    }

    /// <summary>
    /// 문장을 한 글자씩 출력하는 타이핑 효과를 구현합니다.
    /// </summary>
    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = ""; // 텍스트 초기화
        
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        
        isTyping = false;
    }

    /// <summary>
    /// 대화 처리를 완료하고 플래그를 설정합니다.
    /// </summary>
    private void EndDialogue()
    {
        dialogueCompleted = true;
        
        if (dialogueText != null)
        {
            dialogueText.text = ""; 
        }

        Debug.Log("[DialogueSystem] Dialogue ended.");
        // CafeManager의 StartScenarioDialogue 코루틴이 이 지점에서 재개됩니다.
    }
}