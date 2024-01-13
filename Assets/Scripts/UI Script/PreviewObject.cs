using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    //충돌한 오브젝트의 콜라이더
    private List<Collider> colliderList = new List<Collider>();
    [SerializeField]
    private int layerGround; // 지상 레이어
    private const int IGNORE_RAYCAST_LAYER = 2;//레이어 무시 기본값

    //각 머티리얼
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
            colliderList.Add(other);//무시해야 할 레이어가 아닐 경우 추가
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
        {
            colliderList.Remove(other);//무시해야 할 레이어가 아닐 경우 추가
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
        foreach(Transform tf_Child in this.transform)//모든 개체를 가져와서 반복 구동
        {
            var newMaterials = new Material[tf_Child.GetComponent<Renderer>().materials.Length];//머티리얼 리스트

            for(int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = mat;
            }

            tf_Child.GetComponent<Renderer>().materials = newMaterials;//머티리얼 변경 후 삽입
        }
    }

   

    public bool isBuildable()
    {
        
        return colliderList.Count == 0;
    }
}
