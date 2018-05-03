﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tables;

public class GameDataValue
{
    public static float ConfigIntToFloat(int val)
    {
        var resultVal = new decimal(0.0001) * new decimal(val);
        return (float)resultVal;
    }

    public static float ConfigIntToFloatDex1(int val)
    {
        int dex = Mathf.CeilToInt(val * 0.1f);
        var resultVal = new decimal(0.001) * new decimal(dex);
        return (float)resultVal;
    }

    public static float ConfigIntToPersent(int val)
    {
        
        var resultVal = new decimal(0.01) * new decimal(val);
        return (float)resultVal;
    }

    public static int ConfigFloatToInt(float val)
    {
        return (int)(val * 10000);
    }

    public static int ConfigFloatToPersent(float val)
    {
        return (int)(val * 100);
    }

    public static int GetMaxRate()
    {
        return 10000;
    }

    #region fight numaric

    #region level -> baseAttr

    private static int _MaxLv = 100;
    private static float _AttackPerLevel = 136.0f;
    private static float _AttackIncreaseLevel = 1.2f;
    private static float _HPPerLevel = 286.0f;
    private static float _DefencePerLevel = 68.0f;
    private static float _ValuePerLevel = 50;
    private static float _ValuePerAttack = 1;

    private static float _LegHPToTorso = 0.5f;
    private static float _LegDefenceToTorso = 0.5f;

    public static int CalWeaponAttack(int equiplevel)
    {
        int power = equiplevel / 5;
        var attackValue = _AttackPerLevel* Mathf.Pow(_AttackIncreaseLevel, power);
        return Mathf.CeilToInt(attackValue);
    }

    public static int CalEquipTorsoHP(int equiplevel)
    {
        int power = equiplevel / 5;
        var value = _HPPerLevel * Mathf.Pow(_AttackIncreaseLevel, power);
        return Mathf.CeilToInt(value);
    }

    public static int CalEquipTorsoDefence(int equiplevel)
    {
        int power = equiplevel / 5;
        var value = _DefencePerLevel * Mathf.Pow(_AttackIncreaseLevel, power);
        return Mathf.CeilToInt(value);
    }

    public static int CalEquipLegsHP(int equiplevel)
    {
        var value = CalEquipTorsoHP(equiplevel) * 0.5f;
        return Mathf.CeilToInt(value);
    }

    public static int CalEquipLegsDefence(int equiplevel)
    {
        var value = CalEquipTorsoDefence(equiplevel) * 0.5f;
        return Mathf.CeilToInt(value);
    }

    #endregion

    #region baseAttr -> exAttr atk

    public static int _AtkPerRoleLevel = 1;
    public static int _AtkRoleLevelBase = 10;
    public static int _HPPerRoleLevel = 100;
    public static int _HPRoleLevelBase = 5000;

    public static float _AttackPerStrength = 0.25f;
    public static float _DmgEnhancePerStrength = 0.5f;
    public static float _StrToAtk = 1;

    public static float _IgnoreAtkPerDex = 0.15f;
    public static float _CriticalRatePerDex = 0.1f;
    public static float _CriticalDmgPerDex = 1.5f;
    public static float _DexToAtk = 1;

    public static float _EleAtkPerInt = 0.1f;
    public static float _EleEnhancePerInt = 0.1f;
    public static float _IntToAtk = 1;

    public static float _HPPerVit = 3;
    public static float _FinalDmgRedusePerVit = 0.3f;
    public static float _VitToAtk = 1;

    public static float _CriticalDmgToAtk = 2f;

    public static float _ElementToAtk = 1;
    public static float _DmgEnhancePerElementEnhance = 10;
    public static float _EleEnhanceToAtk = 0.3f;
    public static float _EleResistToAtk = 0.3f;

    public static float _IgnoreDefenceToAtk = 0.5f;

    public static float _HpToAtk = 11.0f;
    public static float _DefToAtk = 0.5f;
    public static float _MoveSpeedToAtk = 1.35f;
    public static float _AtkSpeedToAtk = 1;
    public static float _CriticalChanceToAtk = 1;
    public static float _DamageEnhance = 1;

    public static float GetAttrToValue(RoleAttrEnum roleAttr)
    {
        float value = 0.0f;
        switch (roleAttr)
        {
            case RoleAttrEnum.Strength:
                value = _StrToAtk;
                break;
            case RoleAttrEnum.Dexterity:
                value = _DexToAtk;
                break;
            case RoleAttrEnum.Intelligence:
                value = _IntToAtk;
                break;
            case RoleAttrEnum.Vitality:
                value = _VitToAtk;
                break;
            case RoleAttrEnum.Attack:
                value = 1;
                break;
            case RoleAttrEnum.HPMax:
                value = _HpToAtk;
                break;
            case RoleAttrEnum.Defense:
                value = _DefToAtk;
                break;
            case RoleAttrEnum.MoveSpeed:
                value = _MoveSpeedToAtk;
                break;
            case RoleAttrEnum.AttackSpeed:
                value = _AtkSpeedToAtk;
                break;
            case RoleAttrEnum.CriticalHitChance:
                value = _CriticalChanceToAtk;
                break;
            case RoleAttrEnum.PhysicDamageEnhance:
                value = _DamageEnhance;
                break;
            case RoleAttrEnum.CriticalHitDamge:
                value = _CriticalDmgToAtk;
                break;
            case RoleAttrEnum.FireAttackAdd:
                value = _ElementToAtk;
                break;
            case RoleAttrEnum.ColdAttackAdd:
                value = _ElementToAtk;
                break;
            case RoleAttrEnum.LightingAttackAdd:
                value = _ElementToAtk;
                break;
            case RoleAttrEnum.WindAttackAdd:
                value = _ElementToAtk;
                break;
            case RoleAttrEnum.FireEnhance:
                value = _EleEnhanceToAtk;
                break;
            case RoleAttrEnum.ColdEnhance:
                value = _EleEnhanceToAtk;
                break;
            case RoleAttrEnum.LightingEnhance:
                value = _EleEnhanceToAtk;
                break;
            case RoleAttrEnum.WindEnhance:
                value = _EleEnhanceToAtk;
                break;
            case RoleAttrEnum.FireResistan:
                value = _EleResistToAtk;
                break;
            case RoleAttrEnum.ColdResistan:
                value = _EleResistToAtk;
                break;
            case RoleAttrEnum.LightingResistan:
                value = _EleResistToAtk;
                break;
            case RoleAttrEnum.WindResistan:
                value = _EleResistToAtk;
                break;
            case RoleAttrEnum.IgnoreDefenceAttack:
                value = _IgnoreDefenceToAtk;
                break;
        }

        return value;
    }

