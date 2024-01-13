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

    //대기 시간
    [SerializeField]
    private float walkTime;
    [SerializeField]
    private float waitTime;
    private float currentTime;

    private bool isAction;//행동 중 판단
    private bool isWalking;//걷기 판단

    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private BoxCollider boxCol;

    private Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        currentTime = waitTime;
        isAction = true;//행동 활성화
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotation();
        ElapseTime();
    }

    private void Move()
    {
        if (isWalking)
        {
            rb.MovePosition(transform.position + transform.forward * walkSpeed * Time.deltaTime);
        }
    }

    private void Rotation()
    {
        if (isWalking)
        {
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, direction, 0.01f);
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
        isWalking = true;
        Debug.Log("걷기");
        currentTime = walkTime;
        anim.SetBool("Walking",isWalking);
    }

    private void AnimReset()
    {
        direction.Set(0f, Random.Range(0f,360f), 0f);//값 랜덤 설정.
        isWalking = false;
        isAction = true;
        anim.SetBool("Walking", isWalking);
        RandomAction();
    }
}
