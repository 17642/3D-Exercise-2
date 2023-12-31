using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{

    public static bool isActivate = false;//활성화 여부
    //현재 장착 Hand 타입 무기
    [SerializeField]
    Hand currentHand;

    //상태
    private bool isAttack = false;
    private bool isSwing = false;

    private RaycastHit hitInfo;//레이캐스트 닿은것

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
        currentHand.anim.SetTrigger("Attack");//Attack 트리거 발동

        yield return new WaitForSeconds(currentHand.attackDelayA);//선딜
        isSwing = true;
        //공격 활성화
        StartCoroutine(HitCoroutine());
        yield return new WaitForSeconds(currentHand.attackDelayB);//후딜
        isSwing = false;
        
        yield return new WaitForSeconds(currentHand.attackDelay-(currentHand.attackDelayA+currentHand.attackDelayB));//공격속도-대기시간
        isAttack = false;
    }
    IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                isSwing = false;//적중한 물체가 있을 경우 공격 판정 제거(여러번 공격 방지)
                Debug.Log(hitInfo.transform.name);//충돌한 물체의 이름 출력
            }
            yield return null;
        }
    }

    private bool CheckObject()//언제나 True
    {
        if(Physics.Raycast(transform.position,transform.forward,out hitInfo, currentHand.range))//range 안에 Ray가 닿았을 경우 hitInfo에 저장
        {
            return true;
        }
        return false;
    }

    public void HandChange(Hand hand)
    {
        if (WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);//기존에 장착한 무기를 비활성화
        }
        currentHand = hand;//무기 변경
        WeaponManager.currentWeapon = currentHand.GetComponent<Transform>();//현재 무기의 transform을 지정
        WeaponManager.currentWeaponAnimator = currentHand.anim;//애니메이션 가져오기

        currentHand.transform.localPosition = Vector3.zero;//손  위치 초기화
        currentHand.gameObject.SetActive(true);//무기 활성화
        isActivate = true;

    }
}