    public static int GetValueAttr(RoleAttrEnum roleAttr, int value)
    {
        int attrValue = 1;
        if (roleAttr == RoleAttrEnum.AttackPersent)
        {
            attrValue = Mathf.Clamp(value, 1, 10000);
        }
        else if (roleAttr == RoleAttrEnum.HPMaxPersent)
        {
            attrValue = Mathf.Clamp(value, 1, 10000);
        }
        else
        {
            attrValue = Mathf.CeilToInt(value * GetAttrToValue(roleAttr));
            if (roleAttr == RoleAttrEnum.AttackSpeed)
            {
                attrValue = Mathf.Clamp(attrValue, 1, 1000);
            }
            else if (roleAttr == RoleAttrEnum.MoveSpeed)
            {
                attrValue = Mathf.Clamp(attrValue, 1, 1500);
            }
            else if (roleAttr == RoleAttrEnum.CriticalHitChance)
            {
                attrValue = Mathf.Clamp(attrValue, 1, 1000);
            }
            else
            {
                attrValue = Mathf.Max(attrValue, 1);
            }
        }
        return attrValue;
    }

    public static int GetAttrValue(RoleAttrEnum roleAttr, int value)
    {
        int attrValue = 1;
        var attrToValue = GetAttrToValue(roleAttr);
        if (attrToValue == 0)
        {
            attrValue = 0;
        }
        else
        {
            attrValue = (int)(value / attrToValue);
        }

        return attrValue;
    }
    #endregion

    #region ex -> base

    private static float _ExToBase = 0.2f;
    private static float _LvValueBase = 50;
    private static float _LvValueV = 10;
    private static float _LvValueA = 0.8f;

    public static int CalLvValue(int level)
    {
        var exValue = _LvValueV * level + level * level * 0.5f * _LvValueA + _LvValueBase;
        return Mathf.CeilToInt(exValue);
    }

    public static int CalExValue(int level)
    {
        var exValue = CalLvValue(level) * _ExToBase;
        return Mathf.CeilToInt(exValue);
    }

    public static int GetMaxValue()
    {
        return Mathf.CeilToInt( CalExValue(_MaxLv) * 1.2f);
    }
    #endregion

    #region equip

    public static int GetExAttrRandomValue(RoleAttrEnum roleAttr, int baseValue, float lowPersent = 0.2f, float upPersent = 0.3f)
    {
        var randomValue = Random.Range(lowPersent, upPersent);
        if (roleAttr == RoleAttrEnum.AttackPersent)
        {
            return Mathf.CeilToInt(10000 * randomValue);
        }
        else if (roleAttr == RoleAttrEnum.HPMaxPersent)
        {
            return Mathf.CeilToInt(10000 * randomValue);
        }
        //else if (roleAttr == RoleAttrEnum.MoveSpeed)
        //{
        //    int value = 300 + Mathf.CeilToInt( randomValue * baseValue);
        //    return Mathf.Clamp(value, 300, 1500);
        //}
        //else if (roleAttr == RoleAttrEnum.AttackSpeed)
        //{
        //    int value = Mathf.CeilToInt(randomValue * baseValue);
        //    return Mathf.Clamp(value, 300, 1000);
        //}
        //else if (roleAttr == RoleAttrEnum.CriticalHitChance)
        //{
        //    int value = Mathf.CeilToInt(randomValue * baseValue);
        //    return Mathf.Clamp(value, 300, 1000);
        //}
        else
        {
            return Mathf.CeilToInt(baseValue * randomValue * _ExToBase);
        }
    }

