using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Water : MonoBehaviour
{
    //public static bool isWater;

    [SerializeField]
    private string sound_WaterOut;
    [SerializeField]
    private string sound_WaterIn;
    [SerializeField]
    private string sound_WaterBreath;

    [SerializeField]
    private float waterDrag;//물 저항
    private float originDrag;//기본 저항

    [SerializeField]
    private Color waterColor;
    [SerializeField]
    private float waterFogDensity;//물 탁함
    [SerializeField]
    private Color waterNightColor;
    [SerializeField]
    private float waterNightFogDensity;//물 탁함

    private Color originColor;//원 화면 색상
    private float originFogDensity;

    [SerializeField]
    private Color originNightColor;//밤 화면 색상
    [SerializeField]
    private float originNightFogDensity;

    [SerializeField]
    private float breatheTime;
    private float currentBreathTime;

    [SerializeField]
    private float totalOxygen;//산소 ㅇ양
    private float currentOxygen;

    [SerializeField]
    private GameObject go_baseUI;
    [SerializeField]
    private Text text_totalOxygen;
    [SerializeField]//UI 요소
    private Text text_currentOxygen;
    [SerializeField]
    private Image gauge;

    private StatusController sc;
    private float temp;
    // Start is called before the first frame update
    void Start()
    {
        originColor = RenderSettings.fogColor;
        originFogDensity = RenderSettings.fogDensity;

        sc = FindObjectOfType<StatusController>();
        currentOxygen = totalOxygen;
        originDrag = 0;
        text_totalOxygen.text = totalOxygen.ToString();
    }

    // Update is called once per frame
    void Update()
    {
       
        if (GameManager.isWater)
        {
            currentBreathTime += Time.deltaTime;
            SoundManager.instance.PlaySE(sound_WaterBreath);
            if (currentBreathTime >= breatheTime)
            {
                currentBreathTime = 0;
            }
        }

        DecreaseOxygen();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            GetInWater(other);
            SoundManager.instance.PlaySE(sound_WaterIn);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            GetOutWater(other);
            SoundManager.instance.PlaySE(sound_WaterOut);
        }
    }

    private void GetInWater(Collider _player)
    {
       // Debug.Log("Get IN Water");
        GameManager.isWater = true;

        go_baseUI.SetActive(true);
        _player.transform.GetComponent<Rigidbody>().drag = waterDrag;
        if (!GameManager.isNight)
        {
            RenderSettings.fogDensity = waterFogDensity;
            RenderSettings.fogColor = waterColor;
        }
        else
        {
            RenderSettings.fogDensity = waterNightFogDensity;
            RenderSettings.fogColor = waterNightColor;
        }
    }

    private void GetOutWater(Collider _player)
    {
        go_baseUI.SetActive(false);
        GameManager.isWater = false;
        _player.transform.GetComponent<Rigidbody>().drag = originDrag;

        if (!GameManager.isNight)
        {
            RenderSettings.fogDensity = originFogDensity;
            RenderSettings.fogColor = originColor;
        }
        else
        {
            RenderSettings.fogDensity = originNightFogDensity;
            RenderSettings.fogColor = originNightColor;
        }

        currentOxygen = totalOxygen;
    }

    private void DecreaseOxygen()
    {
        if (GameManager.isWater)
        {
            currentOxygen -= Time.deltaTime;
            text_currentOxygen.text = Mathf.Round(currentOxygen).ToString();
            gauge.fillAmount = currentOxygen / totalOxygen;

            if (currentOxygen <= 0)
            {
                temp += Time.deltaTime;
                if (temp >= 1)
                {
                    sc.DecreaseHP(1);
                    temp = 0;//1초마다 체력 감소
                }
                
            }
        }
    }
}
