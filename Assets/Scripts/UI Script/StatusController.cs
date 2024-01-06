using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class StatusController : MonoBehaviour
{
    // Start is called before the first frame update
    //ü��
    [SerializeField]
    private int hp;//�ִ� , ����
    private int currentHp;
    //���¹̳�
    [SerializeField]
    private int sp;
    private int currentSp;
    //���¹̳� ������
    [SerializeField]
    private int spIncreaseSpeed;
    //���¹̳� ȸ�� ������
    [SerializeField]
    private int spRechargeTime;
    private int currentSpRechargeTime;
    //���¹̳� ���� ����
    private bool spUsed;
    //����
    [SerializeField]
    private int dp;
    private int currentDp;
    //�����
    [SerializeField]
    private int hungry;
    private int currentHungry;

    [SerializeField]
    private int hungryDecreaseTime;
    private int currentHungryDecreaseTime;

    [SerializeField]
    private int thirsty;
    private int currentThirsty;

    [SerializeField]
    private int thirstyDecreaseTime;
    private int currentThirstyDecreaseTime;
    //������
    [SerializeField]
    private int satisfy;
    private int currentSatisfy;
    //�ʿ� �̹���
    [SerializeField]
    private Image[] images_Gauge;
    //�迭 ��� ���
    private const int HP = 0, DP = 1, SP = 2, HUNGRY = 3, THIRSTY = 4, SATISFY = 5;
    void Start()
    {
        //�ʱⰪ �Է�
        currentHp = hp;
        currentSp = sp;
        currentDp = dp;
        currentHungry = hungry;
        currentThirsty = thirsty;
        currentSatisfy = satisfy;
    }

    // Update is called once per frame
    void Update()
    {
        Hungry();
        Thirsty();
        GaugeUpdate();
        SpRechargeTime();
        SpRecover();
    }

    private void Hungry()
    {
        if(currentHungry > 0)
        {
            if (currentHungryDecreaseTime <= hungryDecreaseTime)
            {
                currentHungryDecreaseTime++;
            }
            else
            {
                currentHungry--;
                currentHungryDecreaseTime = 0;
            }
        }
        else
        {
            Debug.Log("����� ��ġ 0");
        }
    }
    private void Thirsty()
    {
        if (currentThirsty > 0)
        {
            if (currentThirstyDecreaseTime <= thirstyDecreaseTime)
            {
                currentThirstyDecreaseTime++;
            }//�񸶸� Ÿ�̸� üũ
            else
            {
                currentThirsty--;
                currentThirstyDecreaseTime = 0;
            }//�񸶸� ���� �� Ÿ�̸� �ʱ�ȭ
        }
        else
        {
            Debug.Log("�񸶸� ��ġ 0");
        }
    }

    private void GaugeUpdate()
    {
        images_Gauge[HP].fillAmount = (float)currentHp / hp;
        images_Gauge[SP].fillAmount = (float)currentSp / sp;
        images_Gauge[DP].fillAmount = (float)currentDp / dp;
        images_Gauge[HUNGRY].fillAmount = (float)currentHungry / hungry;
        images_Gauge[THIRSTY].fillAmount = (float)currentThirsty / thirsty;
        images_Gauge[SATISFY].fillAmount = (float)currentSatisfy / satisfy;
    }

    public void DecreaseStamina(int _count)
    {
        spUsed = true;
        currentSpRechargeTime = 0;//SP ���� �ø��� ȸ�� �ð� ����

        if(currentSp - _count < 0)
        {
            currentSp = 0;
        }
        else
        {
            currentSp -= _count;
        }
    }

    private void SpRechargeTime()
    {//SPUSED ����
        if(spUsed == true)
        {
            if (currentSpRechargeTime < spRechargeTime)
            {
                currentSpRechargeTime++;
            }
            else
            {
                spUsed = false;
            }
        }
    }

    private void SpRecover()
    {//SPUSED�� ���� SP ȸ�� ����
        if (!spUsed && currentSp < sp)
        {
            currentSp += spIncreaseSpeed;
        }
    }
}