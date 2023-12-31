using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GunController gunController;
    private Gun currentGun;

    [SerializeField]
    private GameObject go_BulletHUD; // �ʿ� �� HUD ��Ȱ��ȭ

    [SerializeField]
    private Text[] text_Bullet;//Text ���<�Ѿ� �� �ݿ� TEXT

    // Update is called once per frame
    void Update()
    {
        CheckBullet();
    }

    void CheckBullet()//�Ѿ� ���� ��������
    {
        currentGun = gunController.GetGun();
        text_Bullet[0].text = currentGun.carryBulletCount.ToString();
        text_Bullet[1].text = currentGun.reloadBullet.ToString();
        text_Bullet[2].text = currentGun.currentBulletCount.ToString();
    }
}
