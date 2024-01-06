using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField]
    private int hp;//체력
    [SerializeField]
    private float delayTime;//파편 제거 시간
    [SerializeField]
    private SphereCollider coll;//바위 콜라이더

    //게임오브젝트 요소 가져오기
    [SerializeField]
    private GameObject go_rock;//일반 바위
    [SerializeField]
    private GameObject go_debris;//깨진 바위
    [SerializeField]
    private GameObject go_rockItemPrefab;//아이템 프리팹
    [SerializeField]
    private int count;//아이템 등장 수

    [SerializeField]
    private GameObject go_effect_prefabs;//이펙트 프리팹

    /*//임시 오디오
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
        var clone=  Instantiate(go_effect_prefabs,coll.bounds.center,Quaternion.identity);//이펙트 물체를 바위에 생성
        Destroy(clone, delayTime);//이펙트 지속 시간 후 파괴
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
        coll.enabled = false;//콜라이더 비활성화
        for(int i = 0; i < count; i++)
        {
            Instantiate(go_rockItemPrefab, go_rock.transform.position, Quaternion.identity);
        }
        Destroy(go_rock);//일반 바위 파괴
        go_debris.SetActive(true);//파편 활성화
        Destroy(go_debris, delayTime);//일정 시간 후 파편 삭제
    }
}