    public static List<RoleAttrEnum> GetRandomEquipAttrsType(Tables.EQUIP_SLOT equipSlot, Tables.ITEM_QUALITY quality, int fixNum = 0)
    {
        List<EquipExAttr> exAttrs = new List<EquipExAttr>();

        int exAttrCnt = 0;
        if (quality == ITEM_QUALITY.WHITE)
            return new List<RoleAttrEnum>();

        if (fixNum == 0)
        {
            switch (equipSlot)
            {
                case Tables.EQUIP_SLOT.WEAPON:
                    exAttrCnt = 2;
                    if (quality == Tables.ITEM_QUALITY.BLUE)
                    {
                        exAttrCnt = Random.Range(1, 3);
                    }
                    return CalRandomAttrs(_WeaponExAttrs, exAttrCnt);
                case Tables.EQUIP_SLOT.TORSO:
                    exAttrCnt = 2;
                    if (quality == Tables.ITEM_QUALITY.BLUE)
                    {
                        exAttrCnt = Random.Range(1, 3);
                    }
                    return CalRandomAttrs(_DefenceExAttrs, exAttrCnt);
                case Tables.EQUIP_SLOT.LEGS:
                    exAttrCnt = 2;
                    if (quality == Tables.ITEM_QUALITY.BLUE)
                    {
                        exAttrCnt = Random.Range(1, 3);
                    }
                    return CalRandomAttrs(_DefenceExAttrs, exAttrCnt);
                case Tables.EQUIP_SLOT.AMULET:
                    exAttrCnt = 3;
                    if (quality == Tables.ITEM_QUALITY.BLUE)
                    {
                        exAttrCnt = Random.Range(1, 3);
                    }
                    return CalRandomAttrs(_AmuletExAttrs, exAttrCnt);
                case Tables.EQUIP_SLOT.RING:
                    exAttrCnt = 3;
                    if (quality == Tables.ITEM_QUALITY.BLUE)
                    {
                        exAttrCnt = Random.Range(1, 3);
                    }
                    return CalRandomAttrs(_AmuletExAttrs, exAttrCnt);
            }
        }
        else
        {
            return CalRandomAttrs(_AmuletExAttrs, fixNum);
        }
        return null;
    }

    public static List<RoleAttrEnum> CalRandomAttrs(List<EquipExAttrRandom> staticList, int randomCnt)
    {
        List<EquipExAttrRandom> randomList = new List<EquipExAttrRandom>(staticList);
        int totalRandom = 0;
        foreach (var attrRandom in randomList)
        {
            totalRandom += (attrRandom.Random);
        }

        List<RoleAttrEnum> attrList = new List<RoleAttrEnum>();
        for (int i = 0; i < randomCnt; ++i)
        {
            int temp = totalRandom;
            int randomVar = Random.Range(0, temp);
            EquipExAttrRandom attr = null;
            foreach (var attrRandom in randomList)
            {
                temp -= attrRandom.Random;
                if (randomVar >= temp)
                {
                    attr = attrRandom;
                    break;
                }
            }
            if (attr == null)
            {
                attr = randomList[randomList.Count - 1];
            }

            attrList.Add(attr.AttrID);
            if (!attr.CanRepeat)
            {
                totalRandom -= attr.Random;
                randomList.Remove(attr);
            }
        }
        return attrList;
    }

    public static void LvUpEquipAttr(ItemEquip itemEquip)
    {
        List<EquipExAttr> valueAttrs = new List<EquipExAttr>();
        int curTotalValue = 0;
        int singleValueMax = Mathf.CeilToInt(itemEquip.EquipValue * _ExToBase);

        foreach (var equipExAttr in itemEquip.EquipExAttr)
        {
            if (equipExAttr.AttrType != "RoleAttrImpactBaseAttr")
                continue;

            if (equipExAttr.AttrParams[0] == (int)RoleAttrEnum.AttackPersent || equipExAttr.AttrParams[0] == (int)RoleAttrEnum.HPMaxPersent)
            {
                var randomPersent = Random.Range(20, 30);
                equipExAttr.Value = Mathf.Min(10000, equipExAttr.Value + randomPersent);
                equipExAttr.AttrParams[1] = GetValueAttr((RoleAttrEnum)equipExAttr.AttrParams[0], equipExAttr.Value);
            }
            else if (equipExAttr.AttrParams[0] == (int)RoleAttrEnum.MoveSpeed)
            {
                var randomPersent = Random.Range(20, 30);
                var incValue = (int)Mathf.Max(1, singleValueMax * ConfigIntToFloat(randomPersent));
                equipExAttr.Value = Mathf.Min(singleValueMax, equipExAttr.Value + incValue);
                equipExAttr.AttrParams[1] = GetValueAttr((RoleAttrEnum)equipExAttr.AttrParams[0], equipExAttr.Value);
            }
            else
            {
                valueAttrs.Add(equipExAttr);
                curTotalValue += equipExAttr.Value;
            }
        }

        int totalValue = valueAttrs.Count * singleValueMax;

        int randomRate = Random.Range(100, 200);
        int deltaValue = (totalValue - curTotalValue);
        int increaseValue = Mathf.CeilToInt(deltaValue * ConfigIntToFloat(randomRate));
        increaseValue = Mathf.Max(increaseValue, Mathf.CeilToInt(deltaValue / 100 + 1));

        var attrEnums = GetRandomEquipAttrsType(itemEquip.EquipItemRecord.Slot, itemEquip.EquipQuality, valueAttrs.Count);
        for (int i = 0; i < valueAttrs.Count; ++i)
        {
            int randomIncValue = Random.Range(0, increaseValue);
            if(i == valueAttrs.Count - 1)
            {
                randomIncValue = increaseValue;
                
            }
            int attrValueToMax = singleValueMax - valueAttrs[i].Value;
            if (attrValueToMax < randomIncValue)
            {
                randomIncValue = attrValueToMax;
            }
            valueAttrs[i].AttrParams[0] = (int)attrEnums[i];
            valueAttrs[i].Value += randomIncValue;
            valueAttrs[i].AttrParams[1] = GetValueAttr(attrEnums[i], valueAttrs[i].Value);
            increaseValue -= randomIncValue;
        }
    }

