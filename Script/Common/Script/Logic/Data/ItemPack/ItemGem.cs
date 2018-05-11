using UnityEngine;
using System.Collections;
using Tables;

public class ItemGem : ItemBase
{
    public ItemGem(string dataID) : base(dataID)
    {
        Level = 0;
    }

    public ItemGem() : base()
    {
        Level = 0;
    }

    #region base attr
    public int Level
    {
        get
        {
            return ItemStackNum;
        }
        protected set
        {
            ItemStackNum = value;
        }
    }

    public int LevelUp()
    {
        ++Level;
        SaveClass(true);
        return Level;
    }

    private GemTableRecord _GemRecord;
    public GemTableRecord GemRecord
    {
        get
        {
            if (_GemRecord == null)
            {
                _GemRecord = TableReader.GemTable.GetRecord(ItemDataID);
            }
            return _GemRecord;
        }
    }


    #endregion 

    #region override

    public override void RefreshItemData()
    {
        base.RefreshItemData();
        _GemRecord = null;
    }

    public override void ResetItem()
    {
        base.ResetItem();
        _GemRecord = null;
    }

    #endregion

    #region fun

    private EquipExAttr _GemAttr;
    public EquipExAttr GemAttr
    {
        get
        {
            if (_GemAttr == null)
            {
                _GemAttr = GameDataValue.GetGemAttr((RoleAttrEnum)GemRecord.AttrValue.AttrParams[0], Level);
            }
            return _GemAttr;
        }
    }

    

    #endregion

}

