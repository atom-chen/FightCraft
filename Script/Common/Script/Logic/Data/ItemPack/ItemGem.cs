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

    public const int _MaxGemLevel = 200;

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
        RefreshGemAttr();
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
                int recordLv = Mathf.Clamp(Level, 0, _MaxGemLevel);
                var levelRecord = TableReader.GemBaseAttr.GetRecord(recordLv.ToString());
                _GemAttr = GameDataValue.GetGemAttr((RoleAttrEnum)GemRecord.AttrValue.AttrParams[0], levelRecord.Value);
            }
            return _GemAttr;
        }
    }

    public void RefreshGemAttr()
    {
        int recordLv = Mathf.Clamp(Level, 0, _MaxGemLevel);
        var levelRecord = TableReader.GemBaseAttr.GetRecord(recordLv.ToString());
        _GemAttr = GameDataValue.GetGemAttr((RoleAttrEnum)GemRecord.AttrValue.AttrParams[0], levelRecord.Value);
    }

    #endregion

}