    public static float GetExAttrPersent(ItemEquip itemEquip, EquipExAttr exAttr)
    {
        if (exAttr.AttrType != "RoleAttrImpactBaseAttr")
            return 1;

        if (ItemEquip.IsAttrSpToEquip(exAttr))
        {
            return ConfigIntToFloat(exAttr.Value);
        }

        int singleValueMax = Mathf.CeilToInt(itemEquip.EquipValue * _ExToBase);
        return (float)exAttr.Value / singleValueMax;
    }

    public class EquipExAttrRandom
    {
        public RoleAttrEnum AttrID;
        public bool CanRepeat;
        public int MinValue;
        public int Random;

        public EquipExAttrRandom(RoleAttrEnum attr, bool repeat, int maxValue, int randomVal)
        {
            AttrID = attr;
            CanRepeat = repeat;
            MinValue = maxValue;
            Random = randomVal;
        }
    }

    public static List<EquipExAttrRandom> _WeaponExAttrs = new List<EquipExAttrRandom>()
    {
        new EquipExAttrRandom(RoleAttrEnum.Strength, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.Dexterity, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.Intelligence, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.Vitality, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.HPMax, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.Attack, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.Defense, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.AttackSpeed, false, 300, 100),
        new EquipExAttrRandom(RoleAttrEnum.CriticalHitChance, false, 300, 100),
        new EquipExAttrRandom(RoleAttrEnum.CriticalHitDamge, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.FireAttackAdd, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.ColdAttackAdd, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.LightingAttackAdd, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.WindAttackAdd, true, -1, 10),
        new EquipExAttrRandom(RoleAttrEnum.FireResistan, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.ColdResistan, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.LightingResistan, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.WindResistan, true, -1, 100),
    };

    public static List<EquipExAttrRandom> _DefenceExAttrs = new List<EquipExAttrRandom>()
    {
        new EquipExAttrRandom(RoleAttrEnum.Strength, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.Dexterity, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.Intelligence, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.Vitality, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.HPMax, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.Attack, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.Defense, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.CriticalHitDamge, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.FireAttackAdd, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.ColdAttackAdd, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.LightingAttackAdd, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.WindAttackAdd, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.FireResistan, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.ColdResistan, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.LightingResistan, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.WindResistan, true, -1, 100),
    };

    public static List<EquipExAttrRandom> _AmuletExAttrs = new List<EquipExAttrRandom>()
    {
        new EquipExAttrRandom(RoleAttrEnum.Strength, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.Dexterity, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.Intelligence, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.Vitality, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.HPMax, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.Attack, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.Defense, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.AttackSpeed, false, 300, 100),
        new EquipExAttrRandom(RoleAttrEnum.CriticalHitChance, false, 300, 100),
        new EquipExAttrRandom(RoleAttrEnum.CriticalHitDamge, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.FireAttackAdd, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.ColdAttackAdd, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.LightingAttackAdd, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.WindAttackAdd, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.FireResistan, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.ColdResistan, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.LightingResistan, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.WindResistan, true, -1, 100),
    };

    #endregion

    #region equip set

    

    #endregion

    #region gem

    public static float _GemAttrV = 10;
    public static float _GemAttrA = 5;

    public static int GetGemValue(int level)
    {
        int value = Mathf.CeilToInt(level * _GemAttrV + 0.5f * level * level * _GemAttrA);
        return value;
    }

    public static EquipExAttr GetGemAttr(RoleAttrEnum attr, int level)
    {
        int value = Mathf.CeilToInt( level * _GemAttrV + 0.5f * level * level * _GemAttrA);
        EquipExAttr exAttr = EquipExAttr.GetBaseExAttr(attr, value);
        return exAttr;
    }

    public static List<EquipExAttr> GetGemSetAttr(Tables.GemSetRecord gemSet, int level)
    {
        List<EquipExAttr> attrList = new List<EquipExAttr>();
        foreach (var attrValue in gemSet.Attrs)
        {
            if (attrValue == null || string.IsNullOrEmpty(attrValue.AttrImpact))
                continue;

            if (attrValue.AttrImpact == "RoleAttrImpactBaseAttr")
            {
                var exAttr = GetGemAttr((RoleAttrEnum)attrValue.AttrParams[0], level);
                attrList.Add(exAttr);
            }
            else
            {
                var exAttr = attrValue.GetExAttr(level);
                attrList.Add(exAttr);
            }
        }
        return attrList;
    }

    #endregion

    #region skill

    public static int GetSkillDamageRate(int skillLv, List<int> skillParam)
    {
        var levelRate = GameDataValue.ConfigIntToFloat(10000 + skillParam[1]);
        var levelVal = Mathf.Pow(levelRate, skillLv);
        return (int)(levelVal * skillParam[0]);
    }

    #endregion

    #region damage

    public static int GetEleDamage(int eleAtk, float damageRate, int eleEnhance, int eleResist)
    {
        float eleDamage = eleAtk * damageRate * (1 + eleEnhance / 1000.0f);
        float resistRate = 0;
        if (eleResist > 0)
        {
            resistRate = (1 - eleResist / (eleResist + 1000.0f));
        }
        else
        {
            resistRate = (1 + eleResist / 1000.0f);
        }
        int finalDamage = Mathf.CeilToInt(eleDamage * resistRate);
        return finalDamage;
    }

