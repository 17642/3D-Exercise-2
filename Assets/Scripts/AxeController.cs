using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeController : CloseWeaponController
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
