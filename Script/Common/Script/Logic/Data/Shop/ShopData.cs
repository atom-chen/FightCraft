using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tables;

public class ShopData : SaveItemBase
{
    #region shop refresh

    [SaveField(1)]
    public string _LastRefreshTimeStr;

    private DateTime _LastRefreshTime;
    public DateTime LastRefreshTime
    {
        get
        {
            if (_LastRefreshTime == null)
            {
                _LastRefreshTime = DateTime.Parse(_LastRefreshTimeStr);
            }
            return _LastRefreshTime;
        }
        set
        {
            _LastRefreshTime = value;
            _LastRefreshTimeStr = _LastRefreshTime.ToString();
        }
    }

    public static int _RefreshMinutes = 240;
    public void RefreshShop()
    {
        var timeDelay = DateTime.Now - LastRefreshTime;
        if (timeDelay.TotalMinutes > _RefreshMinutes)
        {
            //do refresh

            LastRefreshTime = DateTime.Now;
        }
    }

    #endregion

    #region equip shop

    [SaveField(2)]
    public List<ItemEquip> _EquipList;

    public static int _MaxRefreshEquipCnt = 20;

    public void RefreshEquip()
    {
        int maxRoleLv = 0;
        int maxRoleAttrLv = 0;
        foreach (var role in PlayerDataPack.Instance._RoleList)
        {
            if (role._RoleLevel > maxRoleLv)
            {
                maxRoleLv = role._RoleLevel;
            }

            if (role._AttrLevel > maxRoleAttrLv)
            {
                maxRoleAttrLv = role._AttrLevel;
            }
        }

        _EquipList.Clear();
        for (int i = 0; i < _MaxRefreshEquipCnt; ++i)
        {
            var equip = RandomEquip(maxRoleLv, maxRoleLv + maxRoleAttrLv);
            _EquipList.Add(equip);
        }
    }

    private ItemEquip RandomEquip(int level, int value)
    {
        int randomLevel = UnityEngine.Random.Range(level - 5, level + 5);
        int randomValue = UnityEngine.Random.Range((int)(value * 0.8f), (int)(value * 1.2f));
        int quality = GameRandom.GetRandomLevel(3,12,2);

        var equipItem = ItemEquip.CreateEquip(randomLevel, (ITEM_QUALITY)quality, randomValue, -1);
        return equipItem;
    }

    public void BuyEquip(int equipIdx)
    {
        if (_EquipList.Count < equipIdx)
            return;

        if (!_EquipList[equipIdx].IsVolid())
            return;

        var emptyPos = BackBagPack.Instance.GetEmptyPageEquip();
        if (emptyPos == null)
            return;

        if (!PlayerDataPack.Instance.DecGold(GetEuqipBuyPrice(_EquipList[equipIdx])))
            return;

        emptyPos.ExchangeInfo(_EquipList[equipIdx]);
    }

    public static int GetEquipSellPrice(ItemEquip equip)
    {
        return 1;
    }

    public static int GetEuqipBuyPrice(ItemEquip equip)
    {
        return 99;
    }
    #endregion

    #region item shop

    [SaveField(2)]
    public List<ItemBase> _ItemList;

    #endregion

    #region gambling

    #endregion

}
