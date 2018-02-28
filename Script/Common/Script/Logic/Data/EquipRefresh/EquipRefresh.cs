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
        if (itemEquip.EquipItemRecord.Slot == EQUIP_SLOT.WEAPON)
        {
            if (itemEquip.EquipLevel < 40)
            {
                costRecord._MatCnt = 3;
                costRecord._CostGold = 1000;
                costRecord._CostDiamond = 100;
            }
            else if (itemEquip.EquipLevel < 70)
            {
                costRecord._MatCnt = 3;
                costRecord._CostGold = 1000;
                costRecord._CostDiamond = 100;
            }
            else if (itemEquip.EquipLevel < 90)
            {
                costRecord._MatCnt = 3;
                costRecord._CostGold = 1000;
                costRecord._CostDiamond = 100;
            }
            else if (itemEquip.EquipLevel < 100)
            {
                costRecord._MatCnt = 3;
                costRecord._CostGold = 1000;
                costRecord._CostDiamond = 100;
            }
            else
            {
                costRecord._MatCnt = 3;
                costRecord._CostGold = 1000;
                costRecord._CostDiamond = 100;
            }
        }
        else if (itemEquip.EquipItemRecord.Slot == EQUIP_SLOT.TORSO
            || itemEquip.EquipItemRecord.Slot == EQUIP_SLOT.LEGS)
        {
            if (itemEquip.EquipLevel < 40)
            {
                costRecord._MatCnt = 3;
                costRecord._CostGold = 1000;
                costRecord._CostDiamond = 100;
            }
            else if (itemEquip.EquipLevel < 70)
            {
                costRecord._MatCnt = 3;
                costRecord._CostGold = 1000;
                costRecord._CostDiamond = 100;
            }
            else if (itemEquip.EquipLevel < 90)
            {
                costRecord._MatCnt = 3;
                costRecord._CostGold = 1000;
                costRecord._CostDiamond = 100;
            }
            else if (itemEquip.EquipLevel < 100)
            {
                costRecord._MatCnt = 3;
                costRecord._CostGold = 1000;
                costRecord._CostDiamond = 100;
            }
            else
            {
                costRecord._MatCnt = 3;
                costRecord._CostGold = 1000;
                costRecord._CostDiamond = 100;
            }
        }
        else if (itemEquip.EquipItemRecord.Slot == EQUIP_SLOT.AMULET
            || itemEquip.EquipItemRecord.Slot == EQUIP_SLOT.RING)
        {
            if (itemEquip.EquipLevel < 40)
            {
                costRecord._MatCnt = 3;
                costRecord._CostGold = 1000;
                costRecord._CostDiamond = 100;
            }
            else if (itemEquip.EquipLevel < 70)
            {
                costRecord._MatCnt = 3;
                costRecord._CostGold = 1000;
                costRecord._CostDiamond = 100;
            }
            else if (itemEquip.EquipLevel < 90)
            {
                costRecord._MatCnt = 3;
                costRecord._CostGold = 1000;
                costRecord._CostDiamond = 100;
            }
            else if (itemEquip.EquipLevel < 100)
            {
                costRecord._MatCnt = 3;
                costRecord._CostGold = 1000;
                costRecord._CostDiamond = 100;
            }
            else
            {
                costRecord._MatCnt = 3;
                costRecord._CostGold = 1000;
                costRecord._CostDiamond = 100;
            }
        }

        return costRecord;
    }

    public void EquipRefreshMat(ItemEquip itemEquip)
    {
        var refreshCost = GetEquipRefreshCost(itemEquip);

        var matCnt = BackBagPack.Instance.GetItemCnt(_RefreshMatDataID);
        if (matCnt < refreshCost._MatCnt)
        {
            UIMessageTip.ShowMessageTip(30003);
        }

        if (!PlayerDataPack.Instance.DecGold(refreshCost._CostGold))
        {
            return;
        }
        BackBagPack.Instance.DecItem(_RefreshMatDataID, refreshCost._MatCnt);

        RandomAttrs.LvUpEquipExAttr(itemEquip);
        itemEquip.EquipRefreshCostMatrial += refreshCost._MatCnt;

        Hashtable hash = new Hashtable();
        hash.Add("EquipInfo", itemEquip);
        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_REFRESH, this, hash);
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
        int destoryMatCnt = Mathf.CeilToInt(itemEquip.EquipRefreshCostMatrial * 0.8f);
        destoryMatCnt = Mathf.Max(destoryMatCnt, 1);
        if (itemEquip.EquipQuality == ITEM_QUALITY.PURPER)
        {
            destoryMatCnt += 10;
        }
        else if (itemEquip.EquipQuality == ITEM_QUALITY.ORIGIN)
        {
            destoryMatCnt += 50;
        }
        return destoryMatCnt;
    }

    public void DestoryMatCnt(ItemEquip itemEquip)
    {
        if (itemEquip.CommonItemRecord.Quality == ITEM_QUALITY.ORIGIN
            || itemEquip.CommonItemRecord.Quality == ITEM_QUALITY.PURPER)
        {
            UIMessageBox.Show(20003, () => { DestoryMatCntOk(itemEquip); }, null);
            return;
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

        while (idx1 < exchangeEquip1.EquipExAttr.Count && idx2 < exchangeEquip2.EquipExAttr.Count)
        {
            if (idx1 == 0 && idx2 == 0)
            {
                if (ItemEquip.IsAttrSpToEquip(exchangeEquip1.EquipExAttr[idx1]) && ItemEquip.IsAttrSpToEquip(exchangeEquip2.EquipExAttr[idx2]))
                {
                    ExChangeSingleAttr(exchangeEquip1, exchangeEquip1.EquipExAttr[idx1], exchangeEquip2, exchangeEquip2.EquipExAttr[idx2]);
                    ++idx1;
                    ++idx2;
                }
            }

            if (ItemEquip.IsAttrSpToEquip(exchangeEquip1.EquipExAttr[idx1]) || !ItemEquip.IsAttrBaseAttr(exchangeEquip1.EquipExAttr[idx1]))
            {
                ++idx1;
                continue;
            }

            if (ItemEquip.IsAttrSpToEquip(exchangeEquip2.EquipExAttr[idx2]) || !ItemEquip.IsAttrBaseAttr(exchangeEquip2.EquipExAttr[idx2]))
            {
                ++idx2;
                continue;
            }

            ExChangeSingleAttr(exchangeEquip1, exchangeEquip1.EquipExAttr[idx1], exchangeEquip2, exchangeEquip2.EquipExAttr[idx2]);
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
        int idx1 = exchangeEquip1.EquipExAttr.IndexOf(exAttr1);
        int idx2 = exchangeEquip2.EquipExAttr.IndexOf(exAttr2);

        exchangeEquip1.EquipExAttr.Insert(idx1, exAttr2);
        exchangeEquip2.EquipExAttr.Insert(idx2, exAttr1);

        exchangeEquip1.EquipExAttr.Remove(exAttr1);
        exchangeEquip2.EquipExAttr.Remove(exAttr2);
    }


    #endregion
}