    public static int GetPhyDamage(int phyAtk, float damageRate, int enhance, int defence, int roleLevel)
    {
        float eleDamage = phyAtk * damageRate * (1 + enhance / 1000.0f);
        float resistRate = 1;
        if (defence > 0 && roleLevel > 0)
        {
            resistRate = (1 - defence / (defence + _DefencePerLevel * roleLevel * 1.5f));
        }

        int finalDamage = Mathf.CeilToInt(eleDamage * resistRate);
        return finalDamage;
    }

    public static bool IsCriticleHit(int criticleRate)
    {
        int randomRate = Random.Range(0, 10001);
        return criticleRate > randomRate;
    }

    public static float GetCriticleDamageRate(int criticleDamage)
    {
        return ConfigIntToFloat(criticleDamage) + 0.5f;
    }

    #endregion

    #endregion

    #region product & consume

    #region exp

    public static int _NormalExpBase = 50;
    public static int _EliteExpBase = 250;
    public static int _SpecialExpBase = 500;
    public static int _BossExpBase = 1250;
    public static float _Level30ExpRate = 0.1f;
    public static float _Level60ExpRate = 0.12f;
    public static float _Level90ExpRate = 0.15f;
    public static float _Level100ExpRate = 0.2f;
    public static float _Level999ExpRate = 0.1f;

    public static int _LevelExpBase30 = 4000;
    public static int _LevelExpBase60 = 6000;
    public static int _LevelExpBase90 = 8000;
    public static int _LevelExpBase100 = 12000;
    public static int _LevelExpStep999 = 1600;
    public static int _LevelExpBase999 = 100000;
    public static int _LevelExpMax999 = 180000;
    public static float _LvUp30ExpRate = 0.25f;
    public static float _LvUp60ExpRate = 0.4f;
    public static float _LvUp90ExpRate = 0.6f;
    public static float _LvUp100ExpRate = 1.0f;
    public static float _LvUp999ExpRate = 1f;

    private static int _Lv30UpTotalVal = 0;
    private static int _Lv60UpTotalVal = 0;
    private static int _Lv90UpTotalVal = 0;
    private static int _Lv100UpTotalVal = 0;

    public static int GetLvUpExp(int playerLv, int attrLv)
    {
        if (_Lv30UpTotalVal == 0)
        {
            _Lv30UpTotalVal = Mathf.CeilToInt(_LvUp30ExpRate * 30 * _LevelExpBase30);
            _Lv60UpTotalVal = Mathf.CeilToInt(_Lv30UpTotalVal + _LvUp60ExpRate * 30 * _LevelExpBase60);
            _Lv90UpTotalVal = Mathf.CeilToInt(_Lv60UpTotalVal + _LvUp90ExpRate * 30 * _LevelExpBase90);
            _Lv100UpTotalVal = Mathf.CeilToInt(_Lv90UpTotalVal + _LvUp100ExpRate * 10 * _LevelExpBase100);

        }

        int realLv = playerLv + attrLv;
        int levelExp = 0;
        if (realLv <= 30)
        {
            levelExp = Mathf.CeilToInt((1 + _LvUp30ExpRate * realLv) * _LevelExpBase30);
        }
        else if (realLv <= 60)
        {
            levelExp = Mathf.CeilToInt((1 + _LvUp60ExpRate * (realLv - 30)) * _LevelExpBase60) + _Lv30UpTotalVal;
        }
        else if (realLv <= 90)
        {
            levelExp = Mathf.CeilToInt((1 + _LvUp90ExpRate * (realLv - 60)) * _LevelExpBase90) + _Lv60UpTotalVal;
        }
        else if (realLv <= 100)
        {
            levelExp = Mathf.CeilToInt((1 + _LvUp100ExpRate * (realLv - 90)) * _LevelExpBase100 + _Lv90UpTotalVal);
        }
        else
        {
            levelExp = Mathf.CeilToInt((1 + _LvUp999ExpRate * (realLv - 100)) * _LevelExpStep999 + _LevelExpBase999);
            levelExp = Mathf.Clamp(levelExp, _LevelExpBase999, _LevelExpMax999);
        }

        return levelExp;
    }

    private static float _Lv30TotalRate = 0;
    private static float _Lv60TotalRate = 0;
    private static float _Lv90TotalRate = 0;
    private static float _Lv100TotalRate = 0;
    public static int _MAX_MONSTER_EXP_LEVEL = 120;

