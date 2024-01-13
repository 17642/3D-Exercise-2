using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    //기본 정보
    [SerializeField]
    protected string animalName;
    [SerializeField]
    protected int hp;
    [SerializeField]
    protected float walkSpeed;
    [SerializeField]
    protected float runSpeed;
    //[SerializeField]
   // protected float turningSpeed;

    //protected float applySpeed;

    //대기 시간
    [SerializeField]
    protected float walkTime;
    [SerializeField]
    protected float waitTime;
    [SerializeField]
    protected float runTime;
    protected float currentTime;

    protected bool isAction;//행동 중 판단
    protected bool isWalking;//걷기 판단
    protected bool isRunning;
    protected bool isDead;

    [SerializeField]
    protected Animator anim;
    [SerializeField]
    protected Rigidbody rb;
    [SerializeField]
    protected BoxCollider boxCol;
    protected AudioSource audioSource;
    [SerializeField]
    protected AudioClip[] normal_sound;
    [SerializeField]
    protected AudioClip hurt_sound;
    [SerializeField]
    protected AudioClip dead_sound;
    protected NavMeshAgent nav;

    protected Vector3 destination;//목적지
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentTime = waitTime;
        isAction = true;//행동 활성화
        nav=GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            Move();
            //Rotation();
            ElapseTime();
        }
    }

    protected void Move()
    {
        if (isWalking || isRunning)
        {
            //rb.MovePosition(transform.position + transform.forward * applySpeed * Time.deltaTime);
            nav.SetDestination(transform.position+destination*5f);
        }
    }

    /*
    protected void Rotation()
    {
        if (isWalking || isRunning)
        {
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, new Vector3(0f, direction.y, 0f), turningSpeed);//값의 y값만 반영
            rb.MoveRotation(Quaternion.Euler(_rotation));//회전 설정
        }
    }
    */

    protected void ElapseTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                AnimReset();//랜덤 행동 개시
            }
        }
    }





    protected void TryWalk()
    {
        nav.speed = walkSpeed;
        isWalking = true;
        Debug.Log("걷기");
        currentTime = walkTime;
        anim.SetBool("Walking", isWalking);
    }

    

    public virtual void Damage(int _dmg, Vector3 _targetPos)
    {
        if (!isDead)
        {
            hp -= _dmg;
            if (hp <= 0)
            {
                Dead();
                Debug.Log("체력 0 이하");
                return;
            }
            PlaySE(hurt_sound);
            anim.SetTrigger("Hurt");
        }
    }

    protected void Dead()
    {
        PlaySE(dead_sound);
        isWalking = false;
        isRunning = false;
        anim.SetTrigger("Dead");
        isDead = true;
    }

    protected virtual void AnimReset()//애니메이션 리셋 후 재시작
    {
        //RandomSound();
        nav.speed = walkSpeed;
        nav.ResetPath();
        destination.Set(Random.Range(-0.2f,0.2f),0f,Random.Range(-0.1f,0.5f));//값 랜덤 설정.
        isWalking = false;
        isRunning = false;
        isAction = true;
        anim.SetBool("Walking", isWalking);
        anim.SetBool("Running", isRunning);
       // RandomAction();
    }

    protected void RandomSound()
    {
        int _random = Random.Range(0, normal_sound.Length);
        PlaySE(normal_sound[_random]);
    }

    protected void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
}
