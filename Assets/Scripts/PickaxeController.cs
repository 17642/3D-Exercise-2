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
                if(hitInfo.transform.tag == "Rock")//������ ����� ���
                {
                    hitInfo.transform.GetComponent<Rock>().Mining();//Rock ��ũ��Ʈ �����ͼ� �Լ� ����
                }else if(hitInfo.transform.tag == "WeakAnimal")
                {
                    SoundManager.instance.PlaySE("Animal_Hit");
                    hitInfo.transform.GetComponent<WeakAnimal>().Damage(1, transform.position);//�ӽ÷� ����
                }else if(hitInfo.transform.tag == "StrongAnimal")
                {
                    SoundManager.instance.PlaySE("Animal_Hit");
                    //hitInfo.transform.GetComponent<StrongAnimal>().Damage(1, transform.position);//�ӽ÷� ����
                }
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
