using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static bool canPlayerMove = true;//�÷��̾��� �̵� ���� ����

    public static bool isOpenInventory = false; //�κ��丮 Ȱ��ȭ

    public static bool isOpenCraftMenu = false;

    public static bool isNight = false;

    public static bool isWater = false;

    public static bool isPause = false;//�޴� ȣ��

    private bool flag = false;//���ӿ� �־����� �÷���

    private WeaponManager wm;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;//�ڿ������� ������ ����
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
            Cursor.lockState = CursorLockMode.Locked;//�ڿ������� ������ ����
            Cursor.visible = false;
        }
        Debug.Log(isWater);
        if (isWater)
        {
            Debug.Log(flag + "WATER_IN");
            if (!flag)
            {
                Debug.Log("Weapon IN1");
                StopAllCoroutines();//�ڷ�ƾ ���� �� �ѹ��� ����ǰ� �÷��� ����
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
