
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

 
using UnityEngine.EventSystems;
using System;
using Tables;



public class UIElementExtraItem : UIItemSelect
{
    public Text _Name;
    public Text _AttrText;
    public Image _Icon;
    public Image _Quality;

    private ItemFiveElement _ItemElement;

    public override void Show(Hashtable hash)
    {
        base.Show();

        var showItem = (ItemFiveElement)hash["InitObj"];

        ShowAttr(showItem);
    }

    public void ShowAttr(ItemFiveElement itemElement)
    {

        _ItemElement = itemElement;

        string attrStr = _ItemElement.EquipExAttrs[0].GetAttrStr();
        _AttrText.text = attrStr;
        _Name.text = itemElement.GetElementNameWithColor();
        _Icon.gameObject.SetActive(true);
        _Quality.gameObject.SetActive(true);
    }


}

