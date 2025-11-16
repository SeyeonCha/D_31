using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReturnToQuest : MonoBehaviour
{
   public void ReturnToPreviousScene()
    {

        // SceneManager.LoadScene(previousSceneName, LoadSceneMode.Single);
        // 현재 씬(Scene B)의 이름을 가져옵니다.
        // 현재 스크립트가 붙어있는 오브젝트의 씬을 찾습니다.
        Scene currentScene = gameObject.scene;
        SceneManager.UnloadSceneAsync(currentScene);
    }
}
