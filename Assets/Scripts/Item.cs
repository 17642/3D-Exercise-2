using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "new Item/item")]
public class Item : ScriptableObject
{

    public string itemName;//이름
    public ItemType itemType;//유형
    public Sprite itemImage;//이미지
    public GameObject itemPrefab;//아이템 프리팹

    public string weaponType;//무기 유형

    public enum ItemType//아이템 유형 정의(리스트로 나옴)
    {
        Equipment,
        Used,
        Ingredient,
        ETC
    }
    
 
}
