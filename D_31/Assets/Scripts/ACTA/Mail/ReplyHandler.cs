using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReplyHandler : MonoBehaviour
{
    public TextMeshProUGUI content;

    public void changeText(string txt)
    {
        content.text = txt;
    }
    public void ActivateButton(bool b)
    {
        content.GetComponent<Button>().enabled = b;
    }
}
