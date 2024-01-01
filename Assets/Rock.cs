using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField]
    private int hp;//ü��
    [SerializeField]
    private float delayTime;//���� ���� �ð�
    [SerializeField]
    private SphereCollider coll;//���� �ݶ��̴�

    //���ӿ�����Ʈ ��� ��������
    [SerializeField]
    private GameObject go_rock;//�Ϲ� ����
    [SerializeField]
    private GameObject go_debris;//���� ����

    [SerializeField]
    private GameObject go_effect_prefabs;//����Ʈ ������

    //�ӽ� �����
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip effect_sound;
    [SerializeField]
    private AudioClip effect_sound2;


    public void Mining()
    {
        audioSource.clip = effect_sound;
        audioSource.Play();
        var clone=  Instantiate(go_effect_prefabs,coll.bounds.center,Quaternion.identity);//����Ʈ ��ü�� ������ ����
        Destroy(clone, delayTime);//����Ʈ ���� �ð� �� �ı�
        hp--;
        if (hp <= 0)
        {
            Destruction();
        }
    }

    private void Destruction()
    {
        audioSource.clip = effect_sound2;
        audioSource.Play();
        coll.enabled = false;//�ݶ��̴� ��Ȱ��ȭ
        Destroy(go_rock);//�Ϲ� ���� �ı�
        go_debris.SetActive(true);//���� Ȱ��ȭ
        Destroy(go_debris, delayTime);//���� �ð� �� ���� ����
    }
}
