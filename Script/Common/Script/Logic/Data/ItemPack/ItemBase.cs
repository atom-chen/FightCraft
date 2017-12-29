using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tables;
using System;

public class ItemExData
{
    [SaveField(1)]
    public List<string> _StrParams = new List<string>();
}

public class ItemBase : SaveItemBase
{
    public ItemBase()
    {
        ItemDataID = "";
    }

    public ItemBase(string itemDataID)
    {
        ItemDataID = itemDataID;
    }

    public ItemBase(string itemDataID, int num)
    {
        ItemDataID = itemDataID;
        ItemStackNum = GetVolidItemNum(num);
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
    protected List<int> _DynamicDataInt = new List<int>();

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
        protected set
        {
            if (_DynamicDataInt.Count == 0)
            {
                _DynamicDataInt.Add(0);
            }
            _DynamicDataInt[0] = value;
        }
    }

    private int GetVolidItemNum(int num)
    {
        return Math.Max(0, num);
    }

    public int SetStackNum(int num)
    {
        int temp = num;
        ItemStackNum = GetVolidItemNum(temp);
        SaveClass(true);
        return ItemStackNum;
    }

    public int AddStackNum(int num)
    {
        int temp = num + ItemStackNum;
        ItemStackNum = GetVolidItemNum(temp);
        SaveClass(true);
        return ItemStackNum;
    }

    public int DecStackNum(int num)
    {
        int temp = ItemStackNum - num;
        ItemStackNum = GetVolidItemNum(temp);
        SaveClass(true);
        return ItemStackNum;
    }

    [SaveField(3)]
    protected List<ItemExData> _DynamicDataEx = new List<ItemExData>();

    #region fun

    public virtual void RefreshItemData()
    {
        _CommonItemRecord = null;
    }

    public virtual void ResetItem()
    {
        _ItemDataID = "-1";
        _DynamicDataInt = new List<int>();
        _DynamicDataEx = new List<ItemExData>();
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

        var tempDataVector = itembase._DynamicDataEx;
        itembase._DynamicDataEx = _DynamicDataEx;
        _DynamicDataEx = tempDataVector;

        itembase.RefreshItemData();
        RefreshItemData();
        SaveClass(true);
        itembase.SaveClass(true);
        //LogicManager.Instance.SaveGame();
    }

    public void CopyFrom(ItemBase itembase)
    {
        if (itembase == null)
            return;

        ItemDataID = itembase.ItemDataID;

        _DynamicDataInt = itembase._DynamicDataInt;

        _DynamicDataEx = itembase._DynamicDataEx;

        RefreshItemData();
    }

    #endregion

    #region static create item

    public static ItemBase CreateItem(string itemID)
    {
        ItemBase newItem = new ItemBase(itemID);
        return newItem;
    }

    public static void CreateItemInPack(string itemID, int num)
    {
        int itemDataID = int.Parse(itemID);
        if (itemDataID >= 70100 && itemDataID <= 70105)
        {
            GemData.Instance.AddMaterial(itemID, num);
        }
        else
        {
            BackBagPack.Instance.AddItem(itemID, num);
        }
    }

    #endregion
}

