using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]//클래스 직ㄹㅕㄹ화
public class ItemEffect
{
    public string itemName;//아이템 이름(Key)
    [Tooltip("HP, SP, DP, HUNGRY, THIRSTY, SATISFY만 가능")]
    public string[] part;//부우ㅣ
    public int[] num;//수치
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
                                Debug.Log("잘못된 Status 부위. HP, SP, DP, HUNGRY, THIRSTY, SATISFY만 가능");
                                break;
                        }
                    }
                    return;//아이템 효과 탐색이 끝났으면 함수 종료
                }
            }
            Debug.Log("ItemDataBase에 일치하는 itemName 없음");//찾은 아이템이 없을 시 로그
        }
        if(_item.itemType == Item.ItemType.Equipment)
        {
            StartCoroutine(weaponManager.ChangeWeaponCoroutine(_item.weaponType, _item.itemName));
        }
    }
}
