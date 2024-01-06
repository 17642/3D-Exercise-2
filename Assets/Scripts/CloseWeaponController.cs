using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CloseWeaponController : MonoBehaviour//추상 클래스(직접 이 클래스를 붙일 수 없음) -> 컴포넌트가 될 수 없음 -> Update를 실행할 수 없음. 
{
    
    //현재 장착 Hand 타입 무기
    [SerializeField]
    CloseWeapon currentCloseWeapon;

    //상태
    protected bool isAttack = false;
    protected bool isSwing = false;

    protected RaycastHit hitInfo;//레이캐스트 닿은것

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
        currentCloseWeapon.anim.SetTrigger("Attack");//Attack 트리거 발동

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayA);//선딜
        isSwing = true;
        //공격 활성화
        StartCoroutine(HitCoroutine());
        yield return new WaitForSeconds(currentCloseWeapon.attackDelayB);//후딜
        isSwing = false;

        yield return new WaitForSeconds(currentCloseWeapon.attackDelay - (currentCloseWeapon.attackDelayA + currentCloseWeapon.attackDelayB));//공격속도-대기시간
        isAttack = false;
    }
    protected abstract IEnumerator HitCoroutine(); // 자식 클래스에게 기능 완성을 위임
    

    protected bool CheckObject()//언제나 True
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentCloseWeapon.range))//range 안에 Ray가 닿았을 경우 hitInfo에 저장
        {
            return true;
        }
        return false;
    }

    public virtual void CloseWeaponChange(CloseWeapon hand) // 가상 함수(자식이 추가 편집 가능)
    {
        if (WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);//기존에 장착한 무기를 비활성화
        }
        currentCloseWeapon = hand;//무기 변경
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();//현재 무기의 transform을 지정
        WeaponManager.currentWeaponAnimator = currentCloseWeapon.anim;//애니메이션 가져오기

        currentCloseWeapon.transform.localPosition = Vector3.zero;//손  위치 초기화
        currentCloseWeapon.gameObject.SetActive(true);//무기 활성화
        //isActivate = true;

    }
}
