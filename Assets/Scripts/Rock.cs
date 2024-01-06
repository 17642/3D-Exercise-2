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
    private GameObject go_rockItemPrefab;//������ ������
    [SerializeField]
    private int count;//������ ���� ��

    [SerializeField]
    private GameObject go_effect_prefabs;//����Ʈ ������

    /*//�ӽ� �����
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip effect_sound;
    [SerializeField]
    private AudioClip effect_sound2;
    */
    [SerializeField]
    private string strike_sound;
    [SerializeField]
    private string destroy_sound;


    public void Mining()
    {
        SoundManager.instance.PlaySE(strike_sound);
        //audioSource.clip = effect_sound;
        //audioSource.Play();
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
        //audioSource.clip = effect_sound2;
        //audioSource.Play();
        SoundManager.instance.PlaySE(destroy_sound);
        coll.enabled = false;//�ݶ��̴� ��Ȱ��ȭ
        for(int i = 0; i < count; i++)
        {
            Instantiate(go_rockItemPrefab, go_rock.transform.position, Quaternion.identity);
        }
        Destroy(go_rock);//�Ϲ� ���� �ı�
        go_debris.SetActive(true);//���� Ȱ��ȭ
        Destroy(go_debris, delayTime);//���� �ð� �� ���� ����
    }
}
