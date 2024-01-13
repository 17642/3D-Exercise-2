using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    //�浹�� ������Ʈ�� �ݶ��̴�
    private List<Collider> colliderList = new List<Collider>();
    [SerializeField]
    private int layerGround; // ���� ���̾�
    private const int IGNORE_RAYCAST_LAYER = 2;//���̾� ���� �⺻��

    //�� ��Ƽ����
    [SerializeField]
    private Material green;
    [SerializeField]
    private Material red;

    void Update()
    {
        //colliderList.RemoveRange(0, colliderList.Count);
        ChangeColor();
        //Debug.Log(colliderList.Count);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
        {
            colliderList.Add(other);//�����ؾ� �� ���̾ �ƴ� ��� �߰�
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
        {
            colliderList.Remove(other);//�����ؾ� �� ���̾ �ƴ� ��� �߰�
        }
    }

    private void ChangeColor()
    {
        if (colliderList.Count > 0)
        {
            SetColor(red);
        }
        else
        {
            SetColor(green);
        }
    }

    private void SetColor(Material mat)
    {
        foreach(Transform tf_Child in this.transform)//��� ��ü�� �����ͼ� �ݺ� ����
        {
            var newMaterials = new Material[tf_Child.GetComponent<Renderer>().materials.Length];//��Ƽ���� ����Ʈ

            for(int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = mat;
            }

            tf_Child.GetComponent<Renderer>().materials = newMaterials;//��Ƽ���� ���� �� ����
        }
    }

   

    public bool isBuildable()
    {
        
        return colliderList.Count == 0;
    }
}
