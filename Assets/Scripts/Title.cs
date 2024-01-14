using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public string sceneName = "GameStage";//���� Scene �̸�
    

    public void ClickStart()
    {
        Debug.Log("LOADING" + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    public void ClickExit()
    {
        Debug.Log("����");//Quit�� TestPlay���� ������� ����.
        Application.Quit();//
    }

    public void ClickLoad()
    {
        Debug.Log("��-��");
    }
}
