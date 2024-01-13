using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]//클래스를 직렬화
public class Sound//이 클래스는 컴포넌트로 사용할 수 없음.
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    #region Singleton
    //싱글톤화 (일반적으로 1개만 사용)
    static public SoundManager instance;
    private void Awake()
    {
        if(instance == null)//instance에 아무것도 없을 경우
        {
            instance = this;//자기 자신 삽입
            DontDestroyOnLoad(gameObject);//씬 전환 시 파괴 방지
        }
        else
        {
            Destroy(this.gameObject);//다른 무언가가 들어와 있을 경우 파괴
        }
    }
    #endregion Singleton
    public AudioSource[] audioSourceEffects;
    public AudioSource audioSourceBgm;

    [SerializeField]
    public Sound[] effectSounds;
    [SerializeField]
    public Sound[] bgmSounds;

    public string[] playSoundName;

    private void Start()
    {
        playSoundName = new string[audioSourceEffects.Length];//소리 이름 배열을 오디오소스 수와 일치
    }

    public void PlaySE(string _name)
    {
        for(int i = 0; i < effectSounds.Length; i++)
        {
            if (_name == effectSounds[i].name)//맞는 이름의 SoundEffect 탐색
            {
                for(int j=0; j < audioSourceEffects.Length; j++)//빈 AudioSource 탐색
                {
                    if (!audioSourceEffects[j].isPlaying)
                    {
                        playSoundName[j] = effectSounds[i].name;//이름 지정
                        audioSourceEffects[j].clip = effectSounds[i].clip;
                        audioSourceEffects[j].Play();
                        return;
                    }
                }
                Debug.Log("모든 AUDIOSOURCE 사용중");
                return;
            }
            
        }
        Debug.Log(_name + " 사운드가 등록되지 않았습니다.");

    }

    public void StopAllSE()
    {
        for(int i = 0; i < audioSourceEffects.Length; i++)
        {
            audioSourceEffects[i].Stop();//모든 AudioSource Stop;
        }
    }

    public void StopSE(string _name)
    {
        for(int i=0;i<audioSourceEffects.Length; i++)
        {
            if (playSoundName[i] == _name)
            {
                audioSourceEffects[i].Stop();
                break;
            }
        }
        Debug.Log("재생 중인" + _name + "사운드가 없습니다.");
    }

}
