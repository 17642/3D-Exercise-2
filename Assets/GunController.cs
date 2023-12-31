using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Gun currentGun;

    private float currentFireRate;//발사 쿨다운 타이머. currentGun의 FireRate를 이용해 계산

    private AudioSource audioSource;
    // Start is called before the first frame update

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        GunFirerateCalculate();
        TryFire();
    }

    void GunFirerateCalculate()
    {
        if(currentFireRate > 0) {
            currentFireRate -= Time.deltaTime; //지속적 감소
        }
    }

    void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0)//연사속도 계산이 끝나면
        {
            Fire();
        }
    }

    void Fire()
    {
        currentFireRate = currentGun.fireRate;//연사 타이머 초기화
        Shoot();//총알 발사
    }

    void Shoot()
    {
        playSE(currentGun.FireSound);
        currentGun.muzzleFlash.Play();//이펙트 재생
        Debug.Log("발사함!");
    }

    void playSE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
}
