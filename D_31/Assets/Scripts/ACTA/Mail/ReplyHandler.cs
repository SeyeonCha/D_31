using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReplyHandler : MonoBehaviour
{
    public TextMeshProUGUI content;

    public void changeText(string txt)
    {
        content.text = txt;
    }
}
