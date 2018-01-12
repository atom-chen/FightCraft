using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
using UnityEngine.EventSystems;
using System;
using Tables;

public class RefreshAttr
{
    public EquipExAttr _ShowAttr;
    public int _OrgValue;
}

public class UIEquipRefreshAttrItem : UIItemBase
{
    public Text _AttrText;
    public Text _Value;
    public Text _AddValue;

    private int _OrgValue;
    private ItemEquip _ItemEquip;
    private EquipExAttr _ShowAttr;

    public override void Init()
    {
        base.Init();

        _OrgValue = 0;
    }

    public override void Show(Hashtable hash)
    {
        base.Show();

        var showItem = (RefreshAttr)hash["InitObj"];
        _ItemEquip = (ItemEquip)hash["ItemEquip"];

        _OrgValue = showItem._OrgValue;
        ShowAttr(showItem._ShowAttr);
    }

    public override void Refresh()
    {
        base.Refresh();

        ShowAttr(_ShowAttr);
    }

    private void SetValueDelta()
    {
        var valueDelta = _ShowAttr.Value - _OrgValue;
        if (valueDelta != 0 && _OrgValue != 0)
        {
            _AddValue.text = string.Format("+{0}", valueDelta);
        }
        else
        {
            _AddValue.text = "";
        }
    }

    public void ShowAttr(EquipExAttr attr)
    {

        _ShowAttr = attr;

        string attrStr = _ShowAttr.GetAttrStr();
        //string valueStr = "";
        //if (_ShowAttr.Value > 0)
        //{
        //    valueStr = string.Format("({0})", _ShowAttr.Value);
        //}
        _AddValue.text = "";
        _Value.text = _ShowAttr.Value.ToString();
        if (_ItemEquip != null)
        {
            attrStr = CommonDefine.GetQualityColorStr(_ItemEquip.EquipQuality) + attrStr + "</color>";
        }
        _AttrText.text = attrStr;
        SetValueDelta();
    }


}

