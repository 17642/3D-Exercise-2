using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public string handName;//무기 이름(너클/맨손 구분)
    public float range;
    public int damage;
    public float workSpeed;//작업 속도
    public float attackDelay;//공격 딜레이
    public float attackDelayA;//공격 활성 시점(선딜)
    public float attackDelayB;//공격 비활성 시점(후딜)

    //Start와 Update 등 함수는 존재 자체로 자원을 소모한다.

    public Animator anim;//애니메이션


}