    public static int GetMonsterExp(MOTION_TYPE motionType, int level, int playerLv)
    {
        int levelDelta = Mathf.Clamp(playerLv - level,0, 10);
        int monExpLevel = Mathf.Clamp(level, 1, _MAX_MONSTER_EXP_LEVEL);
        int expBase = 0;
        switch (motionType)
        {
            case MOTION_TYPE.Normal:
                expBase = _NormalExpBase;
                break;
            case MOTION_TYPE.Elite:
                expBase = _EliteExpBase;
                break;
            case MOTION_TYPE.Hero:
                expBase = _BossExpBase;
                break;
        }

        if (_Lv30TotalRate == 0)
        {
            _Lv30TotalRate = _Level30ExpRate * 30;
            _Lv60TotalRate = _Lv30TotalRate + _Level60ExpRate * 30;
            _Lv90TotalRate = _Lv60TotalRate + _Level90ExpRate * 30;
            _Lv100TotalRate = _Lv90TotalRate + _Level100ExpRate * 10;

        }

        int levelExp = 0;
        if (monExpLevel <= 30)
        {
            levelExp = Mathf.CeilToInt((1 + _Level30ExpRate * monExpLevel) * expBase);
        }
        else if (monExpLevel <= 60)
        {
            levelExp = Mathf.CeilToInt((1 + _Lv30TotalRate +  _Level60ExpRate * (monExpLevel - 30)) * expBase);
        }
        else if (monExpLevel <= 90)
        {
            levelExp = Mathf.CeilToInt((1 + _Lv60TotalRate + _Level90ExpRate * (monExpLevel - 60)) * expBase);
        }
        else if (monExpLevel <= 100)
        {
            levelExp = Mathf.CeilToInt((1 + _Lv90TotalRate + _Level100ExpRate * (monExpLevel - 90)) * expBase);
        }
        else
        {
            levelExp = Mathf.CeilToInt((1 + _Lv100TotalRate + _Level999ExpRate * (monExpLevel - 100)) * expBase);
        }

        var deltaRate = Mathf.Clamp(levelDelta * 0.03f, -0.3f, 0.2f);
        levelExp = Mathf.CeilToInt(levelExp * (1 + deltaRate));
        return levelExp;
    }

    #endregion

    #region equip

    private static int _EqiupLvWeapon = 1;
    private static int _EqiupLvTorso = 2;
    private static int _EqiupLvShoes = 3;
    private static int _EqiupLvRing = 4;
    private static int _EqiupLvAmulate = 4;

    private static List<ITEM_QUALITY> GetDropQualitys(MOTION_TYPE motionType, MonsterBaseRecord monsterRecord, int level, int dropActType = 1)
    {
        List<ITEM_QUALITY> dropEquipQualitys = new List<ITEM_QUALITY>();
        int dropCnt = 0;
        int dropQuality = 0;
        switch (motionType)
        {
            case MOTION_TYPE.Normal:
                dropCnt = GameRandom.GetRandomLevel(95, 5);
                for (int i = 0; i < dropCnt; ++i)
                {
                    dropQuality = GameRandom.GetRandomLevel(70, 30);
                    dropEquipQualitys.Add((ITEM_QUALITY)dropQuality);
                }
                
                break;
            case MOTION_TYPE.Elite:
                dropCnt = GameRandom.GetRandomLevel(10, 70, 20);
                for (int i = 0; i < dropCnt; ++i)
                {
                    dropQuality = GameRandom.GetRandomLevel(30, 65, 5);
                    dropEquipQualitys.Add((ITEM_QUALITY)dropQuality);
                }

                break;
            case MOTION_TYPE.Hero:
                if (level <= 30)
                    dropCnt = GameRandom.GetRandomLevel(0, 50, 30, 10);
                else if (level <= 60)
                    dropCnt = GameRandom.GetRandomLevel(0, 50, 30, 10);
                else
                    dropCnt = GameRandom.GetRandomLevel(0, 50, 30, 10);
                bool isOringe = false;
                for (int i = 0; i < dropCnt; ++i)
                {
                    if(level <= 30)
                        dropQuality = GameRandom.GetRandomLevel(0, 60, 30, 10);
                    else
                        dropQuality = GameRandom.GetRandomLevel(0, 60, 35, 5);
                    if (dropQuality == (int)ITEM_QUALITY.ORIGIN)
                    {
                        if (!isOringe)
                        {
                            isOringe = true;
                        }
                        else
                        {
                            --dropQuality;
                        }
                    }
                    dropEquipQualitys.Add((ITEM_QUALITY)dropQuality);
                }

                break;
        }
        return dropEquipQualitys;
    }

    private static int GetEquipLv(EQUIP_SLOT equipSlot, int dropLevel)
    {
        if (dropLevel > _MaxLv)
            return _MaxLv;
        var levelGroup = dropLevel / 5;
        var baseLevel = levelGroup * 5;
        switch (equipSlot)
        {
            case EQUIP_SLOT.WEAPON:
                return baseLevel + _EqiupLvWeapon;
            case EQUIP_SLOT.TORSO:
                return baseLevel + _EqiupLvTorso;
            case EQUIP_SLOT.LEGS:
                return baseLevel + _EqiupLvShoes;
            case EQUIP_SLOT.RING:
                return baseLevel + _EqiupLvRing;
            case EQUIP_SLOT.AMULET:
                return baseLevel + _EqiupLvAmulate;
        }
        return dropLevel;
    }

    public static List<ItemEquip> GetMonsterDropEquip(MOTION_TYPE motionType, MonsterBaseRecord monsterRecord, int level, int dropActType = 1)
    {
        List<ItemEquip> dropEquipList = new List<ItemEquip>();
        var dropEquipQualitys = GetDropQualitys(motionType, monsterRecord, level, dropActType);
        if (dropEquipQualitys.Count == 0)
            return dropEquipList;

        for (int i = 0; i < dropEquipQualitys.Count; ++i)
        {
            if (dropEquipQualitys[i] == ITEM_QUALITY.ORIGIN)
            {
                if (monsterRecord.ValidSpDrops.Count == 0)
                    continue;

                int dropIdx = Random.Range(0, monsterRecord.ValidSpDrops.Count);
                var dropItem = monsterRecord.ValidSpDrops[dropIdx];
                var dropEquipTab = TableReader.EquipItem.GetRecord(dropItem.Id);
                var equipLevel = GetEquipLv(dropEquipTab.Slot, level);
                var equipValue = CalLvValue(equipLevel);
                var dropEquip = ItemEquip.CreateEquip(equipLevel, dropEquipQualitys[i], equipValue, int.Parse(dropItem.Id), (int)dropEquipTab.Slot);
                dropEquipList.Add(dropEquip);
            }
            else
            {
                var equipSlot = GetRandomItemSlot(dropEquipQualitys[i]);
                var equipLevel = GetEquipLv(equipSlot, level);
                var equipValue = CalLvValue(equipLevel);
                var dropEquip = ItemEquip.CreateEquip(equipLevel, dropEquipQualitys[i], equipValue, -1, (int)equipSlot);
                dropEquipList.Add(dropEquip);
            }
        }

        return dropEquipList;
    }

