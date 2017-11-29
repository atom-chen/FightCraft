
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
        if (_ItemEquip != null)
        {
            attrStr = CommonDefine.GetQualityColorStr(_ItemEquip.EquipQuality) + attrStr + "</color>";
        }
        _AttrText.text = attrStr;
    }


}

