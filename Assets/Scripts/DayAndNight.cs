using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    [SerializeField]
    private float secondPerRealTimeSecond;//현실과 게임상의 시간의 비
    //Fog 밀고

    [SerializeField]
    private float fogDensityCalc;//안개 밀도 계산
    [SerializeField]
    private float nightFogDensity;
    [SerializeField]
    private float dayFogDensity;
    private float currentFogDensity;//계산
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.right, 0.1f * secondPerRealTimeSecond * Time.deltaTime);
        if (transform.eulerAngles.x >= 170)
        {
            GameManager.isNight = true;//해의 각도가 일정 이상이면 night 활성화
        }else if(transform.eulerAngles.x <= 340.0f)
        {
            GameManager.isNight = false;
        }

        if (GameManager.isNight)
        {
            if (currentFogDensity <= nightFogDensity)
            {
                currentFogDensity += 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;//안개 밀도 설정
            }
        }
        else
        {
            if (currentFogDensity >= dayFogDensity)
            {
                currentFogDensity -= 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;//안개 밀도 설정
            }
        }
    }
}
