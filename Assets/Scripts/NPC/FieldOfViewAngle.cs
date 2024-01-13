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
    private LayerMask layerMask;//Ÿ�� ����ũ.(�� ��� ����)

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
        _angle += transform.eulerAngles.y;//Angle�� y�� �߰�;
        return new Vector3(Mathf.Sin(_angle*Mathf.Deg2Rad), 0, Mathf.Cos(_angle * Mathf.Deg2Rad));//Sin,Cos�� �̿��� ������ ���ͷ� ����
    }
    private void View()
    {
        Vector3 _leftBoundary = BoundaryAngle(-viewAngle*0.5f);
        Vector3 _rightBoundary = BoundaryAngle(viewAngle * 0.5f);

        Debug.DrawRay(transform.up + transform.position,_leftBoundary*viewDistance, Color.yellow);
        Debug.DrawRay(transform.up + transform.position,_rightBoundary*viewDistance, Color.yellow);//����׸� ���� �þ߰� ǥ��

        Collider[] _target = Physics.OverlapSphere(transform.position, viewDistance, layerMask);//Ư�� �Ÿ� ���� ���(���̾ �´�) ��ü Ž��

        for(int i =0; i < _target.Length; i++)
        {
            Transform _targetTf = _target[i].transform;
            if(_targetTf.name == "Player")
            {
                Vector3 _direction = (_targetTf.position - transform.position).normalized;//���� ����
                float _angle = Vector3.Angle(_direction, transform.forward);//���� ���� �� �÷��̾� �� ���� �ľ�

                if(_angle < viewAngle * 0.5f)//�÷��̾�� ��ü �þ��� ������ �þ߰����� ���� ���
                {
                    RaycastHit _hit;
                    if(Physics.Raycast(transform.position+transform.up, _direction, out _hit,viewDistance))
                    {
                        if (_hit.transform.name == "Player")
                        {
                            Debug.Log("�÷��̾ ���� �þ� ���� �ֽ��ϴ�.");
                            Debug.DrawRay(transform.position + transform.up, _direction, Color.blue);
                            pig.Run(_hit.transform.position);//Ÿ�� ��ġ�� ����
                        }
                       }
                }
            }
        }
    }
}

