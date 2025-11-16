using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EyeAnimation: MonoBehaviour
{
    // 순환할 이미지 배열
    public Image[] imagesToCycle;

    // 이미지 1부터 마지막 이미지까지 머무르는 고정 시간 (초)
    public float cycleInterval = 1.0f;

    // 이미지 0이 머무르는 랜덤 시간의 최소값과 최대값 (Inspector에서 설정 가능)
    [Header("Random Delay for Image 0")]
    [Tooltip("Image 0이 활성화되었을 때 최소 머무르는 시간 (초).")]
    public float minRandomDelay = 1.0f;
    [Tooltip("Image 0이 활성화되었을 때 최대 머무르는 시간 (초).")]
    public float maxRandomDelay = 10.0f; 

    private int currentIndex = 0;

    void Start()
    {
        // 모든 이미지를 비활성화합니다.
        foreach (Image img in imagesToCycle)
        {
            img.gameObject.SetActive(false);
        }

        // 이미지가 존재하면 첫 번째 이미지를 활성화하고 코루틴을 시작합니다.
        if (imagesToCycle.Length > 0)
        {
            imagesToCycle[0].gameObject.SetActive(true);
            StartCoroutine(SwitchImages());
        }
    }

    private IEnumerator SwitchImages()
    {
        while (true)
        {
            float delayDuration;

            // 현재 활성화된 이미지(아직 꺼지지 않은)가 이미지 0인지 확인합니다.
            if (currentIndex == 0)
            {
                // 이미지 0일 경우, 1초에서 10초 사이의 랜덤한 시간을 설정합니다.
                // Random.Range(min, max)는 float의 경우 max(최대값) 포함하지 않으므로, 10.0f 대신 10.0001f를 사용할 수도 있지만,
                // 여기서는 직관성을 위해 maxRandomDelay를 그대로 사용합니다.
                delayDuration = Random.Range(minRandomDelay, maxRandomDelay);
            }
            else
            {
                // 이미지 1부터는 고정된 cycleInterval을 사용합니다.
                delayDuration = cycleInterval;
            }
            
            // 계산된 시간만큼 대기합니다.
            yield return new WaitForSeconds(delayDuration);

            // 현재 이미지를 비활성화합니다.
            imagesToCycle[currentIndex].gameObject.SetActive(false);

            // 다음 인덱스로 이동합니다.
            currentIndex = (currentIndex + 1) % imagesToCycle.Length;

            // 다음 이미지를 활성화합니다.
            imagesToCycle[currentIndex].gameObject.SetActive(true);
        }
    }
}