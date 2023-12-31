using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    private Vector3 originPos;//���� ��ġ
    private Vector3 currentPos;//���� ��ġ
    [SerializeField]
    private Vector3 limitPos;//�ִ� ��鸲
    [SerializeField]
    private Vector3 fineSightLimitPos;//������ �� �ִ� ��鸲
    [SerializeField]
    private Vector3 smoothSway;//�ε巯�� ������ ����
    [SerializeField]
    private GunController gunController;
    // Start is called before the first frame update
    void Start()
    {
        originPos = this.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        TrySway();
    }

    void TrySway()
    {
        if(Input.GetAxisRaw("Mouse X")!=0 || Input.GetAxisRaw("Mouse Y") != 0)//���콺�� �����̰� ���� ��
        {
            Swaying();
        }
        else
        {
            BackToOriginPos();
        }
    }

    void Swaying()
    {
        float _moveX = Input.GetAxisRaw("Mouse X");
        float _moveY = Input.GetAxisRaw("Mouse Y");
        if (!gunController.GetFineSightMode())//������ ���� Ȯ��
        {//���콺 �̵� ����
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.x), -limitPos.x, limitPos.x), Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.x), -limitPos.y, limitPos.y), originPos.z);
        }
        else
        {
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.y), -fineSightLimitPos.x, fineSightLimitPos.x), Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.y), -fineSightLimitPos.y, fineSightLimitPos.y), originPos.z);
            
        }
        transform.localPosition = currentPos;//��鸲 ����
    }
    void BackToOriginPos()
    {
        currentPos = Vector3.Lerp(currentPos, originPos, smoothSway.x);
        transform.localPosition = currentPos;
    }
}
