using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false;//�κ��丮 Ȱ��ȭ ����.

    //�ʿ� ������Ʈ
    [SerializeField]
    private GameObject go_InventoryBase;
    [SerializeField]
    private GameObject go_SlotsParent;

    private Slot[] slots;
    // Start is called before the first frame update
    void Start()
    {
        slots = go_SlotsParent.GetComponentsInChildren<Slot>();//�ڽ� ��ü ��������
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
        if (Item.ItemType.Equipment != _item.itemType)//�ֿ� �������� ����� ��
        {
            for (int i = 0; i < slots.Length; i++)//���� ��ü�� ���ƺ���
            {
                if (slots[i].item != null)//�������� ������ ��(Null ������ ���� ����)
                {
                    if (slots[i].item.itemName == _item.itemName)//�̸��� ��ġ�ϴ� �������� ������
                    {
                        slots[i].SetSlotCount(_count);//������ ������ ����
                        return;
                    }
                }
            }
        }

        for (int i = 0; i < slots.Length; i++)//���� ��ü�� ���ƺ���
        {
            if (slots[i].item == null)//�� ������ ������
            {
                slots[i].AddItem(_item,_count);//�� ���Կ� ������ �߰�
                return;
            }
        }
    }
}
