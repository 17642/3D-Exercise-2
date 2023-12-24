using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
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
        TryAttack();
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
}
