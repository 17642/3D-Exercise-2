using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.PackageManager;
using UnityEngine;

public class PickaxeController : CloseWeaponController
{
    public static bool isActivate = false;//활성화 여부
    protected void Update()
    {
        if (isActivate)
        {
            TryAttack();
        }
    }
    protected override IEnumerator HitCoroutine() // Override 함수
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                if(hitInfo.transform.tag == "Rock")//바위와 닿았을 경우
                {
                    hitInfo.transform.GetComponent<Rock>().Mining();//Rock 스크립트 가져와서 함수 실행
                }else if(hitInfo.transform.tag == "WeakAnimal")
                {
                    SoundManager.instance.PlaySE("Animal_Hit");
                    hitInfo.transform.GetComponent<WeakAnimal>().Damage(1, transform.position);//임시로 설정
                }else if(hitInfo.transform.tag == "StrongAnimal")
                {
                    SoundManager.instance.PlaySE("Animal_Hit");
                    //hitInfo.transform.GetComponent<StrongAnimal>().Damage(1, transform.position);//임시로 설정
                }
                isSwing = false;
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }
    public override void CloseWeaponChange(CloseWeapon hand)
    {
        base.CloseWeaponChange(hand);//부모 클래스의 함수를 호출
        isActivate = true;
    }
}
