using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotToolTip : MonoBehaviour
{
    [SerializeField]
    private GameObject go_Base;

    [SerializeField]
    private Text text_ItemName;
    [SerializeField]
    private Text text_ItemDesc;
    [SerializeField]
    private Text text_ItemHTUsed;
    

    public void ShowToolTip(Item _item,Vector3 _pos)
    {
        _pos += new Vector3(go_Base.GetComponent<RectTransform>().rect.width * 0.5f, -go_Base.GetComponent<RectTransform>().rect.height);
        go_Base.SetActive(true);
        go_Base.transform.position = _pos;

        text_ItemName.text = _item.itemName;
        text_ItemDesc.text = _item.itemDesc;

        if (_item.itemType == Item.ItemType.Equipment)//종류에 따라 사용법 설명 변경
        {
            text_ItemHTUsed.text = "우클릭 - 장착";
        }
        else if (_item.itemType == Item.ItemType.Used)
        {
            text_ItemHTUsed.text = "우클릭 - 먹기";
        }
        else
        {
            text_ItemHTUsed.text = "";
        }
    }

    public void HideToolTIp()
    {
        go_Base.SetActive (false);
    }
}
