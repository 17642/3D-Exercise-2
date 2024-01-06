using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    private float range;//���� ������ �ִ� �Ÿ� 

    private bool pickupActivated=false; //���� ���ɽ� Ȱ��ȭ

    private RaycastHit hitInfo;//�浹ü ���� ����

    [SerializeField]
    private LayerMask layerMask;//���̾� ����ũ(������ ���̾�� �����ϰ�)

    [SerializeField]
    private Text actionText;//������ �ؽ�Ʈ 
    [SerializeField]
    private Inventory inventory;

    void Update()
    {
        CheckItem();
        TryAction();
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckItem();
            CanPickUp();
        }
    }

    private void CanPickUp()
    {
        if (pickupActivated)
        {
            if (hitInfo.transform != null)
            {
                Debug.Log(hitInfo.transform.GetComponent<ItemPickup>().item.itemName + " ȹ�� ");
                inventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickup>().item, 1);//������ ����
                Destroy(hitInfo.transform.gameObject);
                InfoDisappear();
            }
        }
    }

    private void CheckItem()
    {
        if(Physics.Raycast(transform.position,transform.TransformDirection(Vector3.forward),out hitInfo,range,layerMask))
        {
            if(hitInfo.transform.tag == "Item")//��ü�� �±װ� item�϶���
            {
                ItemInfoAppear();
            }
        }
        else//�ε��� ��ü�� ���� ������
        {
            InfoDisappear();
        }
    }

    private void ItemInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickup>().item.itemName + " ȹ�� " + "<color=yellow>" + "(E)" + "</color>";

    }

    private void InfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }
}
