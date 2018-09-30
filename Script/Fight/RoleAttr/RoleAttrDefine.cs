﻿
using System.Collections;
using System.Collections.Generic;
using Tables;
using UnityEngine;

public enum RoleAttrEnum
{
    None,
    Strength = 1,
    Dexterity,
    Intelligence,
    Vitality,

    HPMax,
    HPMaxPersent,
    Attack,
    AttackPersent,
    Defense,
    DefensePersent,
    MoveSpeed,
    AttackSpeed,
    CriticalHitChance,
    CriticalHitDamge,

    FireAttackAdd,
    ColdAttackAdd,
    LightingAttackAdd,
    WindAttackAdd,

    FireResistan,
    ColdResistan,
    LightingResistan,
    WindResistan,

    FireEnhance,
    ColdEnhance,
    LightingEnhance,
    WindEnhance,
    PhysicDamageEnhance,

    IgnoreDefenceAttack,
    FinalDamageReduse,

    AllEleAtk,
    AllResistan,
    AllEnhance,

    ExGoldDrop,
    ExMatDrop,
    ExGemDrop,
    ExEquipDrop,

    FlyGravity,
    HitBack,
    RiseUpSpeed,

    BASE_ATTR_MAX,
}

public class RandomAttrs
{
    
    public static List<EquipExAttr> GetRandomEquipExAttrs(Tables.EQUIP_SLOT equipSlot, int level, int equipValue, Tables.ITEM_QUALITY quality, Tables.PROFESSION profession, EquipItemRecord legencyEquip)
    {
        List<EquipExAttr> exAttrs = new List<EquipExAttr>();

        switch (equipSlot)
        {
            case Tables.EQUIP_SLOT.WEAPON:
                return GetWeaponRandomAttr(level, equipValue, quality, profession, legencyEquip);
            case Tables.EQUIP_SLOT.TORSO:
                return GetTorseRandomAttr(level, equipValue, quality, profession, legencyEquip);
            case Tables.EQUIP_SLOT.LEGS:
                return GetLegsRandomAttr(level, equipValue, quality, profession, legencyEquip);
            case Tables.EQUIP_SLOT.AMULET:
                return GetAmuletRandomAttr(level, equipValue, quality, profession, legencyEquip);
            case Tables.EQUIP_SLOT.RING:
                return GetAmuletRandomAttr(level, equipValue, quality, profession, legencyEquip);
        }

        return exAttrs;
    }

    public static void LvUpEquipExAttr(ItemEquip itemEquip)
    {
        GameDataValue.LvUpEquipAttr(itemEquip);
        itemEquip.BakeExAttr();
        itemEquip.CalculateSet();

        RoleData.SelectRole.CalculateAttr();
    }
    #region weapon
    

    private static List<EquipExAttr> GetWeaponRandomAttr(int level, int equipValue, Tables.ITEM_QUALITY quality, Tables.PROFESSION profession, EquipItemRecord legencyEquip)
    {
        List<RoleAttrEnum> exAttrTypes = GameDataValue.GetRandomEquipAttrsType(Tables.EQUIP_SLOT.WEAPON, quality, legencyEquip);
        //if (quality == Tables.ITEM_QUALITY.BLUE)
        //{
        //    //exAttrTypes = GameDataValue.GetRandomEquipAttrsType(Tables.EQUIP_SLOT.WEAPON, quality);
        //}
        //else if (quality == Tables.ITEM_QUALITY.PURPER)
        //{
        //    exAttrTypes.Insert(0, RoleAttrEnum.AttackPersent);
        //}
        //else if (quality == Tables.ITEM_QUALITY.ORIGIN)
        //{
        //    exAttrTypes.Insert(0, RoleAttrEnum.AttackPersent);
        //}

        return CalculateExAttr(exAttrTypes, equipValue);
    }

    private static List<EquipExAttr> CalculateExAttr(List<RoleAttrEnum> exAttrTypes, int equipValue)
    {
        List<EquipExAttr> exAttrs = new List<EquipExAttr>();

        for (int i = 0; i < exAttrTypes.Count; ++i)
        {
            var equipExAttr = new EquipExAttr();
            equipExAttr.AttrType = "RoleAttrImpactBaseAttr";
            equipExAttr.Value = GameDataValue.GetExAttrRandomValue(exAttrTypes[i], equipValue);
            equipExAttr.AttrParams.Add((int)exAttrTypes[i]);
            equipExAttr.AttrParams.Add(GameDataValue.GetValueAttr(exAttrTypes[i], equipExAttr.Value));
            exAttrs.Add(equipExAttr);
        }

        return exAttrs;
    }

    #endregion

    #region torse & legs

    private static List<EquipExAttr> GetTorseRandomAttr(int level, int equipValue, Tables.ITEM_QUALITY quality, Tables.PROFESSION profession, EquipItemRecord legencyEquip)
    {
        List<RoleAttrEnum> exAttrTypes = GameDataValue.GetRandomEquipAttrsType(Tables.EQUIP_SLOT.TORSO, quality, legencyEquip);
        //if (quality == Tables.ITEM_QUALITY.BLUE)
        //{
        //    //exAttrTypes = GameDataValue.GetRandomEquipAttrsType(Tables.EQUIP_SLOT.WEAPON, quality);
        //}
        //else if (quality == Tables.ITEM_QUALITY.PURPER)
        //{
        //    exAttrTypes.Insert(0, RoleAttrEnum.HPMaxPersent);
        //}
        //else if (quality == Tables.ITEM_QUALITY.ORIGIN)
        //{
        //    exAttrTypes.Insert(0, RoleAttrEnum.HPMaxPersent);
        //}

        return CalculateExAttr(exAttrTypes, equipValue);
    }

