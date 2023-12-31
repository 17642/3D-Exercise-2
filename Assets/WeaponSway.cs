using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    private Vector3 originPos;//기존 위치
    private Vector3 currentPos;//현재 위치
    [SerializeField]
    private Vector3 limitPos;//최대 흔들림
    [SerializeField]
    private Vector3 fineSightLimitPos;//정조준 시 최대 흔들림
    [SerializeField]
    private Vector3 smoothSway;//부드러운 움직임 정도
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
        if(Input.GetAxisRaw("Mouse X")!=0 || Input.GetAxisRaw("Mouse Y") != 0)//마우스가 움직이고 있으 때
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
        if (!gunController.GetFineSightMode())//정조준 여부 확인
        {//마우스 이동 대입
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.x), -limitPos.x, limitPos.x), Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.x), -limitPos.y, limitPos.y), originPos.z);
        }
        else
        {
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.y), -fineSightLimitPos.x, fineSightLimitPos.x), Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.y), -fineSightLimitPos.y, fineSightLimitPos.y), originPos.z);
            
        }
        transform.localPosition = currentPos;//흔들림 적용
    }
    void BackToOriginPos()
    {
        currentPos = Vector3.Lerp(currentPos, originPos, smoothSway.x);
        transform.localPosition = currentPos;
    }
}
