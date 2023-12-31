using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Gun currentGun;

    private float currentFireRate;//�߻� ��ٿ� Ÿ�̸�. currentGun�� FireRate�� �̿��� ���

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
            currentFireRate -= Time.deltaTime; //������ ����
        }
    }

    void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0)//����ӵ� ����� ������
        {
            Fire();
        }
    }

    void Fire()
    {
        currentFireRate = currentGun.fireRate;//���� Ÿ�̸� �ʱ�ȭ
        Shoot();//�Ѿ� �߻�
    }

    void Shoot()
    {
        playSE(currentGun.FireSound);
        currentGun.muzzleFlash.Play();//����Ʈ ���
        Debug.Log("�߻���!");
    }

    void playSE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
}
