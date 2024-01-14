using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static bool canPlayerMove = true;//플레이어의 이동 가능 여부

    public static bool isOpenInventory = false; //인벤토리 활성화

    public static bool isOpenCraftMenu = false;

    public static bool isNight = false;

    public static bool isWater = false;

    public static bool isPause = false;//메뉴 호출

    private bool flag = false;//물속에 있었는지 플래그

    private WeaponManager wm;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;//자연스럽게 보이지 않음
        Cursor.visible = false;

        wm = FindObjectOfType<WeaponManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpenInventory||isOpenCraftMenu||isPause)
        {
            canPlayerMove = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            canPlayerMove = true;
            Cursor.lockState = CursorLockMode.Locked;//자연스럽게 보이지 않음
            Cursor.visible = false;
        }
        Debug.Log(isWater);
        if (isWater)
        {
            Debug.Log(flag + "WATER_IN");
            if (!flag)
            {
                Debug.Log("Weapon IN1");
                StopAllCoroutines();//코루틴 정리 및 한번만 실행되게 플래그 설정
                StartCoroutine(wm.WeaponInCoroutine());
                flag = true;
            }
        }
        else
        {
            if (flag)
            {
                wm.WeaponOut();
                flag = false;
            }
        }
    }
}
