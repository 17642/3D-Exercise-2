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
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;//자연스럽게 보이지 않음
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
            Cursor.lockState = CursorLockMode.Locked;//자연스럽게 보이지 않음
            Cursor.visible = false;
        }
    }
}
