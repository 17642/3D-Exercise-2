using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.PackageManager;
using UnityEngine;

public class PickaxeController : CloseWeaponController
{
    public static bool isActivate = false;//Ȱ��ȭ ����
    protected void Update()
    {
        if (isActivate)
        {
            TryAttack();
        }
    }
    protected override IEnumerator HitCoroutine() // Override �Լ�
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
        base.CloseWeaponChange(hand);//�θ� Ŭ������ �Լ��� ȣ��
        isActivate = true;
    }
}
