using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class AppButtonHandler : MonoBehaviour, IPointerClickHandler
{
    
    public string nextSceneName = "ACTA_Mainpage";

    public void OnPointerClick(PointerEventData eventData)
    {
        SceneManager.LoadScene(nextSceneName, LoadSceneMode.Additive);
    }
}
