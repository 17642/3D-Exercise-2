using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // Start is called before the first frame update
    public string gunName;
    public float range;//��Ÿ�
    public float accuracy;//��Ȯ��
    public float fireRate;//�����
    public float reloadTime;//������ �ӵ�

    public int damage;//������

    public int reloadBullet;//źâ �� ź ��
    public int currentBulletCount;//źâ �� �Ѿ� ��
    public int maxBulletCount;//�ִ� �Ѿ� ��
    public int carryBulletCount;//�����ϴ� �Ѿ��� ��

    public float retroActionForce;//�ݵ� ����
    public float retroActionFineSightForce;//������ �ݵ� ����

    public Vector3 fineSightOriginPos; // �ݵ� ���ƿ� ��ġ

    public Animator anim; // �� �ִϸ�����

    public ParticleSystem muzzleFlash;//�ѱ�ȭ��

    public AudioClip FireSound;//�߻� �Ҹ�
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
