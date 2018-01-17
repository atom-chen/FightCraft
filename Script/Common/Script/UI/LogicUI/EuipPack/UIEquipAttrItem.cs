
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

 
using UnityEngine.EventSystems;
using System;
using Tables;



public class UIEquipAttrItem : UIItemBase
{
    public Text _AttrText;

    private ItemEquip _ItemEquip;
    private EquipExAttr _ShowAttr;

    public override void Show(Hashtable hash)
    {
        base.Show();

        var showItem = (EquipExAttr)hash["InitObj"];
        _ItemEquip = (ItemEquip)hash["ItemEquip"];

        ShowAttr(showItem);
    }

    public override void Refresh()
    {
        base.Refresh();

        ShowAttr(_ShowAttr);
    }

    public void ShowAttr(EquipExAttr attr)
    {

        _ShowAttr = attr;

        string attrStr = _ShowAttr.GetAttrStr();
        string valueStr = "";
        if (_ShowAttr.Value > 0)
        {
            valueStr = string.Format("({0})", _ShowAttr.Value);
        }
        if (_ItemEquip != null)
        {
            attrStr = CommonDefine.GetQualityColorStr(attr.AttrQuality) + attrStr + valueStr + "</color>";
        }
        _AttrText.text = attrStr;
    }


}

