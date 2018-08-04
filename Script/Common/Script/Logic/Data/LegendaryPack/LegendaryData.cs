using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tables;

public class LegendaryData : SaveItemBase
{
    #region 唯一

    private static LegendaryData _Instance = null;
    public static LegendaryData Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new LegendaryData();
            }
            return _Instance;
        }
    }

    private LegendaryData()
    {
        _SaveFileName = "LegendaryData";
    }

    #endregion

    #region 

    [SaveField(1)]
    public List<ItemEquip> _LegendaryEquips = new List<ItemEquip>();

    public Dictionary<EquipItemRecord, ItemEquip> _LegendaryEquipDict = new Dictionary<EquipItemRecord, ItemEquip>();

    private List<EquipExAttr> _ExAttrs;
    public List<EquipExAttr> ExAttrs
    {
        get
        {
            if (_ExAttrs == null)
            {
                CalculateAttrs();
            }
            return _ExAttrs;
        }
    }

    private int _LegendaryValue = -1;
    public int LegendaryValue
    {
        get
        {
            if (_LegendaryValue < 0)
            {
                CalculateValue();
            }
            return _LegendaryValue;
        }
    }

    public int GetLegendatyCnt()
    {
        int cnt = 0;
        foreach (var equip in _LegendaryEquips)
        {
            if (equip.IsVolid())
            {
                ++cnt;
            }
        }

        return cnt;
    }

    public void InitLegendaryEquips()
    {
        var equipTabs = TableReader.EquipItem.Records;

        if (_LegendaryEquips == null || _LegendaryEquips.Count != equipTabs.Count)
        {
            if (_LegendaryEquips == null)
            {
                _LegendaryEquips = new List<ItemEquip>();
            }
            int startIdx = _LegendaryEquips.Count;
            for (int i = startIdx; i < equipTabs.Count; ++i)
            {
                var itemEquip = new ItemEquip("-1");
                _LegendaryEquips.Add(itemEquip);
            }

            SaveClass(true);
        }

        int idx = 0;
        foreach (var equipTab in equipTabs)
        {
            if (equipTab.Value.EquipClass != EQUIP_CLASS.Legendary)
                continue;

            _LegendaryEquipDict.Add(equipTab.Value, _LegendaryEquips[idx]);
            if (_LegendaryEquips[idx].IsVolid())
            {
                _LegendaryEquips[idx].CalculateCombatValue();
            }
            ++idx;
        }
    }

    public bool IsCollect(ItemEquip itemEquip)
    {
        var collectItem = _LegendaryEquipDict[itemEquip.EquipItemRecord];
        if (collectItem == null)
            return false;

        return collectItem.IsVolid();
    }

    public bool PutInEquip(ItemEquip equip)
    {
        var legendaryTab = TableReader.EquipItem.GetRecord(equip.ItemDataID);
        if (legendaryTab == null)
            return false;
        if (!_LegendaryEquipDict.ContainsKey(legendaryTab))
            return false;

        _LegendaryEquipDict[legendaryTab].ExchangeInfo(equip);
        CalculateAttrs();

        Hashtable hash = new Hashtable();
        hash.Add("EquipInfo", equip);
        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_STORE, this, hash);

        RoleData.SelectRole.CalculateAttr();

        return true;
    }

    public bool PutOffEquip(ItemEquip equip)
    {
        var emptyPos = BackBagPack.Instance.GetEmptyPageEquip();
        if (emptyPos == null)
            return false;

        emptyPos.ExchangeInfo(equip);
        CalculateAttrs();

        RoleData.SelectRole.CalculateAttr();

        Hashtable hash = new Hashtable();
        hash.Add("EquipInfo", equip);
        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_STORE, this, hash);

        RoleData.SelectRole.CalculateAttr();

        return true;
    }

    private void CalculateValue()
    {
        _LegendaryValue = 0;
        foreach (var equip in _LegendaryEquips)
        {
            _LegendaryValue += equip.EquipLevel;
        }
    }

    private void CalculateAttrs()
    {
        _ExAttrs = new List<EquipExAttr>();
        CalculateValue();

        _ExAttrs.Add(EquipExAttr.GetBaseExAttr(RoleAttrEnum.Attack, _LegendaryValue));
        _ExAttrs.Add(EquipExAttr.GetBaseExAttr(RoleAttrEnum.HPMax, _LegendaryValue));

        _ExAttrs.Add(new EquipExAttr("RoleAttrImpactPassiveShadowHit", 0, new int[] { 1, 1000}));
    }

    public void SetAttr(RoleAttrStruct roleAttr)
    {
        for (int i = 0; i < ExAttrs.Count; ++i)
        {
            if (ExAttrs[i].AttrType == "RoleAttrImpactBaseAttr")
            {
                roleAttr.AddValue((RoleAttrEnum)ExAttrs[i].AttrParams[0], ExAttrs[i].AttrParams[1]);
            }
            else
            {
                roleAttr.AddExAttr(RoleAttrImpactManager.GetAttrImpact(ExAttrs[i]));
            }
        }
    }

    public static bool IsEquipLegendary(ItemEquip equip)
    {
        return equip.EquipItemRecord.EquipClass == EQUIP_CLASS.Legendary;
    }

    #endregion
}
