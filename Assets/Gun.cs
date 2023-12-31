using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // Start is called before the first frame update
    public string gunName;
    public float range;//사거리
    public float accuracy;//정확도
    public float fireRate;//연사력
    public float reloadTime;//재장전 속도

    public int damage;//데미지

    public int reloadBullet;//탄창 당 탄 수
    public int currentBulletCount;//탄창 내 총알 수
    public int maxBulletCount;//최대 총알 수
    public int carryBulletCount;//소유하는 총알의 수

    public float retroActionForce;//반동 세기
    public float retroActionFineSightForce;//정조준 반동 세기

    public Vector3 fineSightOriginPos; // 반동 돌아올 위치

    public Animator anim; // 총 애니메이터

    public ParticleSystem muzzleFlash;//총구화염

    public AudioClip FireSound;//발사 소리
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