    public static int GetLegencyLv(int equipLv)
    {
        int randomLv = GameRandom.GetRandomLevel(3, 5, 2);
        int baseLv = 1;
        if (equipLv < 30)
        {
            baseLv = 1;
        }
        else if (equipLv < 50)
        {
            baseLv = 2;
        }
        else if (equipLv < 70)
        {
            baseLv = 3;
        }
        else if (equipLv < 90)
        {
            baseLv = 4;
        }
        else
        {
            baseLv = 5;
        }

        int delta = (equipLv - 100) / 10;
        baseLv += delta;

        baseLv += (randomLv - 1);
        baseLv = Mathf.Max(baseLv, 1);
        return baseLv;
    }

    public static EQUIP_SLOT GetRandomItemSlot(ITEM_QUALITY itemQuality)
    {
        int slotTypeCnt = (int)EQUIP_SLOT.RING + 1;
        if (itemQuality == ITEM_QUALITY.WHITE)
        {
            slotTypeCnt = (int)EQUIP_SLOT.LEGS + 1;
        }
        int randomSlot = UnityEngine.Random.Range(0, slotTypeCnt);
        return (EQUIP_SLOT)randomSlot;
    }


    #endregion

    #region equip material

    public static int _EquipMatDropBase = 100;
    public static int _DropMatLevel = 50;
    public static int _ConsumeOneTime = 6;
    public static int _DestoryEquipMat = 5;
    public static float _ComsumeDiamondFixed = 0.1f;

    public static float _LevelParam = 0.01f;
    public static int _NormalMatBase = 2500;
    public static int _EliteMatBase = 10000;
    public static int _SpecialMatBase = 10000;
    public static int _BossMatBase = 100000;
    //public static int _NormalMatBase = 0;
    //public static int _EliteMatBase = 10000;
    //public static int _SpecialMatBase = 10000;
    //public static int _BossMatBase = 50000;

    public static int GetEquipMatDropCnt(MOTION_TYPE motionType, MonsterBaseRecord monsterRecord, int level)
    {
        int dropCnt = 0;
        if (level < _DropMatLevel)
            return dropCnt;

        switch (motionType)
        {
            case MOTION_TYPE.Normal:
                dropCnt = GetDropCnt(Mathf.CeilToInt( _NormalMatBase * level * _LevelParam));
                break;
            case MOTION_TYPE.Elite:
                dropCnt = GetDropCnt(Mathf.CeilToInt(_EliteMatBase * level * _LevelParam));
                break;
            case MOTION_TYPE.Hero:
                dropCnt = GetDropCnt(Mathf.CeilToInt(_BossMatBase * level * _LevelParam));
                break;
        }

        return dropCnt;
    }

    private static int GetDropCnt(int rate)
    {
        if (rate >= GetMaxRate())
        {
            return Mathf.CeilToInt(rate / GetMaxRate());
        }

        var random = Random.Range(0, GetMaxRate());
        if (random < rate)
            return 1;

        return 0;
    }

    public static int GetEquipLvUpConsume(ItemEquip equip)
    {
        return Mathf.CeilToInt(_ConsumeOneTime * equip.EquipLevel * 0.1f);
    }

    public static int GetEquipLvUpConsumeDiamond(ItemEquip equip)
    {
        return Mathf.CeilToInt(GetEquipLvUpConsume(equip) * _ComsumeDiamondFixed);
    }

    public static int GetDestoryGetMatCnt(ItemEquip equip)
    {
        if (equip.EquipLevel < _DropMatLevel)
            return 0;

        int destoryMatCnt = _DestoryEquipMat * equip.EquipLevel;

        if (equip.EquipLevel < 100)
        {
            if (equip.EquipQuality == ITEM_QUALITY.PURPER)
                destoryMatCnt = Mathf.CeilToInt(destoryMatCnt * 0.08f);
            else if (equip.EquipQuality == ITEM_QUALITY.ORIGIN)
                destoryMatCnt = Mathf.CeilToInt(destoryMatCnt * 0.3f);
            else
                destoryMatCnt = Mathf.CeilToInt(destoryMatCnt * 0.03f);
        }
        else
        {
            if (equip.EquipQuality == ITEM_QUALITY.PURPER)
                destoryMatCnt = Mathf.CeilToInt(destoryMatCnt * 0.1f);
            else if (equip.EquipQuality == ITEM_QUALITY.ORIGIN)
                destoryMatCnt = Mathf.CeilToInt(destoryMatCnt * 0.5f);
            else
                destoryMatCnt = Mathf.CeilToInt(destoryMatCnt * 0.04f);
        }

        return destoryMatCnt;
    }
    #endregion

    #region gem

    public static float _GemConsumeV = 10;
    public static float _GemConsumeA = 5;
    public static int _DropGemLevel = 30;

