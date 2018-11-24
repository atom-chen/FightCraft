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

public class GemData
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
        
    }

    #endregion

    public void InitGemData()
    {
        InitGemContainer();
        InitGemPack();
        
    }

    #region gem pack

    public const int MAX_GEM_EQUIP = 6;

    private ItemPackBase<ItemGem> _EquipedGemDatas;
    public ItemPackBase<ItemGem> EquipedGemDatas
    {
        get
        {
            return _EquipedGemDatas;
        }
    }

    public void SaveEquipGem()
    {
        EquipedGemDatas.SaveClass(true);
    }

    public bool InitGemPack()
    {
        _EquipedGemDatas = new ItemPackBase<ItemGem>();
        _EquipedGemDatas._SaveFileName = "GemDataEquiped";
        _EquipedGemDatas._PackSize = MAX_GEM_EQUIP;
        _EquipedGemDatas.LoadClass(true);
        
        if (_EquipedGemDatas._PackItems == null || _EquipedGemDatas._PackItems.Count < MAX_GEM_EQUIP)
        {
            if (_EquipedGemDatas._PackItems == null)
            {
                _EquipedGemDatas._PackItems = new List<ItemGem>();
                _EquipedGemDatas._PackSize = MAX_GEM_EQUIP;
            }
            int equipSlotCnt = MAX_GEM_EQUIP;
            int startIdx = _EquipedGemDatas._PackItems.Count;
            for (int i = startIdx; i < equipSlotCnt; ++i)
            {
                ItemGem newItemGem = new ItemGem("-1");
                _EquipedGemDatas._PackItems.Add(newItemGem);
            }
            _EquipedGemDatas.SaveClass(true);
        }

        GemSuit.Instance.IsActSet();
        return false;
    }

    public int GetPutOnIdx()
    {
        for (int i = 0; i < EquipedGemDatas._PackItems.Count; ++i)
        {
            if (!EquipedGemDatas._PackItems[i].IsVolid())
                return i;
        }
        return -1;
    }

    public bool PutOnGem(ItemGem gem, int slot)
    {
        if (slot< 0 || slot >= MAX_GEM_EQUIP)
        {
            UIMessageTip.ShowMessageTip("gem slot error");
            return false;
        }

        var equipedGem = EquipedGemDatas.GetItem(gem.ItemDataID);
        if (equipedGem != null)
        {
            UIMessageTip.ShowMessageTip("allready put on gem");
            return false;
        }

        var putOnSlot = EquipedGemDatas._PackItems[slot];
        if (putOnSlot == null || putOnSlot.IsVolid())
        {
            UIMessageTip.ShowMessageTip(30001);
            return false;
        }

        if (gem.ItemStackNum < 1)
            return false;

        gem.DecStackNum(1);

        putOnSlot.ItemDataID = gem.ItemDataID;
        putOnSlot.AddStackNum(1);

        GemSuit.Instance.IsActSet();

        RoleData.SelectRole.CalculateAttr();
        return true;
    }

    public bool PutOff(ItemGem gem)
    {
        if (!PackGemDatas.AddItem(gem.ItemDataID, 1))
        {
            UIMessageTip.ShowMessageTip(10002);
            return false;
        }

        gem.DecStackNum(1);

        RoleData.SelectRole.CalculateAttr();
        return true;
    }

    public void ExchangeGem(ItemGem gem1, ItemGem gem2)
    {
        gem1.ExchangeInfo(gem2);
        GemSuit.Instance.IsActSet();

        RoleData.SelectRole.CalculateAttr();
    }

    public bool IsEquipedGem(string gemDataID)
    {
        var gemData = EquipedGemDatas.GetItem(gemDataID);
        return gemData != null;
    }

    #endregion

    #region gem container

    public const int MAX_GEM_PACK = 100;

    private ItemPackBase<ItemGem> _PackGemDatas;
    public ItemPackBase<ItemGem> PackGemDatas
    {
        get
        {
            return _PackGemDatas;
        }
    }

    private void InitGemContainer()
    {
        _PackGemDatas = new ItemPackBase<ItemGem>();
        _PackGemDatas._SaveFileName = "GemDataPack";
        _PackGemDatas._PackSize = MAX_GEM_PACK;
        _PackGemDatas.LoadClass(true);

        if (_PackGemDatas._PackItems == null)
        {
            _PackGemDatas._PackItems = new List<ItemGem>();
            _PackGemDatas.SaveClass(true);
        }
    }

    public void PacketSort()
    {

    }

    #endregion

    #region 

    public void SetGemAttr(RoleAttrStruct roleAttr)
    {
        for (int i = 0; i < EquipedGemDatas._PackItems.Count; ++i)
        {
            if (EquipedGemDatas._PackItems[i] == null)
                continue;

            if (!EquipedGemDatas._PackItems[i].IsVolid())
                continue;

            if (EquipedGemDatas._PackItems[i].GemAttr.AttrType == "RoleAttrImpactBaseAttr")
            {
                roleAttr.AddValue((RoleAttrEnum)EquipedGemDatas._PackItems[i].GemAttr.AttrParams[0], EquipedGemDatas._PackItems[i].GemAttr.AttrParams[1]);
            }
            else
            {
                roleAttr.AddExAttr(RoleAttrImpactManager.GetAttrImpact(EquipedGemDatas._PackItems[i].GemAttr));
            }
        }

        GemSuit.Instance.SetGemSetAttr(roleAttr);
       
    }

    public ItemGem GetGemClassMax(int classID, int minLevel)
    {
        ItemGem classItem = null;
        foreach (var gemData in PackGemDatas._PackItems)
        {
            if (gemData.IsVolid() && gemData.GemRecord.Class == classID && gemData.GemRecord.Level >= minLevel)
            {
                if (classItem == null)
                {
                    classItem = gemData;
                }
                else if (classItem.GemRecord.Level < gemData.GemRecord.Level)
                {
                    classItem = gemData;
                }
            }
        }

        foreach (var gemData in EquipedGemDatas._PackItems)
        {
            if (gemData.IsVolid() && gemData.GemRecord.Class == classID && gemData.GemRecord.Level >= minLevel)
            {
                if (classItem == null)
                {
                    classItem = gemData;
                }
                else if (classItem.GemRecord.Level < gemData.GemRecord.Level)
                {
                    classItem = gemData;
                }
            }
        }

        return classItem;
    }


    #endregion
}
