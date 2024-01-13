using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour
{
    //�⺻ ����
    [SerializeField]
    private string animalName;
    [SerializeField]
    private int hp;
    [SerializeField]
    private float walkSpeed;

    //��� �ð�
    [SerializeField]
    private float walkTime;
    [SerializeField]
    private float waitTime;
    private float currentTime;

    private bool isAction;//�ൿ �� �Ǵ�
    private bool isWalking;//�ȱ� �Ǵ�

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
        isAction = true;//�ൿ Ȱ��ȭ
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
            rb.MoveRotation(Quaternion.Euler(_rotation));//ȸ�� ����
        }
    }

    private void ElapseTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                AnimReset();//���� �ൿ ����
            }
        }
    }

    private void RandomAction()
    {
        isAction = true;

        int _random = Random.Range(0, 4);//�� Anim  ��: 4

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
        Debug.Log("���");
        currentTime = waitTime;
    }

    private void Eat()
    {
        Debug.Log("Ǯ���");
        currentTime = waitTime;
        anim.SetTrigger("Eat");
    }

    private void Peek()
    {
        Debug.Log("�θ���");
        currentTime = waitTime;
        anim.SetTrigger("Peek");
    }

    private void TryWalk()
    {
        isWalking = true;
        Debug.Log("�ȱ�");
        currentTime = walkTime;
        anim.SetBool("Walking",isWalking);
    }

    private void AnimReset()
    {
        direction.Set(0f, Random.Range(0f,360f), 0f);//�� ���� ����.
        isWalking = false;
        isAction = true;
        anim.SetBool("Walking", isWalking);
        RandomAction();
    }
}
