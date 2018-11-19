using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Tables;
using System;

public class ItemFiveElement : ItemBase
{
    public ItemFiveElement(string datID) : base(datID)
    {

    }

    public ItemFiveElement():base()
    {

    }

    #region elevemt data

    private FiveElementRecord _FiveElementRecord;
    public FiveElementRecord FiveElementRecord
    {
        get
        {
            if (_FiveElementRecord == null)
            {
                if (string.IsNullOrEmpty(_ItemDataID))
                    return null;

                if (_ItemDataID == "-1")
                    return null;

                _FiveElementRecord = TableReader.FiveElement.GetRecord(_ItemDataID);
            }
            return _FiveElementRecord;
        }
    }

    private int _CombatValue = 0;
    public int CombatValue
    {
        get
        {
            return _CombatValue;
        }
    }

    public void CalculateCombatValue()
    {
        _CombatValue = 0;

        foreach (var exAttrs in EquipExAttrs)
        {
            if (exAttrs.AttrType == "RoleAttrImpactBaseAttr")
            {
                _CombatValue += GameDataValue.GetAttrValue((RoleAttrEnum)exAttrs.AttrParams[0], exAttrs.AttrParams[1]);
            }
            else
            {
                _CombatValue += 0;
            }
        }
    }

    public ITEM_QUALITY GetElementQuality()
    {
        if (EquipExAttrs.Count == 1)
        {
            return ITEM_QUALITY.WHITE;
        }
        else if (EquipExAttrs.Count == 2)
        {
            return ITEM_QUALITY.GREEN;
        }
        else if (EquipExAttrs.Count == 3)
        {
            return ITEM_QUALITY.BLUE;
        }
        else if (EquipExAttrs.Count == 4)
        {
            return ITEM_QUALITY.PURPER;
        }
        else if (EquipExAttrs.Count == 5)
        {
            return ITEM_QUALITY.ORIGIN;
        }

        return ITEM_QUALITY.WHITE;
    }

    public string GetElementNameWithColor()
    {
        string equipName = StrDictionary.GetFormatStr(CommonItemRecord.NameStrDict);
        return CommonDefine.GetQualityColorStr(GetElementQuality()) + equipName + "</color>";
    }

    #endregion

    #region attr

    private List<EquipExAttr> _EquipExAttrs;
    public List<EquipExAttr> EquipExAttrs
    {
        get
        {
            if (_EquipExAttrs == null)
            {
                _EquipExAttrs = new List<global::EquipExAttr>();
                foreach (var strParam in _DynamicDataEx)
                {
                    EquipExAttr exAttr = new global::EquipExAttr();
                    exAttr.AttrType = strParam._StrParams[0];
                    exAttr.Value = int.Parse(strParam._StrParams[1]);
                    for (int i = 2; i < strParam._StrParams.Count; ++i)
                    {
                        exAttr.AttrParams.Add(int.Parse(strParam._StrParams[i]));
                    }
                    _EquipExAttrs.Add(exAttr);
                }
                CalculateCombatValue();
            }
            return _EquipExAttrs;
        }
        set
        {
            _EquipExAttrs = value;
            CalculateCombatValue();
            BakeExAttr();
        }
    }

    public void BakeExAttr()
    {
        _DynamicDataEx.Clear();
        foreach (var exAttr in EquipExAttrs)
        {
            ItemExData exData = new ItemExData();
            exData._StrParams.Add(exAttr.AttrType);
            exData._StrParams.Add(exAttr.Value.ToString());
            for (int i = 0; i < exAttr.AttrParams.Count; ++i)
            {
                exData._StrParams.Add(exAttr.AttrParams[i].ToString());
            }
            _DynamicDataEx.Add(exData);
        }
        SaveClass(true);
    }

    public void ReplaceAttr(int idx, EquipExAttr attr)
    {
        if (EquipExAttrs.Count < idx)
            return;

        EquipExAttrs[idx] = attr;
        CalculateCombatValue();
        BakeExAttr();
    }

    public void AddExAttr(EquipExAttr attr)
    {
        EquipExAttrs.Add(attr);
        ItemExData exData = new ItemExData();
        exData._StrParams.Add(attr.AttrType);
        exData._StrParams.Add(attr.Value.ToString());
        for (int i = 0; i < attr.AttrParams.Count; ++i)
        {
            exData._StrParams.Add(attr.AttrParams[i].ToString());
        }
        _DynamicDataEx.Add(exData);
        CalculateCombatValue();
    }

    #endregion

    #region fun

    public override void RefreshItemData()
    {
        base.RefreshItemData();
        _FiveElementRecord = null;
        _EquipExAttrs = null;
    }

    public override void ResetItem()
    {
        base.ResetItem();
        _FiveElementRecord = null;
        _EquipExAttrs = null;
    }

    #endregion


}

