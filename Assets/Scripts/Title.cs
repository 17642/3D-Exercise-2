using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public string sceneName = "GameStage";//다음 Scene 이름

    public static Title instance;//싱글톤

    private SaveNLoad saves;
    private void Awake()
    {
        
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);//타이틀 인스턴스 생성
        }
        else
        {
            Destroy(this.gameObject);//있을 경우에는 자신 삭제
        }
    }
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
        StartCoroutine(LoadCoroutine());
        //SceneManager.LoadScene(sceneName);//플레이어 개체 생성을 위해 Scene 이동 후 로드
        //saves.LoadData();//로드;
    }

    IEnumerator LoadCoroutine()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        //SceneManager.LoadScene(sceneName);



        while (!operation.isDone)//operation에 지정된 작업이 끝날 때까지
        {
            yield return null;//대기
        }

        saves = FindObjectOfType<SaveNLoad>();//로딩이 끝났을 때 세이브파일 불러옴

        saves.LoadData();
    }
}
