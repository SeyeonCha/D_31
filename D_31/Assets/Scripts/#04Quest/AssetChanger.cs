using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AssetChanger : MonoBehaviour
{
    private TextMeshProUGUI assetText;

    private void Awake()
    {
        assetText = GetComponent<TextMeshProUGUI>();
        Debug.Log($"{assetText.text}");
    }

    public void ChangeAsset(int change)
    {
        string currentText = assetText.text.Replace(",","");
        if (int.TryParse(currentText, out int currentAsset))
        {
            
            int newAsset = currentAsset + change;
            assetText.text = newAsset.ToString("N0");
            Debug.Log($"{assetText.text}");
        }
        else
        {
            Debug.Log($"not Parsing...");
        }
    }
}
