using UnityEngine;
using System.Collections;
using Tables;

public class ItemGem : ItemBase
{
    #region base attr
    public int Level
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

    public EquipExAttr GetExAttr()
    {
        if (_GemAttr == null)
        {
            _GemAttr = new EquipExAttr();
            _GemAttr.AttrType = "RoleAttrImpactBaseAttr";
            _GemAttr.AttrValues.Add(GemRecord.BaseAttr);
            var attrValue = GemRecord.AttrStep[0] + GemRecord.AttrStep[1] * Level;
            _GemAttr.AttrValues.Add(attrValue);
        }
        return _GemAttr;
    }

    #endregion
}

