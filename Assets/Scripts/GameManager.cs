using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static bool canPlayerMove = true;//�÷��̾��� �̵� ���� ����

    public static bool isOpenInventory = false; //�κ��丮 Ȱ��ȭ

    public static bool isOpenCraftMenu = false;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;//�ڿ������� ������ ����
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpenInventory||isOpenCraftMenu)
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
    }
}