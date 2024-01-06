using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IDropHandler, IEndDragHandler
{
    public Item item;//������
    public int itemCount;//������ ��
    public Image itemImage;
    //private Vector3 originPos;//������ ����

    [SerializeField]
    private Text text_Count;
    [SerializeField]
    private GameObject go_CountImage;
    private WeaponManager weaponManager;
    // Start is called before the first frame update
    private void Start()
    {
        weaponManager = FindObjectOfType<WeaponManager>();//�������� ��� SerializeField�� ����ϸ� �������� ������ �����ؾ� �� �� ����.
    }
    public  void AddItem(Item _item, int _count = 1)//���� �⺻�� 1
    {
        //originPos = transform.position;

        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;

        if (item.itemType != Item.ItemType.Equipment)
        {
            go_CountImage.SetActive(true);
            text_Count.text = itemCount.ToString();
        }
        else
        {
            text_Count.text = "0";//�ؽ�Ʈ �ʱ�ȭ
            go_CountImage.SetActive(false);//��Ȱ��ȭ�� ���¿��� ������ ���� �� ���� �߻�
            
        }

        SetColor(1);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
        if (eventData.button == PointerEventData.InputButton.Right)//���콺 ��Ŭ���ϸ�
        {
            if (item != null)
            {
                if(item.itemType == Item.ItemType.Equipment)//�������� Ÿ���� ����� ���
                {//����
                    StartCoroutine(weaponManager.ChangeWeaponCoroutine(item.weaponType, item.itemName));
                    //"GUN", subMachineGun
                }
                else
                {//�Ҹ�
                    Debug.Log(item.itemName + " �� �����");
                    SetSlotCount(-1);//���� ���� ����
                }
            }
        }
    }

    public void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString();//�ؽ�Ʈ�� ������ ���� �� ����

        if (itemCount <= 0)
        {
            ClearSlot();
        }
    }

    private void ClearSlot()
    {
        //���� �ʱ�ȭ
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);
        text_Count.text = "0";
        go_CountImage.SetActive(false);
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.dragSlot = this;//DragSlot�� �ڽ��� �ٿ�����
            DragSlot.instance.DragSetImage(itemImage);//�̹��� ����

            DragSlot.instance.transform.position = eventData.position; //DragSlot ��ġ ����(���콺 ��ġ��
            //transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null)//dragSlot�� ���� ���� ���
        {
            ChangeSlot();
        }
        Debug.Log("ONDROP ȣ��");
           
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("ONENDDRAG ȣ��");
        DragSlot.instance.dragSlot = null;
        DragSlot.instance.SetColor(0);
    }

    private void ChangeSlot()
    {
        Item _tempItem = item;
        int _tempItemCount = itemCount;

        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);//DragSlot�� �������� �ڽ����� �ٿ�����
    
        if(_tempItem != null)
        {
            DragSlot.instance.dragSlot.AddItem(_tempItem, _tempItemCount);//tempItem�� �����ϴ� ��� DragSlot�� �������� �ٿ�����
        }
        else//�� ������ ���
        {
            DragSlot.instance.dragSlot.ClearSlot();//DragSlot �ʱ�ȭ
        }
    }
}
