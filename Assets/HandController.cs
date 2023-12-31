using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{

    public static bool isActivate = false;//Ȱ��ȭ ����
    //���� ���� Hand Ÿ�� ����
    [SerializeField]
    Hand currentHand;

    //����
    private bool isAttack = false;
    private bool isSwing = false;

    private RaycastHit hitInfo;//����ĳ��Ʈ ������

    // Update is called once per frame
    void Update()
    {
        if (isActivate)
        {
            TryAttack();
        }
    }

    private void TryAttack()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!isAttack) {
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currentHand.anim.SetTrigger("Attack");//Attack Ʈ���� �ߵ�

        yield return new WaitForSeconds(currentHand.attackDelayA);//����
        isSwing = true;
        //���� Ȱ��ȭ
        StartCoroutine(HitCoroutine());
        yield return new WaitForSeconds(currentHand.attackDelayB);//�ĵ�
        isSwing = false;
        
        yield return new WaitForSeconds(currentHand.attackDelay-(currentHand.attackDelayA+currentHand.attackDelayB));//���ݼӵ�-���ð�
        isAttack = false;
    }
    IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                isSwing = false;//������ ��ü�� ���� ��� ���� ���� ����(������ ���� ����)
                Debug.Log(hitInfo.transform.name);//�浹�� ��ü�� �̸� ���
            }
            yield return null;
        }
    }

    private bool CheckObject()//������ True
    {
        if(Physics.Raycast(transform.position,transform.forward,out hitInfo, currentHand.range))//range �ȿ� Ray�� ����� ��� hitInfo�� ����
        {
            return true;
        }
        return false;
    }

    public void HandChange(Hand hand)
    {
        if (WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);//������ ������ ���⸦ ��Ȱ��ȭ
        }
        currentHand = hand;//���� ����
        WeaponManager.currentWeapon = currentHand.GetComponent<Transform>();//���� ������ transform�� ����
        WeaponManager.currentWeaponAnimator = currentHand.anim;//�ִϸ��̼� ��������

        currentHand.transform.localPosition = Vector3.zero;//��  ��ġ �ʱ�ȭ
        currentHand.gameObject.SetActive(true);//���� Ȱ��ȭ
        isActivate = true;

    }
}
