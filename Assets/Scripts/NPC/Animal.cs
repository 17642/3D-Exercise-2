using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    //�⺻ ����
    [SerializeField]
    protected string animalName;
    [SerializeField]
    protected int hp;
    [SerializeField]
    protected float walkSpeed;
    [SerializeField]
    protected float runSpeed;
    [SerializeField]
    protected float turningSpeed;

    protected float applySpeed;

    //��� �ð�
    [SerializeField]
    protected float walkTime;
    [SerializeField]
    protected float waitTime;
    [SerializeField]
    protected float runTime;
    protected float currentTime;

    protected bool isAction;//�ൿ �� �Ǵ�
    protected bool isWalking;//�ȱ� �Ǵ�
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

    protected Vector3 direction;
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

    protected void Move()
    {
        if (isWalking || isRunning)
        {
            rb.MovePosition(transform.position + transform.forward * applySpeed * Time.deltaTime);
        }
    }

    protected void Rotation()
    {
        if (isWalking || isRunning)
        {
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, new Vector3(0f, direction.y, 0f), turningSpeed);//���� y���� �ݿ�
            rb.MoveRotation(Quaternion.Euler(_rotation));//ȸ�� ����
        }
    }

    protected void ElapseTime()
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





    protected void TryWalk()
    {
        applySpeed = walkSpeed;
        isWalking = true;
        Debug.Log("�ȱ�");
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
                Debug.Log("ü�� 0 ����");
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

    protected virtual void AnimReset()//�ִϸ��̼� ���� �� �����
    {
        //RandomSound();
        applySpeed = walkSpeed;
        direction.Set(0f, Random.Range(0f, 360f), 0f);//�� ���� ����.
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
