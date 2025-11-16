using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Dialogue_eye : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    [TextArea(3, 10)]
    public string[] sentences;
    public float typingSpeed = 0.05f;
    public string nextSceneName = "";

    private int index = 0;
    private bool isTyping = false;
    private bool isSentenceComplete = false;

    void Start()
    {
        if (sentences.Length > 0)
            StartCoroutine(TypeSentence(sentences[index]));
    }

    void Update()
    {
        if (KeyboardSpacePressed())
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = sentences[index];
                isTyping = false;
                isSentenceComplete = true;
            }
            else if (isSentenceComplete)
            {
                NextSentence();
            }
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        isSentenceComplete = false;
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        isSentenceComplete = true;
    }

    void NextSentence()
    {
        if (index < sentences.Length - 1)
        {
            index++;
            StartCoroutine(TypeSentence(sentences[index]));
        }
        else
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    bool KeyboardSpacePressed()
    {
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
        return UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame;
#else
        return Input.GetKeyDown(KeyCode.Space);
#endif
    }
}
