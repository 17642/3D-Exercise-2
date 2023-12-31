using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(GunController))]//요구하는 컴포넌트(이 컴포넌트 없으면 오류 출려걔
public class WeaponManager : MonoBehaviour
{
    public static bool isChangeWeapon = false;//클래스 전체가 공유함. -> 클래스에서 접근 가능

    [SerializeField]
    private float changeWeaponDelayTime;//무기 교체 딜레이
    [SerializeField]
    private float changeWeaponEndDelayTime;//무기 교체가 끝나는 시점

    //종류별 무기들
    [SerializeField]
    private Gun[] guns;
    [SerializeField]
    private Hand[] hands;

    //무기 딕셔너리
    private Dictionary<string,Gun> gunDictionary = new Dictionary<string,Gun>();
    private Dictionary<string,Hand> handDictionary = new Dictionary<string,Hand>();

    [SerializeField]
    private string currentWeaponType;//현재 무기 타입

    public static Transform currentWeapon;//현재 무기(공용)
    public static Animator currentWeaponAnimator;//현재 무기 애니메이션

    [SerializeField] // 각 컨트롤러
    private GunController gunController;
    [SerializeField]
    private HandController handController;
    // Start is called before the first frame update
    //[SerializeField]
    private void Start()
    {
        //딕셔너리 지정
        for (int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName, guns[i]);
        }
        for(int i=0; i < hands.Length; i++)
        {
            handDictionary.Add(hands[i].handName, hands[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isChangeWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))//숫자 1을 눌렀을 때
            {
                //무기 교체 실행(손)
                StartCoroutine(ChangeWeaponCoroutine("HAND", "맨손"));
            }else if (Input.GetKeyDown(KeyCode.Alpha2))
            {//총
                StartCoroutine(ChangeWeaponCoroutine("GUN", "SubMachineGun1"));
            }
        }
    }
    public IEnumerator ChangeWeaponCoroutine(string type, string name)//무기 타입 -> 바꿀 무기 이름
    {
            isChangeWeapon = true;
            currentWeaponAnimator.SetTrigger("WeaponOut");

            yield return new WaitForSeconds(changeWeaponDelayTime);

            CancelPreWeaponAction();
        WeaponChange(type,name);
        yield return new WaitForSeconds(changeWeaponEndDelayTime);
        currentWeaponType = type;
        isChangeWeapon = false;
        
    }

    void CancelPreWeaponAction()
        {
            switch (currentWeaponType)
            {
                case "GUN":
                    gunController.CancelFineSight();
                    gunController.CancelReload();
                GunController.isActivate = false;//무기 교체 시 동작 비활성화
                    break;
                case "HAND":
                HandController.isActivate = false;
                    break;
            }
        }
    void WeaponChange(string _type, string _name)
    {
        if (_type == "GUN")
        {
            gunController.GunChange(gunDictionary[_name]);
        }
        else if (_type == "HAND") 
        {
            handController.HandChange(handDictionary[_name]);
        }
    }
}
