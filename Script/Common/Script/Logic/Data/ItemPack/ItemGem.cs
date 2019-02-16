using UnityEngine;
using System.Collections;
using Tables;

public class ItemGem : ItemBase
{
    public ItemGem(string dataID) : base(dataID)
    {

    }

    public ItemGem() : base()
    {

    }

    #region base attr

    public const int _MaxGemLevel = 5;

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
                RefreshGemAttr();
            }
            return _GemAttr;
        }
    }

    public void RefreshGemAttr()
    {
        _GemAttr = GameDataValue.GetGemAttr((RoleAttrEnum)GemRecord.AttrValue.AttrParams[0], GemRecord.AttrValue.AttrParams[1]);
    }

    #endregion

}

