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
        go_InventoryBase.SetActive(true);
    }
    private void CloseInventory() 
    {
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
}
