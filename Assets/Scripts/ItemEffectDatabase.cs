using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]//Ŭ���� �����Ť�ȭ
public class ItemEffect
{
    public string itemName;//������ �̸�(Key)
    [Tooltip("HP, SP, DP, HUNGRY, THIRSTY, SATISFY�� ����")]
    public string[] part;//�ο��
    public int[] num;//��ġ
}
public class ItemEffectDatabase : MonoBehaviour
{
    [SerializeField]
    private ItemEffect[] itemEffects;
    [SerializeField]
    private WeaponManager weaponManager;
    [SerializeField]
    private StatusController statusController;

    private const string HP = "HP", SP = "SP", DP = "DP", HUNGRY = "HUNGRY", THIRSTY = "THIRSTY", SATISFY = "SATISFY";
    public void UseItem(Item _item)
    {
        if (_item.itemType == Item.ItemType.Used)
        {
            for(int i=0; i < itemEffects.Length; i++)
            {
                if (itemEffects[i].itemName == _item.itemName)
                {
                    for (int j = 0; j < itemEffects[i].part.Length; j++)
                    {
                        switch (itemEffects[i].part[j])
                        {
                            case HP:
                                statusController.IncreaseHP(itemEffects[i].num[j]);
                                break;
                            case SP:
                                statusController.IncreaseSP(itemEffects[i].num[j]);
                                break;
                            case DP:
                                statusController.IncreaseDP(itemEffects[i].num[j]);
                                break;
                            case HUNGRY:
                                statusController.IncreaseHungry(itemEffects[i].num[j]);
                                break;
                            case THIRSTY:
                                statusController.IncreaseThirsty(itemEffects[i].num[j]);
                                break;
                            case SATISFY:
                                //statusController.IncreaseSatisfy(itemEffects[i].num[j]);
                                break;
                            default:
                                Debug.Log("�߸��� Status ����. HP, SP, DP, HUNGRY, THIRSTY, SATISFY�� ����");
                                break;
                        }
                    }
                    return;//������ ȿ�� Ž���� �������� �Լ� ����
                }
            }
            Debug.Log("ItemDataBase�� ��ġ�ϴ� itemName ����");//ã�� �������� ���� �� �α�
        }
        if(_item.itemType == Item.ItemType.Equipment)
        {
            StartCoroutine(weaponManager.ChangeWeaponCoroutine(_item.weaponType, _item.itemName));
        }
    }
}
