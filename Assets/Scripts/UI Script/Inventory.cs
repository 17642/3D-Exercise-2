using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false;//인벤토리 활성화 여부.

    //필요 컴포넌트
    [SerializeField]
    private GameObject go_InventoryBase;
    [SerializeField]
    private GameObject go_SlotsParent;

    private Slot[] slots;
    // Start is called before the first frame update
    void Start()
    {
        slots = go_SlotsParent.GetComponentsInChildren<Slot>();//자식 객체 가져오기
    }

    // Update is called once per frame
    void Update()
    {
        TryOpenInventory();
    }

    private void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !inventoryActivated;

            if (inventoryActivated)
            {
                OpenInventory();
            }
            else
            {
                CloseInventory();
            }
        }
    }
    private void OpenInventory()
    {
        GameManager.isOpenInventory = true;
        go_InventoryBase.SetActive(true);
    }
    private void CloseInventory() 
    {
        GameManager.isOpenInventory = false;
        go_InventoryBase.SetActive(false);
    }
    public void AcquireItem(Item _item, int _count)
    {
        if (Item.ItemType.Equipment != _item.itemType)//주운 아이템이 장비일 때
        {
            for (int i = 0; i < slots.Length; i++)//슬롯 전체를 돌아봐서
            {
                if (slots[i].item != null)//아이템이 존재할 때(Null 포인터 참조 예방)
                {
                    if (slots[i].item.itemName == _item.itemName)//이름이 일치하는 아이템이 있으면
                    {
                        slots[i].SetSlotCount(_count);//아이탬 수량만 설정
                        return;
                    }
                }
            }
        }

        for (int i = 0; i < slots.Length; i++)//슬롯 전체를 돌아봐서
        {
            if (slots[i].item == null)//빈 슬롯이 있으면
            {
                slots[i].AddItem(_item,_count);//이 슬롯에 아이템 추가
                return;
            }
        }
    }

    public Slot[] GetSlots()//자신의 슬롯들을 세이브를 위해 리턴
    {
        return slots;
    }
    [SerializeField]
    private Item[] items;
    public void LoadToInven(int _arrayNum, string _name, int _itemNum) {
        for(int i=0;i<items.Length; i++)
        {
            if (items[i].itemName == _name)
            {
                slots[_arrayNum].AddItem(items[i], _itemNum);//아이템을 슬롯에 저장
            }
        }
    
    }
}
