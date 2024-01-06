using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item item;//아이템
    public int itemCount;//아이템 수
    public Image itemImage;

    [SerializeField]
    private Text text_Count;
    [SerializeField]
    private GameObject go_CountImage;
    // Start is called before the first frame update
    public  void AddItem(Item _item, int _count = 1)//개수 기본값 1
    {
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
}
