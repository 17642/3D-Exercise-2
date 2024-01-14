using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public string sceneName = "GameStage";//���� Scene �̸�

    public static Title instance;//�̱���

    private SaveNLoad saves;
    private void Awake()
    {
        
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);//Ÿ��Ʋ �ν��Ͻ� ����
        }
        else
        {
            Destroy(this.gameObject);//���� ��쿡�� �ڽ� ����
        }
    }
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
        StartCoroutine(LoadCoroutine());
        //SceneManager.LoadScene(sceneName);//�÷��̾� ��ü ������ ���� Scene �̵� �� �ε�
        //saves.LoadData();//�ε�;
    }

    IEnumerator LoadCoroutine()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        //SceneManager.LoadScene(sceneName);



        while (!operation.isDone)//operation�� ������ �۾��� ���� ������
        {
            yield return null;//���
        }

        saves = FindObjectOfType<SaveNLoad>();//�ε��� ������ �� ���̺����� �ҷ���

        saves.LoadData();
    }
}
