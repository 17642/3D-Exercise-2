using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWeapon : MonoBehaviour
{
    public string closeWeaponName;//���� �̸�(��Ŭ/�Ǽ� ����)

    //���� ����
    public bool isHand;
    public bool isAxe;
    public bool isPickaxe;

    //���� ����
    public float range;
    public int damage;
    public float workSpeed;//�۾� �ӵ�
    public float attackDelay;//���� ������
    public float attackDelayA;//���� Ȱ�� ����(����)
    public float attackDelayB;//���� ��Ȱ�� ����(�ĵ�)

    //Start�� Update �� �Լ��� ���� ��ü�� �ڿ��� �Ҹ��Ѵ�.

    public Animator anim;//�ִϸ��̼�


}
