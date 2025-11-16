using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneFlowManager : MonoBehaviour
{
    public GameObject HeadlinePanel;
    public GameObject headlineContent;
    public TypingEffect headlineTypingEffect;
    //public GameObject spacebarHintText_H;

    public GameObject NewsPanel;

    public GameObject newsContent;
    public Animator anchorAnimator;
    public TypingEffect dialogueTypingEffect;
    //public GameObject spacebarHintText;

    [TextArea(3, 10)]
    public string[] dialogueSentences;
    public string nextSceneName;

    private int currentPhase = 0;
    private int sentenceIndex = 0;

    void Start()
    {
        HeadlinePanel.SetActive(true);
        NewsPanel.SetActive(false);

        headlineTypingEffect.StartTyping(headlineTypingEffect.GetComponent<TMP_Text>().text);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandleSpacebarPress();
        }
    }

    void HandleSpacebarPress()
    {
        if (currentPhase == 0)
        {
            if (headlineTypingEffect.IsTyping)
            {
                headlineTypingEffect.SkipTyping(headlineTypingEffect.GetComponent<TMP_Text>().text);
            }
            else
            {
                TransitionToNewsReport();
            }
        }
        else if (currentPhase == 1)
        {
            if (dialogueTypingEffect.IsTyping)
            {
                dialogueTypingEffect.SkipTyping(dialogueSentences[sentenceIndex]);
            }
            else
            {
                sentenceIndex++;
                
                if (sentenceIndex < dialogueSentences.Length)
                {
                    dialogueTypingEffect.StartTyping(dialogueSentences[sentenceIndex]);
                }
                else
                {
                    SceneManager.LoadScene(nextSceneName);
                }
            }
        }
    }

    void TransitionToNewsReport()
    {
        currentPhase = 1;
        
        NewsPanel.SetActive(true);
        HeadlinePanel.SetActive(false);

        if (anchorAnimator != null) anchorAnimator.enabled = true;

        if (dialogueSentences.Length > 0)
        {
            sentenceIndex = 0;
            dialogueTypingEffect.StartTyping(dialogueSentences[sentenceIndex]);
        }
    }
}