using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IDropHandler, IEndDragHandler
{
    public Item item;//아이템
    public int itemCount;//아이템 수
    public Image itemImage;
    //private Vector3 originPos;//사용되지 않음

    [SerializeField]
    private Text text_Count;
    [SerializeField]
    private GameObject go_CountImage;
    private WeaponManager weaponManager;
    // Start is called before the first frame update
    private void Start()
    {
        weaponManager = FindObjectOfType<WeaponManager>();//프리팹의 경우 SerializeField를 사용하면 여러개에 일일히 지정해야 할 수 있음.
    }
    public  void AddItem(Item _item, int _count = 1)//개수 기본값 1
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
            text_Count.text = "0";//텍스트 초기화
            go_CountImage.SetActive(false);//비활성화된 상태에서 변동이 있을 시 오류 발생
            
        }

        SetColor(1);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
        if (eventData.button == PointerEventData.InputButton.Right)//마우스 우클릭하면
        {
            if (item != null)
            {
                if(item.itemType == Item.ItemType.Equipment)//아이템의 타입이 장비인 경우
                {//장착
                    StartCoroutine(weaponManager.ChangeWeaponCoroutine(item.weaponType, item.itemName));
                    //"GUN", subMachineGun
                }
                else
                {//소모
                    Debug.Log(item.itemName + " 을 사용함");
                    SetSlotCount(-1);//물건 수량 감소
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
        text_Count.text = itemCount.ToString();//텍스트엔 정수를 넣을 수 없음

        if (itemCount <= 0)
        {
            ClearSlot();
        }
    }

    private void ClearSlot()
    {
        //슬롯 초기화
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
            DragSlot.instance.dragSlot = this;//DragSlot에 자신을 붙여넣음
            DragSlot.instance.DragSetImage(itemImage);//이미지 지정

            DragSlot.instance.transform.position = eventData.position; //DragSlot 위치 변경(마우스 위치로
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
        if (DragSlot.instance.dragSlot != null)//dragSlot이 비지 않은 경우
        {
            ChangeSlot();
        }
        Debug.Log("ONDROP 호출");
           
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("ONENDDRAG 호출");
        DragSlot.instance.dragSlot = null;
        DragSlot.instance.SetColor(0);
    }

    private void ChangeSlot()
    {
        Item _tempItem = item;
        int _tempItemCount = itemCount;

        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);//DragSlot의 아이템을 자신으로 붙여넣음
    
        if(_tempItem != null)
        {
            DragSlot.instance.dragSlot.AddItem(_tempItem, _tempItemCount);//tempItem이 존재하는 경우 DragSlot에 아이템을 붙여넣음
        }
        else//빈 슬롯인 경우
        {
            DragSlot.instance.dragSlot.ClearSlot();//DragSlot 초기화
        }
    }
}