    private static List<EquipExAttr> GetLegsRandomAttr(int level, int equipValue, Tables.ITEM_QUALITY quality, Tables.PROFESSION profession, EquipItemRecord legencyEquip)
    {
        List<RoleAttrEnum> exAttrTypes = GameDataValue.GetRandomEquipAttrsType(Tables.EQUIP_SLOT.LEGS, quality, legencyEquip);
        //if (quality == Tables.ITEM_QUALITY.BLUE)
        //{
        //    //exAttrTypes = GameDataValue.GetRandomEquipAttrsType(Tables.EQUIP_SLOT.WEAPON, quality);
        //}
        //else if (quality == Tables.ITEM_QUALITY.PURPER)
        //{
        //    exAttrTypes.Insert(0, RoleAttrEnum.MoveSpeed);
        //}
        //else if (quality == Tables.ITEM_QUALITY.ORIGIN)
        //{
        //    exAttrTypes.Insert(0, RoleAttrEnum.MoveSpeed);
        //}

        return CalculateExAttr(exAttrTypes, equipValue);
    }

    #endregion

    #region amulet & ring
    
    private static List<EquipExAttr> GetAmuletRandomAttr(int level, int equipValue, Tables.ITEM_QUALITY quality, Tables.PROFESSION profession, EquipItemRecord legencyEquip)
    {
        List<RoleAttrEnum> exAttrTypes = GameDataValue.GetRandomEquipAttrsType(Tables.EQUIP_SLOT.RING, quality, legencyEquip);

        return CalculateExAttr(exAttrTypes, equipValue);
    }

    #endregion

    #region attr show

    public static string GetAttrName(RoleAttrEnum attr)
    {
        return StrDictionary.GetFormatStr((int)attr);
    }

    public static string GetAttrValueShow(RoleAttrEnum attr, int value)
    {
        string valueStr = value.ToString();
        switch ((RoleAttrEnum)attr)
        {
            case RoleAttrEnum.AttackPersent:
            case RoleAttrEnum.HPMaxPersent:
            case RoleAttrEnum.MoveSpeed:
            case RoleAttrEnum.AttackSpeed:
            case RoleAttrEnum.CriticalHitChance:
            case RoleAttrEnum.PhysicDamageEnhance:
                var persentVal = (value) * 0.01f;
                valueStr = string.Format("{0:0.00}", persentVal);
                valueStr += "%";
                break;
            case RoleAttrEnum.ExEquipDrop:
            case RoleAttrEnum.ExGemDrop:
            case RoleAttrEnum.ExGoldDrop:
            case RoleAttrEnum.ExMatDrop:
                var persentVal2 = (value) * 0.01f;
                valueStr = string.Format("{0:0.00}", persentVal2);
                break;
        }

        return valueStr;
    }

    #endregion
}

public class RoleAttrStruct
{
    public List<int> _BaseAttrValues;
    public List<RoleAttrImpactBase> _ExAttr;

    public RoleAttrStruct()
    {
        _BaseAttrValues = new List<int>();
        for (int i = 0; i < (int)RoleAttrEnum.BASE_ATTR_MAX; ++i)
        {
            _BaseAttrValues.Add(0);
        }
        _ExAttr = new List<RoleAttrImpactBase>();
    }

    public RoleAttrStruct(RoleAttrStruct copyStruct)
    {
        _BaseAttrValues = new List<int>(copyStruct._BaseAttrValues);
        _ExAttr = new List<RoleAttrImpactBase>(copyStruct._ExAttr);
    }

    public void ResetBaseAttr()
    {
        for(int i = 0; i< _BaseAttrValues.Count; ++i)
        {
            _BaseAttrValues[i] = 0;
        }
        _ExAttr.Clear();
    }

    public void SetValue(RoleAttrEnum attrEnum, int value)
    {
        _BaseAttrValues[(int)attrEnum] = value;
    }

    public void AddValue(RoleAttrEnum attrEnum, int value)
    {
        _BaseAttrValues[(int)attrEnum] += value;
    }

    public int GetValue(RoleAttrEnum attrEnum)
    {
        return _BaseAttrValues[(int)attrEnum];
    }

    //public int GetExAttr(RoleAttrEnum attrEnum)
    //{
    //    if (_ExAttr.ContainsKey(attrEnum))
    //        return _ExAttr[attrEnum].AttrValue1;

    //    return 0;
    //}

    public void AddExAttr(RoleAttrImpactBase exAttr)
    {
        RoleAttrImpactBase existAttr = null;
        for (int i = 0; i < _ExAttr.Count; ++i)
        {
            if (_ExAttr[i].ToString() == exAttr.ToString() && _ExAttr[i]._SkillInput == exAttr._SkillInput)
            {
                existAttr = _ExAttr[i];
            }
        }

        if (existAttr == null)
        {
            _ExAttr.Add(exAttr);
        }
        else
        {
            //existAttr.AddData(exAttr);
        }
        
    }
}