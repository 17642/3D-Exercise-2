using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public string sceneName = "GameStage";//다음 Scene 이름
    

    public void ClickStart()
    {
        Debug.Log("LOADING" + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    public void ClickExit()
    {
        Debug.Log("종료");//Quit는 TestPlay에서 실행되지 않음.
        Application.Quit();//
    }

    public void ClickLoad()
    {
        Debug.Log("로-드");
    }
}
