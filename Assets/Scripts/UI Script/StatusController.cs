using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class StatusController : MonoBehaviour
{
    // Start is called before the first frame update
    //체력
    [SerializeField]
    private int hp;//최대 , 현재
    private int currentHp;
    //스태미나
    [SerializeField]
    private int sp;
    private int currentSp;
    //스태미나 증가율
    [SerializeField]
    private int spIncreaseSpeed;
    //스태미나 회복 딜레이
    [SerializeField]
    private int spRechargeTime;
    private int currentSpRechargeTime;
    //스태미나 감소 여부
    private bool spUsed;
    //방어력
    [SerializeField]
    private int dp;
    private int currentDp;
    //배고픔
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
    //만족도
    [SerializeField]
    private int satisfy;
    private int currentSatisfy;
    //필요 이미지
    [SerializeField]
    private Image[] images_Gauge;
    //배열 요소 상수
    private const int HP = 0, DP = 1, SP = 2, HUNGRY = 3, THIRSTY = 4, SATISFY = 5;
    void Start()
    {
        //초기값 입력
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
            Debug.Log("배고픔 수치 0");
        }
    }
    private void Thirsty()
    {
        if (currentThirsty > 0)
        {
            if (currentThirstyDecreaseTime <= thirstyDecreaseTime)
            {
                currentThirstyDecreaseTime++;
            }//목마름 타이머 체크
            else
            {
                currentThirsty--;
                currentThirstyDecreaseTime = 0;
            }//목마름 감소 및 타이머 초기화
        }
        else
        {
            Debug.Log("목마름 수치 0");
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
        currentSpRechargeTime = 0;//SP 감소 시마다 회복 시간 리셋

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
    {//SPUSED 제어
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
    {//SPUSED에 따라 SP 회복 제어
        if (!spUsed && currentSp < sp)
        {
            currentSp += spIncreaseSpeed;
        }
    }

    public int GetCurrentSP()
    {
        return currentSp;
    }

    public void IncreaseHP(int _count)
    {
        if (currentHp + _count < hp)
        {
            currentHp += _count;
        }
        else
        {
            currentHp = hp;
        }
    }
    public void DecreaseHP(int _count)
    {

        if (currentDp > 0) {//DP가 있을 경우 DP를 우선 차감
            DecreaseDP(_count);
            return;
        }

            currentHp -= _count;

        if (currentHp < 0) { Debug.Log("캐릭터의 HP가 0이 되었습니다."); }
        

    }

    public void IncreaseDP(int _count)
    {
        if (currentDp + _count < dp)
        {
            currentDp += _count;
        }
        else
        {
            currentDp = hp;
        }
    }
    public void DecreaseDP(int _count)
    {

        currentDp -= _count;

        if (currentDp < 0) { Debug.Log("방어력이 0이 되었습니다."); }

    }

    public void IncreaseHungry(int _count)
    {
        if (currentHungry + _count < hungry)
        {
            currentHungry += _count;
        }
        else
        {
            currentHungry = hungry;
        }
    }
    public void DecreaseHungry(int _count)
    {

        if (currentHungry - _count < 0)
        {
            currentHungry = 0;
        }
        else
        {
            currentHungry -= _count;
        }
    }
    public void IncreaseThirsty(int _count)
    {
        if (currentThirsty + _count < thirsty)
        {
            currentThirsty += _count;
        }
        else
        {
            currentThirsty = thirsty;
        }
    }
    public void DecreaseThirsty(int _count)
    {

        if (currentThirsty - _count < 0)
        {
            currentThirsty = 0;
        }
        else
        {
            currentThirsty -= _count;
        }
    }
}
