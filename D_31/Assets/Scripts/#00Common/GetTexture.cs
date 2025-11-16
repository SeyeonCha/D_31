using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LoadURLImage : MonoBehaviour
{
    public string url = "https://i.pinimg.com/1200x/32/4f/3a/324f3acbce1ef4483ebb1d3a319ce727.jpg";
    RawImage rawImage;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetTexture(url));
        rawImage = GetComponent<RawImage>();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator GetTexture(string url)
    {

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            rawImage.texture = myTexture;
        }
    }
}