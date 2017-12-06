using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tables;

public class GemData : SaveItemBase
{
    #region 唯一

    private static GemData _Instance = null;
    public static GemData Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new GemData();
            }
            return _Instance;
        }
    }

    private GemData() { }

    #endregion

    #region gem pack

    public const int MAX_GEM_EQUIP = 6;

    [SaveField(1)]
    private List<ItemBase> _EquipedGems;

    public void InitGemPack()
    {
        if (_EquipedGems == null || _EquipedGems.Count == 0)
        {
            _EquipedGems = new List<ItemBase>();
            for (int i = 0; i < MAX_GEM_EQUIP; ++i)
            {
                _EquipedGems.Add(new ItemBase());
            }
        }
    }

    public void PutOnGem(ItemBase gem, int slot)
    {
        if (slot >= MAX_GEM_EQUIP)
        {
            UIMessageTip.ShowMessageTip("gem slot error");
            return;
        }

        int putOnSlot = -1;
        if (slot < 0)
        {
            for (int i = 0; i < MAX_GEM_EQUIP; ++i)
            {
                if (!_EquipedGems[i].IsVolid())
                {
                    putOnSlot = i;
                }
            }
        }
        if (putOnSlot == -1)
        {
            UIMessageTip.ShowMessageTip(30001);
            return;
        }

        _EquipedGems[putOnSlot].ExchangeInfo(gem);
    }

    public void PutOff()
    {

    }

    #endregion

    #region gem container

    private List<ItemBase> _GemContainer;

    private void InitGemContainer()
    {
        if (_GemContainer == null || _GemContainer.Count == 0)
        {
            _GemContainer = new List<ItemBase>();
            foreach (var gemRecord in TableReader.GemTable.Records.Values)
            {
                ItemBase gemItem = new ItemBase();
                gemItem.ItemDataID = gemRecord.Id;
                gemItem.ItemStackNum = 0;

                _GemContainer.Add(gemItem);
            }
        }
    }

    #endregion
}
