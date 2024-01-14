using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject go_baseUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!GameManager.isPause)
            {
                CallMenu();
            }
            else
            {
                CloseMenu();
            }
        }
    }

    private void CallMenu()
    {
        GameManager.isPause = true;
        go_baseUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CloseMenu()
    {
        GameManager.isPause = false;
        go_baseUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ClickSave()
    {
        Debug.Log("세이브");
    }

    public void ClickLoad()
    {
        Debug.Log("로드");
    }

    public void ClickExit()
    {
        Debug.Log("나가기");
    }
}
