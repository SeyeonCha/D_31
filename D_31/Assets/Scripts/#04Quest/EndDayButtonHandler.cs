using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDayButtonHandler : MonoBehaviour
{

    // private void Update()
    // {
    //     if (GameManager.Instance.missionCompleted == true)
    //     {
    //         GameManager.Instance.missionCompleted = false;
    //     }
        
    // }
    public void EndDayButtonClicked()
    {
        if (GameManager.Instance.missionCompleted == true)
        {
            GameManager.Instance.missionCompleted = false;
            // SceneManager.LoadScene("#03News"); // 다음 데이의 뉴스로 넘어가면 됨. 
            Debug.Log("ToNextDay()실행 in EndDayButton");
            GameManager.Instance.ToNextDay();
            
        }
        
    }
}
