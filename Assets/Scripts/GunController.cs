using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public static bool isActivate = true;//활성화 여부
    [SerializeField]
    private LayerMask layermask;
    [SerializeField]
    private Gun currentGun;
    [SerializeField]
    private Vector3 OriginPos;//원래 위치
    private Crosshair crosshair;
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private GameObject hit_effect_prefab;

    private RaycastHit hitinfo;

    private float currentFireRate;//발사 쿨다운 타이머. currentGun의 FireRate를 이용해 계산

    private AudioSource audioSource;

    private bool isReload = false;
    private bool isFineSightMod = false;//정조준 모드
    // Start is called before the first frame update

    private void Start()
    {
        crosshair=FindObjectOfType<Crosshair>();
        audioSource = GetComponent<AudioSource>();
        OriginPos = Vector3.zero;
        //OriginPos = transform.localPosition;

        WeaponManager.currentWeapon = currentGun.GetComponent<Transform>();
        WeaponManager.currentWeaponAnimator = currentGun.anim;
    }
    // Update is called once per frame
    void Update()
    {
        if (isActivate)
        {
            GunFirerateCalculate();
            if (!Inventory.inventoryActivated)
            {
                TryFire();
                TryReload();
                TryFineSIght();
            }
        }
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

    void Fire()//발사 전
    {
        if (!isReload)
        {
            if (currentGun.currentBulletCount > 0)
            {
                Shoot();//총알 발사
            }
            else
            {
                CancelFineSight();
                StartCoroutine(ReloadCoroutine());
            }
        }
    }

    private void Hit()
    {
        if(Physics.Raycast(cam.transform.position,cam.transform.forward+new Vector3(Random.Range(-crosshair.GetAccuracy()-currentGun.accuracy,+crosshair.GetAccuracy()+currentGun.accuracy), Random.Range(-crosshair.GetAccuracy() - currentGun.accuracy, +crosshair.GetAccuracy() + currentGun.accuracy),0),out hitinfo, currentGun.range,layermask))
        {
            GameObject clone = Instantiate(hit_effect_prefab, hitinfo.point, Quaternion.LookRotation(hitinfo.normal));
            Destroy(clone, 2f);
        }
    }

    void Shoot()//발사 후
    {
        Hit();
        crosshair.FIreAnimation();
        currentGun.currentBulletCount--;
        playSE(currentGun.FireSound);
        currentGun.muzzleFlash.Play();//이펙트 재생
        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine());
        Debug.Log("발사함!");
        currentFireRate = currentGun.fireRate;//연사 타이머 초기화
    }

    void playSE(AudioClip _clip)//클립 재생
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
    void Reload()//코루틴을 사용하지 않는 재장전 함수
    {
        if (currentGun.carryBulletCount > 0)
        {
            currentGun.anim.SetTrigger("Reload");
            if (currentGun.carryBulletCount > currentGun.reloadBullet) // 가진 총알이 충분하면
            {
                currentGun.currentBulletCount = currentGun.reloadBullet; // 장탄 수만큼 장전
                currentGun.carryBulletCount -= currentGun.reloadBullet;
            }
            else
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount; // 아닐 경우 소지한 모든 총알 장전
                currentGun.carryBulletCount = 0;
            }
        }
    }

    IEnumerator ReloadCoroutine()
    {
        if (currentGun.carryBulletCount > 0)
        {
            isReload = true;//재장전 시작
            currentGun.anim.SetTrigger("Reload");

            //수동 재장전을 위한 코드 -> 가진 총알을 가방에 넣는다.
            currentGun.carryBulletCount += currentGun.currentBulletCount;
            currentGun.currentBulletCount = 0;

            yield return new WaitForSeconds(currentGun.reloadTime);

            if (currentGun.carryBulletCount > currentGun.reloadBullet) // 가진 총알이 충분하면
            {
                currentGun.currentBulletCount = currentGun.reloadBullet; // 장탄 수만큼 장전
                currentGun.carryBulletCount -= currentGun.reloadBullet;
            }
            else
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount; // 아닐 경우 소지한 모든 총알 장전
                currentGun.carryBulletCount = 0;
            }
            isReload = false;//재장전 끝
        }
        else
        {
            Debug.Log("소유한 총알이 없습니다.");
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

    void TryFineSIght()//정조준 시도
    {
        if (Input.GetButtonDown("Fire2")&& !isReload)
        {
            FineSight();//정조준
        }
    }

    void FineSight()
    {
        isFineSightMod = !isFineSightMod;
        currentGun.anim.SetBool("FineSight", isFineSightMod);
        crosshair.FineSightAnimation(isFineSightMod);

        if (isFineSightMod)
        {
            StopAllCoroutines();//코루틴 모두 정지
            StartCoroutine(FineSightActivateCoroutine());
        }
        else
        {
            StopAllCoroutines();//코루틴 모두 정지
            StartCoroutine(FineSightDeActivateCoroutine());
        }
    }

    IEnumerator FineSightActivateCoroutine()
    {
        while(currentGun.transform.localPosition != currentGun.fineSightOriginPos)//화면 중심으로 총이 올 때 까지
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.2f);
            yield return null;//1프레임 대기
        }
    }

    IEnumerator FineSightDeActivateCoroutine()
    {
        while (currentGun.transform.localPosition != OriginPos)//화면 중심으로 총이 올 때 까지
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, OriginPos, 0.2f);
            yield return null;//1프레임 대기
        }
    }

    IEnumerator RetroActionCoroutine()
    {
        //정조준 시 반동 -> -1이 더해져야 정상적인 값이 나오는데 이유를 모르겠음
        //좌우 반동을 위해서는 x축을 사용해야 함(90도 회전해 있으므로)
        Vector3 recoilBack=new Vector3(currentGun.retroActionForce,OriginPos.y,OriginPos.z);//일반 최대 반동
        Vector3 retroActionRecoilBack = new Vector3(currentGun.retroActionFineSightForce, currentGun.fineSightOriginPos.y, currentGun.fineSightOriginPos.z);//정조준 - 최대 반동

        if (!isFineSightMod)//정조준 아님
        {
            currentGun.transform.localPosition = OriginPos;//처음 위치로 이동

            //반동 시작
            while (currentGun.transform.localPosition.x <= currentGun.retroActionForce-0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
                yield return null;
            }

            //원위치
            while(currentGun.transform.localPosition != OriginPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, OriginPos, 0.1f);
                yield return null;
            }
        }
        else
        {
            currentGun.transform.localPosition = currentGun.fineSightOriginPos;//처음(조준) 위치로 이동

            //반동 시작
            while (currentGun.transform.localPosition.x <= currentGun.retroActionFineSightForce - 0.02f)//일정 범위까지 퍼짐
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, 0.4f);
                yield return null;
            }

            //원위치
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

    public Gun GetGun() //private 멤버 Gun 반환
    {
        return currentGun;
    }

    public bool GetFineSightMode()
    {
        return isFineSightMod;
    }

    public void CancelReload()
    {
        if (isReload)
        {
            StopAllCoroutines();
            isReload = false;
        }
    }

    public void GunChange(Gun gun)
    {
        if(WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);//기존에 장착한 무기를 비활성화
        }
        currentGun = gun;//무기 변경
        WeaponManager.currentWeapon = currentGun.GetComponent<Transform>();//현재 무기의 transform을 지정
        WeaponManager.currentWeaponAnimator= currentGun.anim;//애니메이션 가져오기

        currentGun.transform.localPosition = Vector3.zero;//총 위치 초기화
        currentGun.gameObject.SetActive(true);//무기 활성화
        isActivate = true;
    }
}
