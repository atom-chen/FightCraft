using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tables;

public class GemLevelInfo
{
    public string MaterialData;
    public int MaterialCnt;
    public int CostMoney;
}

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

    private GemData()
    {
        _SaveFileName = "GemData";
    }

    #endregion

    public void InitGemData()
    {
        bool needSave = false;
        needSave |= InitGemContainer();
        needSave |= InitGemPack();

        if (needSave)
        {
            SaveClass(true);
        }
    }

    #region gem pack

    public const int MAX_GEM_EQUIP = 6;

    [SaveField(1)]
    public List<ItemGem> _EquipedGems;

    public bool InitGemPack()
    {
        if (_EquipedGems == null || _EquipedGems.Count < MAX_GEM_EQUIP)
        {
            if (_EquipedGems == null)
            {
                _EquipedGems = new List<ItemGem>();
            }
            int startIdx = _EquipedGems.Count;
            for (int i = startIdx; i < MAX_GEM_EQUIP; ++i)
            {
                _EquipedGems.Add(new ItemGem("-1"));
            }
            return true;
        }

        foreach (var gemEquiped in _EquipedGems)
        {
            foreach (var gemInPack in _GemContainer)
            {
                if (gemEquiped.ItemDataID == gemInPack.ItemDataID)
                {
                    gemEquiped.CopyFrom(gemInPack);
                }
            }
        }


        GemSuit.Instance.IsActSet();
        return false;
    }

    public int GetPutOnIdx()
    {
        for (int i = 0; i < _EquipedGems.Count; ++i)
        {
            if (!_EquipedGems[i].IsVolid())
                return i;
        }
        return 0;
    }

    public bool PutOnGem(ItemGem gem, int slot)
    {
        if (slot >= MAX_GEM_EQUIP)
        {
            UIMessageTip.ShowMessageTip("gem slot error");
            return false;
        }

        if (_EquipedGems.Contains(gem))
        {
            UIMessageTip.ShowMessageTip("allready put on gem");
            return false;
        }

        int putOnSlot = slot;
        if (slot < 0)
        {
            putOnSlot = GetPutOnIdx();
        }
        if (putOnSlot == -1)
        {
            UIMessageTip.ShowMessageTip(30001);
            return false;
        }

        if (gem.ItemStackNum < 1)
        {
            UIMessageTip.ShowMessageTip(30002);
            return false;
        }

        _EquipedGems[putOnSlot].CopyFrom(gem);

        GemSuit.Instance.IsActSet();
        return true;
    }

    public bool PutOff(ItemGem gem)
    {
        if (!_EquipedGems.Contains(gem))
            return false;

        gem.ResetItem();
        return true;
    }

    public void ExchangeGem(ItemGem gem1, ItemGem gem2)
    {
        if (!_EquipedGems.Contains(gem1))
            return;

        if (!_EquipedGems.Contains(gem2))
            return;

        gem1.ExchangeInfo(gem2);
    }

    public bool IsEquipedGem(string gemDataID)
    {
        foreach (var gem in _EquipedGems)
        {
            if (gem.ItemDataID == gemDataID)
                return true;
        }

        return false;
    }

    #endregion

    #region gem container

    [SaveField(2)]
    public List<ItemGem> _GemContainer;

    public static List<string> _GemMaterialDataIDs = new List<string>() { "70100", "70101", "70102", "70103", "70104" };

    private bool InitGemContainer()
    {
        var gemTabs = TableReader.GemTable.Records;
        if (_GemContainer == null || _GemContainer.Count != gemTabs.Count)
        {
            if (_GemContainer == null)
            {
                _GemContainer = new List<ItemGem>();
            }
            foreach (var gemRecord in TableReader.GemTable.Records.Values)
            {
                var gemItem = _GemContainer.Find((itemGem) =>
                {
                    if (itemGem.ItemDataID == gemRecord.Id)
                    {
                        return true;
                    }
                    return false;
                });
                if (gemItem == null)
                {
                    gemItem = new ItemGem(gemRecord.Id);
                    _GemContainer.Add(gemItem);
                }
            }
            return true;
        }
        return false;
    }
    
    public ItemGem GetGemInfo(string gemData)
    {
        foreach (var gemInfo in _GemContainer)
        {
            if (gemInfo.ItemDataID == gemData)
            {
                return gemInfo;
            }
        }
        return null;
    }

    public GemLevelInfo GetGemLevelUpInfo(ItemGem gemItemBase)
    {
        ItemGem lvUpGem = gemItemBase;

        var gemRecord = TableReader.GemTable.GetRecord(lvUpGem.ItemDataID);
        if (gemRecord == null)
            return null;

        GemLevelInfo lvInfo = new GemLevelInfo();
        lvInfo.MaterialData = gemRecord.LevelUpParam.ToString();
        lvInfo.MaterialCnt = (2 ^ gemItemBase.ItemStackNum) * 6;
        lvInfo.CostMoney = (2 ^ gemItemBase.ItemStackNum) * 1200;

        return lvInfo;
    }

    public bool IsCanLevelUp(ItemGem gemItemBase, GemLevelInfo lvInfo = null)
    {
        if (lvInfo == null)
            lvInfo = GetGemLevelUpInfo(gemItemBase);

        if (PlayerDataPack.Instance.Gold < lvInfo.CostMoney)
        {
            UIMessageTip.ShowMessageTip(20000);
            return false;
        }

        if (BackBagPack.Instance.GetItemCnt(lvInfo.MaterialData) < lvInfo.MaterialCnt)
        {
            UIMessageTip.ShowMessageTip(30003);
            return false;
        }

        return true;
    }

    public bool GemLevelUp(ItemGem gemItemBase, GemLevelInfo lvInfo = null)
    {
        ItemGem lvUpGem = gemItemBase;
        if (_EquipedGems.Contains(gemItemBase))
        {
            foreach (var gem in _GemContainer)
            {
                if (gem.ItemDataID == gemItemBase.ItemDataID)
                {
                    lvUpGem = gem;
                    break;
                }
            }
        }

        if (lvUpGem == null)
            return false;

        if (lvInfo == null)
            lvInfo = GetGemLevelUpInfo(gemItemBase);

        if (!IsCanLevelUp(gemItemBase, lvInfo))
            return false;

        PlayerDataPack.Instance.DecGold(lvInfo.CostMoney);
        var matItem = BackBagPack.Instance.GetItem(lvInfo.MaterialData);
        matItem.DecStackNum(lvInfo.MaterialCnt);

        lvUpGem.LevelUp();

        return true;
    }

    #endregion
}
