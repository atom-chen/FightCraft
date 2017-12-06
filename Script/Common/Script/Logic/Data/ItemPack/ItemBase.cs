using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tables;
using System;

public class EquipExAttr
{
    [SaveField(1)]
    public string AttrType;
    [SaveField(2)]
    public List<int> AttrValues;


    public EquipExAttr()
    {
        AttrValues = new List<int>();
        //AttrValues.Add(0);
    }

    public EquipExAttr(string attrType, params int[] attrValues)
    {
        AttrType = attrType;
        AttrValues = new List<int>(attrValues);
    }

    public EquipExAttr(EquipExAttr copyInstance)
    {
        AttrType = copyInstance.AttrType;
        AttrValues = copyInstance.AttrValues;
    }

    public string GetAttrStr()
    {
        var impactType = Type.GetType(AttrType);
        Debug.Log("AttrType:" + AttrType);
        if (impactType == null)
            return "";

        var method = impactType.GetMethod("GetAttrDesc");
        if (method == null)
            return "";

        return method.Invoke(null, new object[] { AttrValues }) as string;
    }

    public bool Add(EquipExAttr d)
    {
        if (d.AttrType != AttrType)
            return false;

        if (AttrType == "RoleAttrImpactBaseAttr")
        {
            for (int i = 0; i < AttrValues.Count; ++i)
            {
                AttrValues[i] += d.AttrValues[i];
            }
            return true;
        }

        return false;
    }
}

public class ItemBase : SaveItemBase
{
    public ItemBase()
    {
        ItemDataID = "";
    }

    [SaveField(1)]
    protected string _ItemDataID;
    public string ItemDataID
    {
        get
        {
            return _ItemDataID;
        }
        set
        {
            _ItemDataID = value;
        }
    }

    private CommonItemRecord _CommonItemRecord;
    public CommonItemRecord CommonItemRecord
    {
        get
        {
            if (_CommonItemRecord == null)
            {
                if (string.IsNullOrEmpty(_ItemDataID))
                    return null;

                if (_ItemDataID == "-1")
                    return null;

                _CommonItemRecord = TableReader.CommonItem.GetRecord(_ItemDataID);
            }
            return _CommonItemRecord;
        }
    }

    public bool IsVolid()
    {
        if (string.IsNullOrEmpty(_ItemDataID) || _ItemDataID == "-1")
            return false;
        return true;
    }

    [SaveField(2)]
    public List<int> _DynamicDataInt = new List<int>();

    public int ItemStackNum
    {
        get
        {
            if (_DynamicDataInt.Count == 0)
            {
                _DynamicDataInt.Add(0);
            }
            return _DynamicDataInt[0];
        }
        set
        {
            if (_DynamicDataInt.Count == 0)
            {
                _DynamicDataInt.Add(0);
            }
            _DynamicDataInt[0] = value;
        }
    }

    [SaveField(3)]
    public List<EquipExAttr> _DynamicDataVector = new List<EquipExAttr>();

    #region fun

    public virtual void RefreshItemData()
    {
        _CommonItemRecord = null;
    }

    public virtual void ResetItem()
    {
        _ItemDataID = "-1";
        _DynamicDataInt = new List<int>();
        _DynamicDataVector = new List<EquipExAttr>();
    }

    public void ExchangeInfo(ItemBase itembase)
    {
        if (itembase == null)
            return;

        var tempId = itembase.ItemDataID;
        itembase.ItemDataID = ItemDataID;
        ItemDataID = tempId;

        var tempData = itembase._DynamicDataInt;
        itembase._DynamicDataInt = _DynamicDataInt;
        _DynamicDataInt = tempData;

        var tempDataVector = itembase._DynamicDataVector;
        itembase._DynamicDataVector = _DynamicDataVector;
        _DynamicDataVector = tempDataVector;

        itembase.RefreshItemData();
        RefreshItemData();

        //LogicManager.Instance.SaveGame();
    }

    #endregion

    #region static create item

    public static ItemBase CreateItem(string itemID)
    {
        ItemBase newItem = new ItemBase();
        newItem.ItemDataID = itemID;
        return newItem;
    }

    #endregion
}

