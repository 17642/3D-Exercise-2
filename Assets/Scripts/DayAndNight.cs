using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    [SerializeField]
    private float secondPerRealTimeSecond;//���ǰ� ���ӻ��� �ð��� ��
    //Fog �а�

    [SerializeField]
    private float fogDensityCalc;//�Ȱ� �е� ���
    [SerializeField]
    private float nightFogDensity;
    [SerializeField]
    private float dayFogDensity;
    private float currentFogDensity;//���

    private bool isNight = false;
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
            isNight = true;//���� ������ ���� �̻��̸� night Ȱ��ȭ
        }else if(transform.eulerAngles.x <= 340.0f)
        {
            isNight = false;
        }

        if (isNight)
        {
            if (currentFogDensity <= nightFogDensity)
            {
                currentFogDensity += 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;//�Ȱ� �е� ����
            }
        }
        else
        {
            if (currentFogDensity >= dayFogDensity)
            {
                currentFogDensity -= 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;//�Ȱ� �е� ����
            }
        }
    }
}
