using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]//Ŭ������ ����ȭ
public class Sound//�� Ŭ������ ������Ʈ�� ����� �� ����.
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    #region Singleton
    //�̱���ȭ (�Ϲ������� 1���� ���)
    static public SoundManager instance;
    private void Awake()
    {
        if(instance == null)//instance�� �ƹ��͵� ���� ���
        {
            instance = this;//�ڱ� �ڽ� ����
            DontDestroyOnLoad(gameObject);//�� ��ȯ �� �ı� ����
        }
        else
        {
            Destroy(this.gameObject);//�ٸ� ���𰡰� ���� ���� ��� �ı�
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
        playSoundName = new string[audioSourceEffects.Length];//�Ҹ� �̸� �迭�� ������ҽ� ���� ��ġ
    }

    public void PlaySE(string _name)
    {
        for(int i = 0; i < effectSounds.Length; i++)
        {
            if (_name == effectSounds[i].name)//�´� �̸��� SoundEffect Ž��
            {
                for(int j=0; j < audioSourceEffects.Length; j++)//�� AudioSource Ž��
                {
                    if (!audioSourceEffects[j].isPlaying)
                    {
                        playSoundName[j] = effectSounds[i].name;//�̸� ����
                        audioSourceEffects[j].clip = effectSounds[i].clip;
                        audioSourceEffects[j].Play();
                        return;
                    }
                }
                Debug.Log("��� AUDIOSOURCE �����");
                return;
            }
            
        }
        Debug.Log(_name + " ���尡 ��ϵ��� �ʾҽ��ϴ�.");

    }

    public void StopAllSE()
    {
        for(int i = 0; i < audioSourceEffects.Length; i++)
        {
            audioSourceEffects[i].Stop();//��� AudioSource Stop;
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
        Debug.Log("��� ����" + _name + "���尡 �����ϴ�.");
    }

}
