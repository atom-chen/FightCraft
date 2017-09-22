using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tables;



public class EquipExAttr
{
    [SaveField(1)]
    public RoleAttrEnum AttrID;
    [SaveField(2)]
    public int SubClass;
    [SaveField(3)]
    public int AttrValue1;


    public EquipExAttr()
    { }

    public EquipExAttr(RoleAttrEnum attrEnum, int subClass = 0, int value1 = 0)
    {
        AttrID = attrEnum;
        AttrValue1 = value1;
        SubClass = subClass;
    }

    public EquipExAttr(EquipExAttr copyInstance)
    {
        AttrID = copyInstance.AttrID;
        SubClass = copyInstance.SubClass;
        AttrValue1 = copyInstance.AttrValue1;
    }

    public string GetAttrStr()
    {
        return AttrDisplay.GetAttrDisplayStr(AttrID, SubClass, AttrValue1);
    }

    public bool Add(EquipExAttr d)
    {
        if (d.AttrID != AttrID)
            return false;

        if (d.SubClass != SubClass)
            return false;

        AttrValue1 += d.AttrValue1;
        return true;
    }
}

public class ItemBase : SaveItemBase
{
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

