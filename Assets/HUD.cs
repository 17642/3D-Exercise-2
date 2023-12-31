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
    private GameObject go_BulletHUD; // 필요 시 HUD 비활성화

    [SerializeField]
    private Text[] text_Bullet;//Text 목록<총알 수 반영 TEXT

    // Update is called once per frame
    void Update()
    {
        CheckBullet();
    }

    void CheckBullet()//총알 정보 가져오기
    {
        currentGun = gunController.GetGun();
        text_Bullet[0].text = currentGun.carryBulletCount.ToString();
        text_Bullet[1].text = currentGun.reloadBullet.ToString();
        text_Bullet[2].text = currentGun.currentBulletCount.ToString();
    }
}
