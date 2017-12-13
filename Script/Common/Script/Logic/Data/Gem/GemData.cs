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
        InitGemContainer();
        InitGemMaterials();
        InitGemPack();
    }

    #region gem pack

    public const int MAX_GEM_EQUIP = 6;

    [SaveField(1)]
    public List<ItemGem> _EquipedGems;

    public void InitGemPack()
    {
        if (_EquipedGems == null || _EquipedGems.Count == 0)
        {
            _EquipedGems = new List<ItemGem>();
            for (int i = 0; i < MAX_GEM_EQUIP; ++i)
            {
                _EquipedGems.Add(new ItemGem());
            }
        }
        else
        {
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
        }
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
        return true;
    }

    public bool PutOff(ItemGem gem)
    {
        if (!_EquipedGems.Contains(gem))
            return false;

        gem.ResetItem();
        return true;
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

    [SaveField(3)]
    public List<ItemBase> _GemMaterials;

    private void InitGemContainer()
    {
        if (_GemContainer == null || _GemContainer.Count == 0)
        {
            _GemContainer = new List<ItemGem>();
            foreach (var gemRecord in TableReader.GemTable.Records.Values)
            {
                ItemGem gemItem = new ItemGem();
                gemItem.ItemDataID = gemRecord.Id;
                gemItem.ItemStackNum = 0;

                _GemContainer.Add(gemItem);
            }
        }
    }

    private void InitGemMaterials()
    {
        if (_GemMaterials == null || _GemMaterials.Count == 0)
        {
            _GemMaterials = new List<ItemBase>();
            _GemMaterials.Add(new ItemBase() { ItemDataID = "70100" });
            _GemMaterials.Add(new ItemBase() { ItemDataID = "70101" });
            _GemMaterials.Add(new ItemBase() { ItemDataID = "70102" });
            _GemMaterials.Add(new ItemBase() { ItemDataID = "70103" });
            _GemMaterials.Add(new ItemBase() { ItemDataID = "70104" });
            //_GemMaterials.Add(new ItemBase() { ItemDataID = "70105" });
        }
    }

    public bool AddMaterial(string itemID, int itemNum)
    {
        if (itemNum <= 0)
            return false;

        for (int i = 0; i < _GemMaterials.Count; ++i)
        {
            if (_GemMaterials[i].ItemDataID == itemID)
            {
                _GemMaterials[i].ItemStackNum += itemNum;
                return true;
            }
        }

        return false;
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

        foreach (var mat in _GemMaterials)
        {
            if (mat.ItemDataID == lvInfo.MaterialData)
            {
                if (mat.ItemStackNum < lvInfo.MaterialCnt)
                {
                    UIMessageTip.ShowMessageTip(30003);
                    return false;
                }
            }
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
        foreach (var mat in _GemMaterials)
        {
            if (mat.ItemDataID == lvInfo.MaterialData)
            {
                mat.ItemStackNum -= lvInfo.MaterialCnt;
            }
        }

        lvUpGem.ItemStackNum += 1;

        return true;
    }

    #endregion
}
