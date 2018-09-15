using System.Collections;
using System.Collections.Generic;
using Tables;
using UnityEngine;

public class EquipRefreshCost
{
    public int _MatCnt;
    public int _CostGold;
    public int _CostDiamond;
}

public class EquipRefresh : DataPackBase
{
    #region 单例

    private static EquipRefresh _Instance;
    public static EquipRefresh Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new EquipRefresh();
            }
            return _Instance;
        }
    }

    private EquipRefresh()
    {
        _SaveFileName = "EquipRefresh";
    }

    #endregion

    #region equip refresh

    [SaveField(1)]
    private int _LastFreeTimes = 0;
    public int LastFreeTimes
    {
        get
        {
            return _LastFreeTimes;
        }
    }

    public void InitEquipRefresh()
    {
        if (_LastFreeTimes < 0)
        {
            _LastFreeTimes = 0;
        }
    }

    public static string _RefreshMatDataID = "20000";

    public EquipRefreshCost GetEquipRefreshCost(ItemEquip itemEquip)
    {
        EquipRefreshCost costRecord = new EquipRefreshCost();
        costRecord._MatCnt = GameDataValue.GetEquipLvUpConsume(itemEquip);
        costRecord._CostDiamond = GameDataValue.GetEquipLvUpConsumeDiamond(itemEquip);
       
        return costRecord;
    }

    public bool EquipRefreshMat(ItemEquip itemEquip, bool needTip = true)
    {
        if (itemEquip.EquipQuality == ITEM_QUALITY.WHITE)
            return false;

        var refreshCost = GetEquipRefreshCost(itemEquip);

        var matCnt = BackBagPack.Instance.GetItemCnt(_RefreshMatDataID);
        if ( matCnt < refreshCost._MatCnt)
        {
            if (needTip)
            {
                UIMessageTip.ShowMessageTip(30003);
            }
            return false;
        }

        if (!PlayerDataPack.Instance.DecGold(refreshCost._CostGold))
        {
            return false;
        }
        BackBagPack.Instance.DecItem(_RefreshMatDataID, refreshCost._MatCnt);

        RandomAttrs.LvUpEquipExAttr(itemEquip);
        itemEquip.EquipRefreshCostMatrial += refreshCost._MatCnt;

        Hashtable hash = new Hashtable();
        hash.Add("EquipInfo", itemEquip);
        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_REFRESH, this, hash);

        return true;
    }

    public void EquipRefreshDiamond(ItemEquip itemEquip)
    {
        var refreshCost = GetEquipRefreshCost(itemEquip);

        if (!PlayerDataPack.Instance.DecDiamond(refreshCost._CostDiamond))
        {
            return;
        }

        RandomAttrs.LvUpEquipExAttr(itemEquip);
        itemEquip.EquipRefreshCostMatrial += refreshCost._MatCnt;

        Hashtable hash = new Hashtable();
        hash.Add("EquipInfo", itemEquip);
        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_REFRESH, this, hash);
    }

    public void EquipRefreshFree(ItemEquip itemEquip)
    {
        var refreshCost = GetEquipRefreshCost(itemEquip);
        if (_LastFreeTimes == 0)
        {
            UIMessageBox.Show(40000, WatchVideoForFreeRefresh, null);
            return;
        }

        --_LastFreeTimes;
        RandomAttrs.LvUpEquipExAttr(itemEquip);
        itemEquip.EquipRefreshCostMatrial += refreshCost._MatCnt;

        SaveClass(false);

        Hashtable hash = new Hashtable();
        hash.Add("EquipInfo", itemEquip);
        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_REFRESH, this, hash);
    }

    public void WatchVideoForFreeRefresh()
    {
        WatchVideoCallBack();
    }

    public void WatchVideoCallBack()
    {
        ++_LastFreeTimes;
        UIEquipRefresh.Refresh();

        SaveClass(false);
    }

    #endregion

    #region equip destory

    public int GetDestoryMatCnt(ItemEquip itemEquip)
    {
        if (itemEquip.EquipLevel < GameDataValue._DropMatLevel)
            return 0;

        int destoryMatCnt = GameDataValue.GetDestoryGetMatCnt(itemEquip);
        return destoryMatCnt;
    }

    public void DestoryMatCnt(ItemEquip itemEquip, bool needEnsure = true)
    {
        if (needEnsure)
        {
            if (itemEquip.CommonItemRecord.Quality == ITEM_QUALITY.ORIGIN
                || itemEquip.CommonItemRecord.Quality == ITEM_QUALITY.PURPER)
            {
                UIMessageBox.Show(20003, () => { DestoryMatCntOk(itemEquip); }, null);
                return;
            }
        }
        DestoryMatCntOk(itemEquip);
    }

    public void DestoryMatCntOk(ItemEquip itemEquip)
    {
        int destoryGetCnt = GetDestoryMatCnt(itemEquip);
        itemEquip.ResetItem();
        itemEquip.SaveClass(true);
        BackBagPack.Instance.AddItem(_RefreshMatDataID, destoryGetCnt);

        Hashtable hash = new Hashtable();
        hash.Add("EquipInfo", itemEquip);
        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_DESTORY, this, hash);
    }
    #endregion

    #region equip exchange

    public void ExchangeExAttr(ItemEquip exchangeEquip1, ItemEquip exchangeEquip2)
    {
        int idx1 = 0;
        int idx2 = 0;

        while (idx1 < exchangeEquip1.EquipExAttrs.Count && idx2 < exchangeEquip2.EquipExAttrs.Count)
        {
            if (idx1 == 0 && idx2 == 0)
            {
                if (ItemEquip.IsAttrSpToEquip(exchangeEquip1.EquipExAttrs[idx1]) && ItemEquip.IsAttrSpToEquip(exchangeEquip2.EquipExAttrs[idx2]))
                {
                    ExChangeSingleAttr(exchangeEquip1, exchangeEquip1.EquipExAttrs[idx1], exchangeEquip2, exchangeEquip2.EquipExAttrs[idx2]);
                    ++idx1;
                    ++idx2;
                }
            }

            if (ItemEquip.IsAttrSpToEquip(exchangeEquip1.EquipExAttrs[idx1]) || !ItemEquip.IsAttrBaseAttr(exchangeEquip1.EquipExAttrs[idx1]))
            {
                ++idx1;
                continue;
            }

            if (ItemEquip.IsAttrSpToEquip(exchangeEquip2.EquipExAttrs[idx2]) || !ItemEquip.IsAttrBaseAttr(exchangeEquip2.EquipExAttrs[idx2]))
            {
                ++idx2;
                continue;
            }

            ExChangeSingleAttr(exchangeEquip1, exchangeEquip1.EquipExAttrs[idx1], exchangeEquip2, exchangeEquip2.EquipExAttrs[idx2]);
            ++idx1;
            ++idx2;
        }

        int tempRefreshValue = exchangeEquip1.EquipRefreshCostMatrial;
        exchangeEquip1.EquipRefreshCostMatrial = exchangeEquip2.EquipRefreshCostMatrial;
        exchangeEquip2.EquipRefreshCostMatrial = tempRefreshValue;

        exchangeEquip1.BakeExAttr();
        exchangeEquip2.BakeExAttr();
    }

    

    private void ExChangeSingleAttr(ItemEquip exchangeEquip1, EquipExAttr exAttr1, ItemEquip exchangeEquip2, EquipExAttr exAttr2)
    {
        int idx1 = exchangeEquip1.EquipExAttrs.IndexOf(exAttr1);
        int idx2 = exchangeEquip2.EquipExAttrs.IndexOf(exAttr2);

        exchangeEquip1.EquipExAttrs.Insert(idx1, exAttr2);
        exchangeEquip2.EquipExAttrs.Insert(idx2, exAttr1);

        exchangeEquip1.EquipExAttrs.Remove(exAttr1);
        exchangeEquip2.EquipExAttrs.Remove(exAttr2);
    }


    #endregion
}
