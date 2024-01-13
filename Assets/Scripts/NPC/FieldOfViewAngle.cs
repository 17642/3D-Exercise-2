using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewAngle : MonoBehaviour
{
    [SerializeField]
    private float viewAngle;
    [SerializeField]
    private float viewDistance;
    [SerializeField]
    private LayerMask layerMask;//타겟 마스크.(볼 대상 지정)

    private Pig pig;

    private void Start()
    {
        pig = gameObject.GetComponent<Pig>();
    }
    // Update is called once per frame
    void Update()
    {
        View();
    }

    private Vector3 BoundaryAngle(float _angle)
    {
        _angle += transform.eulerAngles.y;//Angle의 y값 추가;
        return new Vector3(Mathf.Sin(_angle*Mathf.Deg2Rad), 0, Mathf.Cos(_angle * Mathf.Deg2Rad));//Sin,Cos를 이용해 각도를 벡터로 지정
    }
    private void View()
    {
        Vector3 _leftBoundary = BoundaryAngle(-viewAngle*0.5f);
        Vector3 _rightBoundary = BoundaryAngle(viewAngle * 0.5f);

        Debug.DrawRay(transform.up + transform.position,_leftBoundary*viewDistance, Color.yellow);
        Debug.DrawRay(transform.up + transform.position,_rightBoundary*viewDistance, Color.yellow);//디버그를 위한 시야각 표시

        Collider[] _target = Physics.OverlapSphere(transform.position, viewDistance, layerMask);//특정 거리 내의 모든(레이어에 맞는) 물체 탐색

        for(int i =0; i < _target.Length; i++)
        {
            Transform _targetTf = _target[i].transform;
            if(_targetTf.name == "Player")
            {
                Vector3 _direction = (_targetTf.position - transform.position).normalized;//방향 설정
                float _angle = Vector3.Angle(_direction, transform.forward);//전방 각도 및 플레이어 비교 각도 파악

                if(_angle < viewAngle * 0.5f)//플레이어와 개체 시야의 각도가 시야각보다 작을 경우
                {
                    RaycastHit _hit;
                    if(Physics.Raycast(transform.position+transform.up, _direction, out _hit,viewDistance))
                    {
                        if (_hit.transform.name == "Player")
                        {
                            Debug.Log("플레이어가 돼지 시야 내에 있습니다.");
                            Debug.DrawRay(transform.position + transform.up, _direction, Color.blue);
                            pig.Run(_hit.transform.position);//타겟 위치로 도주
                        }
                       }
                }
            }
        }
    }
}