    public static float _LevelGemParam = 0.01f;
    public static int _NormalGemBase = 800;
    public static int _EliteGemBase = 5000;
    public static int _SpecialGemBase = 5000;
    public static int _BossGemBase = 150000;
    //public static int _NormalGemBase = 0;
    //public static int _EliteGemBase = 0;
    //public static int _SpecialGemBase = 0;
    //public static int _BossGemBase = 150000;

    public static string GetGemMatDropItemID(MonsterBaseRecord monsterRecord)
    {
        if (monsterRecord.ElementType > 0)
        {
            return GemData._GemMaterialDataIDs[monsterRecord.ElementType];
        }
        else
        {
            var random = Random.Range(0, GemData._GemMaterialDataIDs.Count);
            return GemData._GemMaterialDataIDs[random];
        }
    }

    public static int GetGemMatDropCnt(MOTION_TYPE motionType, MonsterBaseRecord monsterRecord, int level)
    {
        int dropCnt = 0;
        if (level < _DropGemLevel)
            return dropCnt;

        switch (motionType)
        {
            case MOTION_TYPE.Normal:
                dropCnt = GetDropCnt(Mathf.CeilToInt(_NormalGemBase * level * _LevelGemParam));
                break;
            case MOTION_TYPE.Elite:
                dropCnt = GetDropCnt(Mathf.CeilToInt(_EliteGemBase * level * _LevelGemParam));
                break;
            case MOTION_TYPE.Hero:
                dropCnt = GetDropCnt(Mathf.CeilToInt(_BossGemBase * level * _LevelGemParam));
                break;
        }

        return dropCnt;
    }

    public static int GetGemConsume(int level)
    {
        int consumeCnt = 0;
        if (level <= 5)
        {
            consumeCnt = Mathf.CeilToInt(_GemConsumeV + _GemConsumeA * level);
        }
        else
        {
            int pow = (level - 5);
            int preConsumeBase = Mathf.CeilToInt(_GemConsumeV + _GemConsumeA * 5);
            float prePow = Mathf.Pow(1.5f, pow);
            consumeCnt = Mathf.CeilToInt(preConsumeBase * prePow);

        }
        //consumeCnt = Mathf.CeilToInt( _GemConsumeV  + _GemConsumeA * level);
        return consumeCnt;
    }

    #endregion

    #region gold

    public static float _GoldLevelParam = 10f;
    public static int _NormalGoldBase = 2000;
    public static int _EliteGoldBase = 8000;
    public static int _SpecialGoldBase = 8000;
    public static int[] _BossGoldBase = {0, 6000, 3000, 1000};
    

    public static int GetGoldDropCnt(MOTION_TYPE motionType, int level)
    {
        int dropCnt = 0;
        switch (motionType)
        {
            case MOTION_TYPE.Normal:
                dropCnt = GetGoldDropCnt(_NormalGoldBase);
                break;
            case MOTION_TYPE.Elite:
                dropCnt = GetGoldDropCnt(_EliteGoldBase);
                break;
            case MOTION_TYPE.Hero:
                dropCnt = GetGoldDropCnt(_BossGoldBase) + 1;
                break;
        }

        return dropCnt;
    }

    public static int GetGoldDropNum(MOTION_TYPE motionType, int level)
    {
        float dropNum = level * _GoldLevelParam;
        if (motionType == MOTION_TYPE.Normal)
            dropNum *= 0.5f;

        float random = Random.Range(0.6f, 1.4f);
        return Mathf.CeilToInt(dropNum * random);
    }

    public static int GetGoldDropCnt(params int[] rates)
    {
        int goldCnt = 0;
        var random = Random.Range(0, GetMaxRate());
        if (rates.Length == 1)
        {
            if (random < rates[0])
                goldCnt = 1;
        }
        else
        {
            goldCnt = GameRandom.GetRandomLevel(rates);
        }

        return goldCnt;
    }

    #endregion

    #region skill

    public static int GetSkillLvUpGold(int costBase, int skillLv)
    {
        int goldCost = (int)(Mathf.Pow(1.35f, skillLv) * costBase);
        return goldCost;
    }

    #endregion

    #endregion

    #region monster attr

    public static int _BossStageStarLevel;

    public static int GetStageLevel(int difficult, int stageIdx, STAGE_TYPE stageMode)
    {
        int level = 0;
        if (stageMode == STAGE_TYPE.NORMAL)
        {
            level = (difficult - 1) * 20 + stageIdx;
        }
        else if (stageMode == STAGE_TYPE.BOSS)
        {
            var stageRecord = TableReader.BossStage.GetRecord(stageIdx.ToString());
            level = stageRecord.Level;
        }

        return level;
    }

    public static float _MonsterHPParam = 0.012f;
    public static float _MonsterAtkParam = 0.006f;

    public static int GetMonsterHP(MonsterBaseRecord monsterBase, int roleLv, MOTION_TYPE monsterType)
    {
        int hpMax = (int)(roleLv * roleLv * _MonsterHPParam * monsterBase.BaseAttr[0] + monsterBase.BaseAttr[0]);
        return hpMax;
    }

    public static int GetMonsterAtk(MonsterBaseRecord monsterBase, int roleLv, MOTION_TYPE monsterType)
    {
        int atk = (int)(roleLv * roleLv * _MonsterAtkParam * monsterBase.BaseAttr[1] + monsterBase.BaseAttr[1]);
        return atk;
    }

    #endregion

}
