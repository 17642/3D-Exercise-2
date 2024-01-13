using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour
{
    //기본 정보
    [SerializeField]
    private string animalName;
    [SerializeField]
    private int hp;
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;

    private float applySpeed;

    //대기 시간
    [SerializeField]
    private float walkTime;
    [SerializeField]
    private float waitTime;
    [SerializeField]
    private float runTime;
    private float currentTime;

    private bool isAction;//행동 중 판단
    private bool isWalking;//걷기 판단
    private bool isRunning;
    private bool isDead;

    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private BoxCollider boxCol;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip[] pig_sound;
    [SerializeField]
    private AudioClip pig_hurt;
    [SerializeField]
    private AudioClip pig_dead;

    private Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentTime = waitTime;
        isAction = true;//행동 활성화
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            Move();
            Rotation();
            ElapseTime();
        }
    }

    private void Move()
    {
        if (isWalking||isRunning)
        {
            rb.MovePosition(transform.position + transform.forward * applySpeed * Time.deltaTime);
        }
    }

    private void Rotation()
    {
        if (isWalking||isRunning)
        {
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, new Vector3(0f,direction.y,0f), 0.01f);//값의 y값만 반영
            rb.MoveRotation(Quaternion.Euler(_rotation));//회전 설정
        }
    }

    private void ElapseTime()
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

    private void RandomAction()
    {
        isAction = true;

        int _random = Random.Range(0, 4);//총 Anim  수: 4

        if(_random == 0)
        {
            Wait();
        }
        else if(_random == 1)
        {
            Eat();
        }
        else if(_random == 2)
        {
            Peek();
        }
        else if( _random == 3)
        {
            TryWalk();
        }
    }

    private void Wait()
    {
        Debug.Log("대기");
        currentTime = waitTime;
    }

    private void Eat()
    {
        Debug.Log("풀뜯기");
        currentTime = waitTime;
        anim.SetTrigger("Eat");
    }

    private void Peek()
    {
        Debug.Log("두리번");
        currentTime = waitTime;
        anim.SetTrigger("Peek");
    }

    private void TryWalk()
    {
        applySpeed = walkSpeed;
        isWalking = true;
        Debug.Log("걷기");
        currentTime = walkTime;
        anim.SetBool("Walking",isWalking);
    }

    public void Run(Vector3 _targetPos)
    {
        applySpeed = runSpeed;
        direction = Quaternion.LookRotation(transform.position - _targetPos).eulerAngles;//반대 방향 Direction 설정
        currentTime = runTime;
        isWalking = false;
        isRunning = true;
        anim.SetBool("Running", isRunning);
    }

    public void Damage(int _dmg, Vector3 _targetPos)
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
            PlaySE(pig_hurt);
            anim.SetTrigger("Hurt");
            Run(_targetPos);
        }
    }

    private void Dead()
    {
        PlaySE(pig_dead);
        isWalking = false;
        isRunning = false;
        anim.SetTrigger("Dead");
        isDead = true;
    }

    private void AnimReset()//애니메이션 리셋 후 재시작
    {
        RandomSound();
        applySpeed = walkSpeed;
        direction.Set(0f, Random.Range(0f,360f), 0f);//값 랜덤 설정.
        isWalking = false;
        isRunning = false;
        isAction = true;
        anim.SetBool("Walking", isWalking);
        anim.SetBool("Running", isRunning);
        RandomAction();
    }

    private void RandomSound()
    {
        int _random = Random.Range(0,pig_sound.Length);
        PlaySE(pig_sound[_random]);
    }

    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
}
