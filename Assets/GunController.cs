using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Gun currentGun;
    [SerializeField]
    private Vector3 OriginPos;//���� ��ġ

    private float currentFireRate;//�߻� ��ٿ� Ÿ�̸�. currentGun�� FireRate�� �̿��� ���

    private AudioSource audioSource;

    private bool isReload = false;
    private bool isFineSightMod = false;//������ ���
    // Start is called before the first frame update

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //OriginPos = transform.localPosition;
    }
    // Update is called once per frame
    void Update()
    {
        GunFirerateCalculate();
        TryFire();
        TryReload();
        TryFineSIght();
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

    void Fire()//�߻� ��
    {
        if (!isReload)
        {
            {

            }
            if (currentGun.currentBulletCount > 0)
            {
                Shoot();//�Ѿ� �߻�
            }
            else
            {
                CancelFineSight();
                StartCoroutine(ReloadCoroutine());
            }
        }
    }

    void Shoot()//�߻� ��
    {
        currentGun.currentBulletCount--;
        playSE(currentGun.FireSound);
        currentGun.muzzleFlash.Play();//����Ʈ ���
        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine());
        Debug.Log("�߻���!");
        currentFireRate = currentGun.fireRate;//���� Ÿ�̸� �ʱ�ȭ
    }

    void playSE(AudioClip _clip)//Ŭ�� ���
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
    void Reload()//�ڷ�ƾ�� ������� �ʴ� ������ �Լ�
    {
        if (currentGun.carryBulletCount > 0)
        {
            currentGun.anim.SetTrigger("Reload");
            if (currentGun.carryBulletCount > currentGun.reloadBullet) // ���� �Ѿ��� ����ϸ�
            {
                currentGun.currentBulletCount = currentGun.reloadBullet; // ��ź ����ŭ ����
                currentGun.carryBulletCount -= currentGun.reloadBullet;
            }
            else
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount; // �ƴ� ��� ������ ��� �Ѿ� ����
                currentGun.carryBulletCount = 0;
            }
        }
    }

    IEnumerator ReloadCoroutine()
    {
        if (currentGun.carryBulletCount > 0)
        {
            isReload = true;//������ ����
            currentGun.anim.SetTrigger("Reload");

            //���� �������� ���� �ڵ� -> ���� �Ѿ��� ���濡 �ִ´�.
            currentGun.carryBulletCount += currentGun.currentBulletCount;
            currentGun.currentBulletCount = 0;

            yield return new WaitForSeconds(currentGun.reloadTime);

            if (currentGun.carryBulletCount > currentGun.reloadBullet) // ���� �Ѿ��� ����ϸ�
            {
                currentGun.currentBulletCount = currentGun.reloadBullet; // ��ź ����ŭ ����
                currentGun.carryBulletCount -= currentGun.reloadBullet;
            }
            else
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount; // �ƴ� ��� ������ ��� �Ѿ� ����
                currentGun.carryBulletCount = 0;
            }
            isReload = false;//������ ��
        }
        else
        {
            Debug.Log("������ �Ѿ��� �����ϴ�.");
        }
    }

    void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBullet)
        {
            CancelFineSight();
            StartCoroutine(ReloadCoroutine());
        }
    }

    void TryFineSIght()//������ �õ�
    {
        if (Input.GetButtonDown("Fire2")&& !isReload)
        {
            FineSight();//������
        }
    }

    void FineSight()
    {
        isFineSightMod = !isFineSightMod;
        currentGun.anim.SetBool("FineSight", isFineSightMod);

        if (isFineSightMod)
        {
            StopAllCoroutines();//�ڷ�ƾ ��� ����
            StartCoroutine(FineSightActivateCoroutine());
        }
        else
        {
            StopAllCoroutines();//�ڷ�ƾ ��� ����
            StartCoroutine(FineSightDeActivateCoroutine());
        }
    }

    IEnumerator FineSightActivateCoroutine()
    {
        while(currentGun.transform.localPosition != currentGun.fineSightOriginPos)//ȭ�� �߽����� ���� �� �� ����
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.2f);
            yield return null;//1������ ���
        }
    }

    IEnumerator FineSightDeActivateCoroutine()
    {
        while (currentGun.transform.localPosition != OriginPos)//ȭ�� �߽����� ���� �� �� ����
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, OriginPos, 0.2f);
            yield return null;//1������ ���
        }
    }

    IEnumerator RetroActionCoroutine()
    {
        //������ �� �ݵ� -> -1�� �������� �������� ���� �����µ� ������ �𸣰���
        //�¿� �ݵ��� ���ؼ��� x���� ����ؾ� ��(90�� ȸ���� �����Ƿ�)
        Vector3 recoilBack=new Vector3(currentGun.retroActionForce,OriginPos.y,OriginPos.z);//�Ϲ� �ִ� �ݵ�
        Vector3 retroActionRecoilBack = new Vector3(currentGun.retroActionFineSightForce, currentGun.fineSightOriginPos.y, currentGun.fineSightOriginPos.z);//������ - �ִ� �ݵ�

        if (!isFineSightMod)//������ �ƴ�
        {
            currentGun.transform.localPosition = OriginPos;//ó�� ��ġ�� �̵�

            //�ݵ� ����
            while (currentGun.transform.localPosition.x <= currentGun.retroActionForce-0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
                yield return null;
            }

            //����ġ
            while(currentGun.transform.localPosition != OriginPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, OriginPos, 0.1f);
                yield return null;
            }
        }
        else
        {
            currentGun.transform.localPosition = currentGun.fineSightOriginPos;//ó��(����) ��ġ�� �̵�

            //�ݵ� ����
            while (currentGun.transform.localPosition.x <= currentGun.retroActionFineSightForce - 0.02f)//���� �������� ����
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, 0.4f);
                yield return null;
            }

            //����ġ
            while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.1f);
                yield return null;
            }
        }
    }

    public void CancelFineSight()
    {
        if (isFineSightMod)
        {
            FineSight();
        }
    }

    public Gun GetGun() //private ��� Gun ��ȯ
    {
        return currentGun;
    }
}