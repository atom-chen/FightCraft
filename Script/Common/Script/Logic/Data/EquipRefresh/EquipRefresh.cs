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
    }

    public void EquipRefreshDiamond(ItemEquip itemEquip)
    {
        var refreshCost = GetEquipRefreshCost(itemEquip);

        if (!PlayerDataPack.Instance.DecDiamond(refreshCost._CostDiamond))
        {
            return;
        }

        RandomAttrs.LvUpEquipExAttr(itemEquip);
    }

    public void EquipRefreshFree(ItemEquip itemEquip)
    {
        if (_LastFreeTimes == 0)
        {
            UIMessageBox.Show(40000, WatchVideoForFreeRefresh, null);
            return;
        }

        --_LastFreeTimes;
        RandomAttrs.LvUpEquipExAttr(itemEquip);
    }

    public void WatchVideoForFreeRefresh()
    {
        WatchVideoCallBack();
    }

    public void WatchVideoCallBack()
    {
        ++_LastFreeTimes;
        UIEquipRefresh.Refresh();
    }

    #endregion

    #region equip destory

    public int GetDestoryMatCnt(ItemEquip itemEquip)
    {
        if (itemEquip.EquipQuality == ITEM_QUALITY.PURPER)
        {
            return 10;
        }
        else if (itemEquip.EquipQuality == ITEM_QUALITY.ORIGIN)
        {
            return 50;
        }
        return 0;
    }

    public void DestoryMatCnt(ItemEquip itemEquip)
    {
        int destoryGetCnt = GetDestoryMatCnt(itemEquip);
        itemEquip.ResetItem();
        BackBagPack.Instance.AddItem(_RefreshMatDataID, destoryGetCnt);
    }
    #endregion
}
