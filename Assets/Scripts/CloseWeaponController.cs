using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CloseWeaponController : MonoBehaviour//�߻� Ŭ����(���� �� Ŭ������ ���� �� ����) -> ������Ʈ�� �� �� ���� -> Update�� ������ �� ����. 
{
    
    //���� ���� Hand Ÿ�� ����
    [SerializeField]
    CloseWeapon currentCloseWeapon;

    //����
    protected bool isAttack = false;
    protected bool isSwing = false;

    protected RaycastHit hitInfo;//����ĳ��Ʈ ������

    // Update is called once per frame


    protected void TryAttack()
    {
        if (!Inventory.inventoryActivated)
        {
            if (Input.GetButton("Fire1"))
            {
                if (!isAttack)
                {
                    StartCoroutine(AttackCoroutine());
                }
            }
        }
    }

    protected IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currentCloseWeapon.anim.SetTrigger("Attack");//Attack Ʈ���� �ߵ�

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayA);//����
        isSwing = true;
        //���� Ȱ��ȭ
        StartCoroutine(HitCoroutine());
        yield return new WaitForSeconds(currentCloseWeapon.attackDelayB);//�ĵ�
        isSwing = false;

        yield return new WaitForSeconds(currentCloseWeapon.attackDelay - (currentCloseWeapon.attackDelayA + currentCloseWeapon.attackDelayB));//���ݼӵ�-���ð�
        isAttack = false;
    }
    protected abstract IEnumerator HitCoroutine(); // �ڽ� Ŭ�������� ��� �ϼ��� ����
    

    protected bool CheckObject()//������ True
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentCloseWeapon.range))//range �ȿ� Ray�� ����� ��� hitInfo�� ����
        {
            return true;
        }
        return false;
    }

    public virtual void CloseWeaponChange(CloseWeapon hand) // ���� �Լ�(�ڽ��� �߰� ���� ����)
    {
        if (WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);//������ ������ ���⸦ ��Ȱ��ȭ
        }
        currentCloseWeapon = hand;//���� ����
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();//���� ������ transform�� ����
        WeaponManager.currentWeaponAnimator = currentCloseWeapon.anim;//�ִϸ��̼� ��������

        currentCloseWeapon.transform.localPosition = Vector3.zero;//��  ��ġ �ʱ�ȭ
        currentCloseWeapon.gameObject.SetActive(true);//���� Ȱ��ȭ
        //isActivate = true;

    }
}
