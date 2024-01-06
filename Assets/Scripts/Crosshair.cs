using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Crosshair : MonoBehaviour
{
    [SerializeField]
    Animator animator;
    [SerializeField]
    private GunController gunController;

    private float gunAccuracy;//�� ��Ȯ��(ũ�ν��� ����)

    [SerializeField]
    private GameObject go_CrosshairHUD;//ũ�ν���� ���� Ȱ��ȭ �뵵

    public void WalkingAnimation(bool _flag)
    {
        WeaponManager.currentWeaponAnimator.SetBool("Walk", _flag);
        animator.SetBool("Walking", _flag);
    }
    public void RunningAnimation(bool _flag)
    {
        WeaponManager.currentWeaponAnimator.SetBool("Run", _flag);
        animator.SetBool("Running", _flag);
    }
    public void JumpAnimation(bool _flag)//�÷��̾� ����
    {
        
        animator.SetBool("Running", _flag);
    }

    public void CrouchingAniation(bool _flag)
    {
        animator.SetBool("Crouching", _flag);
    }

    public void FIreAnimation()
    {
        if (animator.GetBool("Walking"))
        {
            animator.SetTrigger("WalkFire");
        }
        else if (animator.GetBool("Crouching"))
        {
            animator.SetTrigger("CrouchFIre");
        }
        else
        {
            animator.SetTrigger("IdleFire");
        }
    }

    public void FineSightAnimation(bool _flag)
    {
        animator.SetBool("FineSight", _flag);
    }

    public float GetAccuracy()//��Ȯ�� ����
    {
        float gunAccuracy=0f;
        if (animator.GetBool("Walking"))
        {
            gunAccuracy = 0.08f;
        }
        else if (animator.GetBool("Crouching"))
        {
            gunAccuracy = 0.02f;
        }
        else if (gunController.GetFineSightMode())//�����ؽ�
        {
            gunAccuracy = 0.001f;
        }
        else
        {
            gunAccuracy = 0.04f;
        }

        return gunAccuracy;
    }
}
