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
    [SerializeField]
    private float runSpeed;

    private float applySpeed;

    //��� �ð�
    [SerializeField]
    private float walkTime;
    [SerializeField]
    private float waitTime;
    [SerializeField]
    private float runTime;
    private float currentTime;

    private bool isAction;//�ൿ �� �Ǵ�
    private bool isWalking;//�ȱ� �Ǵ�
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
        isAction = true;//�ൿ Ȱ��ȭ
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
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, new Vector3(0f,direction.y,0f), 0.01f);//���� y���� �ݿ�
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
        applySpeed = walkSpeed;
        isWalking = true;
        Debug.Log("�ȱ�");
        currentTime = walkTime;
        anim.SetBool("Walking",isWalking);
    }

    public void Run(Vector3 _targetPos)
    {
        applySpeed = runSpeed;
        direction = Quaternion.LookRotation(transform.position - _targetPos).eulerAngles;//�ݴ� ���� Direction ����
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
                Debug.Log("ü�� 0 ����");
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

    private void AnimReset()//�ִϸ��̼� ���� �� �����
    {
        RandomSound();
        applySpeed = walkSpeed;
        direction.Set(0f, Random.Range(0f,360f), 0f);//�� ���� ����.
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
