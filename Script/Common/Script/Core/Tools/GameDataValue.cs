using System.Collections;
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

    private static int _MaxLv = 50;
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
        var attrRecord = TableReader.EquipBaseAttr.GetEquipBaseAttr(equiplevel);
        return attrRecord.Atk;
        //int power = equiplevel / 5;
        //var attackValue = _AttackPerLevel* Mathf.Pow(_AttackIncreaseLevel, power);
        //return Mathf.CeilToInt(attackValue);
    }

    public static int CalEquipTorsoHP(int equiplevel)
    {
        //int power = equiplevel / 5;
        //var value = _HPPerLevel * Mathf.Pow(_AttackIncreaseLevel, power);
        //return Mathf.CeilToInt(value);
        var value = CalEquipLegsHP(equiplevel) * 0.5f;
        return Mathf.CeilToInt(value);
    }

    public static int CalEquipTorsoDefence(int equiplevel)
    {
        //int power = equiplevel / 5;
        //var value = _DefencePerLevel * Mathf.Pow(_AttackIncreaseLevel, power);
        //return Mathf.CeilToInt(value);
        var attrRecord = TableReader.EquipBaseAttr.GetEquipBaseAttr(equiplevel);
        return attrRecord.Defence;
    }

    public static int CalEquipLegsHP(int equiplevel)
    {
        var attrRecord = TableReader.EquipBaseAttr.GetEquipBaseAttr(equiplevel);
        return attrRecord.HP;
    }

    public static int CalEquipLegsDefence(int equiplevel)
    {
        var value = CalEquipTorsoDefence(equiplevel) * 0.5f;
        return Mathf.CeilToInt(value);
    }

    #endregion

    #region baseAttr -> exAttr atk

    public static int _AtkPerRoleLevel = 0;
    public static int _AtkRoleLevelBase = 10;
    public static int _HPPerRoleLevel = 0;
    public static int _HPRoleLevelBase = 100;

    public static float _AttackPerStrength = 0.5f;
    public static float _DmgEnhancePerStrength = 0.5f;
    public static float _StrToAtk = 1f;

    public static float _IgnoreAtkPerDex = 0.125f;
    public static float _CriticalRatePerDex = 1f;
    public static float _CriticalDmgPerDex = 10f;
    public static float _DexToAtk = 1f;

    public static float _EleAtkPerInt = 0.1f;
    public static float _EleEnhancePerInt = 0.4f;
    public static float _IntToAtk = 1f;

    public static float _HPPerVit = 8;
    public static float _FinalDmgRedusePerVit = 0.12f;
    public static float _VitToAtk = 1f;

    public static float _CriticalDmgToAtk = 2f;

    public static float _ElementToAtk = 1f;
    public static float _DmgEnhancePerElementEnhance = 10;
    public static float _EleEnhanceToAtk = 1f;
    public static float _EleResistToAtk = 1f;

    public static float _IgnoreDefenceToAtk = 0.5f;

    public static float _HpToAtk = 10.0f;
    public static float _DefToAtk = 1.0f;
    public static float _MoveSpeedToAtk = 18f;
    public static float _AtkSpeedToAtk = 8f;
    public static float _CriticalChanceToAtk = 8f;
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

    public static int CalLvValue(int level, EQUIP_SLOT equipSlot)
    {
        //var exValue = _LvValueV * level + level * level * 0.5f * _LvValueA + _LvValueBase;
        //if (equipSlot == EQUIP_SLOT.AMULET || equipSlot == EQUIP_SLOT.RING)
        //{
        //    exValue *= 2;

        //}
        //return Mathf.CeilToInt(exValue);

        var attrRecord = TableReader.EquipBaseAttr.GetEquipBaseAttr(level);
        int equipValue = attrRecord.Value;
        if (equipSlot == EQUIP_SLOT.AMULET || equipSlot == EQUIP_SLOT.RING)
        {
            equipValue *= 2;

        }
        return equipValue;
    }

    #endregion

    #region equip

    public static int GetExAttrRandomValue(RoleAttrEnum roleAttr, int baseValue, float lowPersent = 0.8f, float upPersent = 1.2f)
    {
        var randomValue = Random.Range(lowPersent, upPersent);
        if (roleAttr == RoleAttrEnum.AttackPersent)
        {
            return Mathf.CeilToInt(5000 * randomValue);
        }
        else if (roleAttr == RoleAttrEnum.HPMaxPersent)
        {
            return Mathf.CeilToInt(5000 * randomValue);
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

    public static List<RoleAttrEnum> GetRandomEquipAttrsType(Tables.EQUIP_SLOT equipSlot, Tables.ITEM_QUALITY quality, EquipItemRecord legencyEquip, int fixNum = 0)
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
                    exAttrCnt = 3;
                    if (quality == Tables.ITEM_QUALITY.BLUE)
                    {
                        exAttrCnt = Random.Range(1, 3);
                    }
                    else if (quality == ITEM_QUALITY.PURPER)
                    {
                        exAttrCnt = 3;
                    }
                    else if (quality == ITEM_QUALITY.ORIGIN)
                    {
                        if (legencyEquip == null)
                        {
                            exAttrCnt = 4;
                        }
                        else
                        {
                            exAttrCnt = 3;
                        }
                    }
                    return CalRandomAttrs(_WeaponExAttrs, exAttrCnt);
                case Tables.EQUIP_SLOT.TORSO:
                    exAttrCnt = 3;
                    if (quality == Tables.ITEM_QUALITY.BLUE)
                    {
                        exAttrCnt = Random.Range(1, 3);
                    }
                    else if (quality == ITEM_QUALITY.PURPER)
                    {
                        exAttrCnt = 3;
                    }
                    else if (quality == ITEM_QUALITY.ORIGIN)
                    {
                        if (legencyEquip == null)
                        {
                            exAttrCnt = 4;
                        }
                        else
                        {
                            exAttrCnt = 3;
                        }
                    }
                    return CalRandomAttrs(_DefenceExAttrs, exAttrCnt);
                case Tables.EQUIP_SLOT.LEGS:
                    exAttrCnt = 3;
                    if (quality == Tables.ITEM_QUALITY.BLUE)
                    {
                        exAttrCnt = Random.Range(1, 3);
                    }
                    else if (quality == ITEM_QUALITY.PURPER)
                    {
                        exAttrCnt = 3;
                    }
                    else if (quality == ITEM_QUALITY.ORIGIN)
                    {
                        if (legencyEquip == null)
                        {
                            exAttrCnt = 4;
                        }
                        else
                        {
                            exAttrCnt = 3;
                        }
                    }
                    return CalRandomAttrs(_DefenceExAttrs, exAttrCnt);
                case Tables.EQUIP_SLOT.AMULET:
                    exAttrCnt = 3;
                    if (quality == Tables.ITEM_QUALITY.BLUE)
                    {
                        exAttrCnt = Random.Range(1, 3);
                    }
                    else if (quality == ITEM_QUALITY.PURPER)
                    {
                        exAttrCnt = 3;
                    }
                    else if (quality == ITEM_QUALITY.ORIGIN)
                    {
                        if (legencyEquip == null)
                        {
                            exAttrCnt = 4;
                        }
                        else
                        {
                            exAttrCnt = 3;
                        }
                    }
                    return CalRandomAttrs(_AmuletExAttrs, exAttrCnt);
                case Tables.EQUIP_SLOT.RING:
                    exAttrCnt = 3;
                    if (quality == Tables.ITEM_QUALITY.BLUE)
                    {
                        exAttrCnt = Random.Range(1, 3);
                    }
                    else if (quality == ITEM_QUALITY.PURPER)
                    {
                        exAttrCnt = 3;
                    }
                    else if (quality == ITEM_QUALITY.ORIGIN)
                    {
                        if (legencyEquip == null)
                        {
                            exAttrCnt = 4;
                        }
                        else
                        {
                            exAttrCnt = 3;
                        }
                    }
                    return CalRandomAttrs(_RingExAttrs, exAttrCnt);
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

        foreach (var equipExAttr in itemEquip.EquipExAttrs)
        {
            if (equipExAttr.AttrType != "RoleAttrImpactBaseAttr")
                continue;

            if (equipExAttr.AttrParams[0] == (int)RoleAttrEnum.AttackPersent || equipExAttr.AttrParams[0] == (int)RoleAttrEnum.HPMaxPersent)
            {
                var randomPersent = Random.Range(10, 15);
                equipExAttr.Value = Mathf.Min(5000, equipExAttr.Value + randomPersent);
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

        var attrEnums = GetRandomEquipAttrsType(itemEquip.EquipItemRecord.Slot, itemEquip.EquipQuality, null, valueAttrs.Count);
        //var attrEnums = itemEquip.EquipExAttr;
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
            valueAttrs[i].AttrParams[1] = GetValueAttr((RoleAttrEnum)valueAttrs[i].AttrParams[0], valueAttrs[i].Value);
            increaseValue -= randomIncValue;
        }

        itemEquip.RefreshEquip();
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

    public static List<EquipExAttrRandom> _RingExAttrs = new List<EquipExAttrRandom>()
    {
        new EquipExAttrRandom(RoleAttrEnum.Strength, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.Dexterity, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.Intelligence, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.Vitality, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.HPMax, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.Attack, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.Defense, true, -1, 100),
        new EquipExAttrRandom(RoleAttrEnum.AttackSpeed, false, 300, 100),
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
    public static float _GemAttrA = 13;

    public static int GetGemValue(int level)
    {
        int value = Mathf.CeilToInt(level * _GemAttrV + 0.5f * level * level * _GemAttrA);
        return value;
    }

    public static EquipExAttr GetGemAttr(RoleAttrEnum attr, int value)
    {
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
                int levelID = Mathf.Clamp(level, 0, 200);
                var gemAttrRecord = TableReader.GemBaseAttr.GetRecord(levelID.ToString());
                if (gemAttrRecord == null)
                {
                    gemAttrRecord = TableReader.GemBaseAttr.GetRecord("200");
                }
                var exAttr = GetGemAttr((RoleAttrEnum)attrValue.AttrParams[0], gemAttrRecord.SetAttrValue);
                attrList.Add(exAttr);
            }
            else
            {
                int spAttrLv = GemSuit._ActAttrLevel[GemSuit._ActAttrLevel.Count - 1];
                int attrLv = 0;
                if (level > spAttrLv)
                {
                    attrLv = Mathf.CeilToInt((level - spAttrLv) / 5 + 1);
                }
                var exAttr = attrValue.GetExAttr(attrLv);
                attrList.Add(exAttr);
            }
        }
        return attrList;
    }

    #endregion

    #region skill

    public static int GetSkillDamageRate(int skillLv, List<int> skillParam)
    {
        var levelRate = skillParam[1];
        var levelVal = levelRate * (skillLv - 1);
        return (int)(levelVal + skillParam[0]);
    }

    #endregion

    #region damage

    public const int _MAX_ROLE_LEVEL = 200;

    public static int GetRoleLv(int level)
    {
        return Mathf.Clamp(level, 1, _MAX_ROLE_LEVEL);
    }

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
        var equipRecord = TableReader.EquipBaseAttr.GetRecord(GetRoleLv(roleLevel).ToString());
        float eleDamage = phyAtk * damageRate * (1 + enhance / 1000.0f);
        float resistRate = 1;
        if (defence > 0 && roleLevel > 0)
        {
            resistRate = (1 - (float)defence / (defence + equipRecord.DefenceStandar));
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
        int totalLv = playerLv + attrLv;
        RoleExpRecord expRecord = null;
        if (totalLv > 200)
        {
            expRecord = TableReader.RoleExp.GetRecord("200");
        }
        else
        {
            expRecord = TableReader.RoleExp.GetRecord(totalLv.ToString());
        }
        return expRecord.ExpValue;

        //if (_Lv30UpTotalVal == 0)
        //{
        //    _Lv30UpTotalVal = Mathf.CeilToInt(_LvUp30ExpRate * 30 * _LevelExpBase30);
        //    _Lv60UpTotalVal = Mathf.CeilToInt(_Lv30UpTotalVal + _LvUp60ExpRate * 30 * _LevelExpBase60);
        //    _Lv90UpTotalVal = Mathf.CeilToInt(_Lv60UpTotalVal + _LvUp90ExpRate * 30 * _LevelExpBase90);
        //    _Lv100UpTotalVal = Mathf.CeilToInt(_Lv90UpTotalVal + _LvUp100ExpRate * 10 * _LevelExpBase100);

        //}

        //int realLv = playerLv + attrLv;
        //int levelExp = 0;
        //if (realLv <= 30)
        //{
        //    levelExp = Mathf.CeilToInt((1 + _LvUp30ExpRate * realLv) * _LevelExpBase30);
        //}
        //else if (realLv <= 60)
        //{
        //    levelExp = Mathf.CeilToInt((1 + _LvUp60ExpRate * (realLv - 30)) * _LevelExpBase60) + _Lv30UpTotalVal;
        //}
        //else if (realLv <= 90)
        //{
        //    levelExp = Mathf.CeilToInt((1 + _LvUp90ExpRate * (realLv - 60)) * _LevelExpBase90) + _Lv60UpTotalVal;
        //}
        //else if (realLv <= 100)
        //{
        //    levelExp = Mathf.CeilToInt((1 + _LvUp100ExpRate * (realLv - 90)) * _LevelExpBase100 + _Lv90UpTotalVal);
        //}
        //else
        //{
        //    levelExp = Mathf.CeilToInt((1 + _LvUp999ExpRate * (realLv - 100)) * _LevelExpStep999 + _LevelExpBase999);
        //    levelExp = Mathf.Clamp(levelExp, _LevelExpBase999, _LevelExpMax999);
        //}

        //return levelExp;
    }

    private static float _Lv30TotalRate = 0;
    private static float _Lv60TotalRate = 0;
    private static float _Lv90TotalRate = 0;
    private static float _Lv100TotalRate = 0;
    public static int _MAX_MONSTER_EXP_LEVEL = 120;

    public static int GetMonsterExp(MOTION_TYPE motionType, int level, int playerLv)
    {
        MonsterAttrRecord monAttrRecord = null;
        if (level > 200)
        {
            monAttrRecord = TableReader.MonsterAttr.GetRecord("200");
        }
        else
        {
            monAttrRecord = TableReader.MonsterAttr.GetRecord(level.ToString());
        }
        int exp= monAttrRecord.Drops[0];
        if (motionType == MOTION_TYPE.Elite)
        {
            exp = exp * 3;
        }
        else if (motionType == MOTION_TYPE.Hero)
        {
            exp = exp * 10;
        }
        return exp;

        //int levelDelta = Mathf.Clamp(playerLv - level,0, 10);
        //int monExpLevel = Mathf.Clamp(level, 1, _MAX_MONSTER_EXP_LEVEL);
        //int expBase = 0;
        //switch (motionType)
        //{
        //    case MOTION_TYPE.Normal:
        //        expBase = _NormalExpBase;
        //        break;
        //    case MOTION_TYPE.Elite:
        //        expBase = _EliteExpBase;
        //        break;
        //    case MOTION_TYPE.Hero:
        //        expBase = _BossExpBase;
        //        break;
        //}

        //if (_Lv30TotalRate == 0)
        //{
        //    _Lv30TotalRate = _Level30ExpRate * 30;
        //    _Lv60TotalRate = _Lv30TotalRate + _Level60ExpRate * 30;
        //    _Lv90TotalRate = _Lv60TotalRate + _Level90ExpRate * 30;
        //    _Lv100TotalRate = _Lv90TotalRate + _Level100ExpRate * 10;

        //}

        //int levelExp = 0;
        //if (monExpLevel <= 30)
        //{
        //    levelExp = Mathf.CeilToInt((1 + _Level30ExpRate * monExpLevel) * expBase);
        //}
        //else if (monExpLevel <= 60)
        //{
        //    levelExp = Mathf.CeilToInt((1 + _Lv30TotalRate +  _Level60ExpRate * (monExpLevel - 30)) * expBase);
        //}
        //else if (monExpLevel <= 90)
        //{
        //    levelExp = Mathf.CeilToInt((1 + _Lv60TotalRate + _Level90ExpRate * (monExpLevel - 60)) * expBase);
        //}
        //else if (monExpLevel <= 100)
        //{
        //    levelExp = Mathf.CeilToInt((1 + _Lv90TotalRate + _Level100ExpRate * (monExpLevel - 90)) * expBase);
        //}
        //else
        //{
        //    levelExp = Mathf.CeilToInt((1 + _Lv100TotalRate + _Level999ExpRate * (monExpLevel - 100)) * expBase);
        //}

        //var deltaRate = Mathf.Clamp(levelDelta * 0.03f, -0.3f, 0.2f);
        //levelExp = Mathf.CeilToInt(levelExp * (1 + deltaRate));
        //return levelExp;
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
        float exEquipRate = ConfigIntToFloat(RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.ExEquipDrop)) + 1;
        switch (motionType)
        {
            case MOTION_TYPE.Normal:
                dropCnt = GameRandom.GetRandomLevel(95, 5);
                for (int i = 0; i < dropCnt; ++i)
                {
                    dropQuality = GameRandom.GetRandomLevel(7000, (int)(3000 * exEquipRate));
                    dropEquipQualitys.Add((ITEM_QUALITY)dropQuality);
                }
                
                break;
            case MOTION_TYPE.Elite:
                dropCnt = GameRandom.GetRandomLevel(10, 70, 20);
                for (int i = 0; i < dropCnt; ++i)
                {
                    dropQuality = GameRandom.GetRandomLevel(3000, (int)(6500 * exEquipRate), (int)(500 * exEquipRate));
                    dropEquipQualitys.Add((ITEM_QUALITY)dropQuality);
                }

                break;
            case MOTION_TYPE.Hero:
                if (level <= 20)
                    dropCnt = GameRandom.GetRandomLevel(0, 10, 50, 30);
                else if (level <= 40)
                    dropCnt = GameRandom.GetRandomLevel(0, 0, 50, 50,10);
                else
                    dropCnt = GameRandom.GetRandomLevel(0, 0, 50, 30,30);
                bool isOringe = false;
                for (int i = 0; i < dropCnt; ++i)
                {
                    if (level <= 10)
                        dropQuality = GameRandom.GetRandomLevel(0, 7000, (int)(2500 * exEquipRate), (int)(500 * exEquipRate));
                    else if (level <= 30)
                        dropQuality = GameRandom.GetRandomLevel(0, 6000, (int)(2500 * exEquipRate), (int)(1500 * exEquipRate));
                    else
                        dropQuality = GameRandom.GetRandomLevel(0, 4000, (int)(3500 * exEquipRate), (int)(2500 * exEquipRate));
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

        var randomValue = Random.Range(-1, 4);
        int equipLv = dropLevel + randomValue;
        return Mathf.Clamp(equipLv, 1, _MaxLv);
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

                int dropIdx = GameRandom.GetRandomLevel(35, 30, 20, 15, 10, 50);
                //int dropIdx = Random.Range(0, monsterRecord.ValidSpDrops.Count);
                var dropItem = monsterRecord.SpDrops[dropIdx];
                if (dropItem == null)
                {
                    var equipSlot = GetRandomItemSlot(dropEquipQualitys[i]);
                    var equipLevel = GetEquipLv(equipSlot, level);
                    var equipValue = CalLvValue(equipLevel, equipSlot);
                    var dropEquip = ItemEquip.CreateEquip(equipLevel, dropEquipQualitys[i], -1, (int)equipSlot);
                    dropEquipList.Add(dropEquip);
                }
                else
                {
                    var dropEquipTab = TableReader.EquipItem.GetRecord(dropItem.Id);
                    var equipLevel = GetEquipLv(dropEquipTab.Slot, level);
                    var equipValue = CalLvValue(equipLevel, dropEquipTab.Slot);
                    var dropEquip = ItemEquip.CreateEquip(equipLevel, dropEquipQualitys[i], int.Parse(dropItem.Id), (int)dropEquipTab.Slot);
                    dropEquipList.Add(dropEquip);
                }
            }
            else
            {
                var equipSlot = GetRandomItemSlot(dropEquipQualitys[i]);
                var equipLevel = GetEquipLv(equipSlot, level);
                var equipValue = CalLvValue(equipLevel, equipSlot);
                var dropEquip = ItemEquip.CreateEquip(equipLevel, dropEquipQualitys[i], -1, (int)equipSlot);
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

    public static int GetEquipSellGold(ItemEquip itemEquip)
    {
        int gold = Mathf.CeilToInt((itemEquip.EquipLevel * 0.5f + itemEquip.EquipExAttrs.Count) * ((int)itemEquip.EquipQuality + 1));
        return gold;
    }

    public static int GetEquipBuyGold(ItemEquip itemEquip)
    {
        int gold = GetEquipSellGold(itemEquip) * 8;
        return gold;
    }


    #endregion

    #region equip material

    public static int _EquipMatDropBase = 100;
    public static int _DropMatLevel = 30;
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

        float modifyRate = (ConfigIntToFloat(RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.ExMatDrop)) + 1);
        switch (motionType)
        {
            case MOTION_TYPE.Normal:
                dropCnt = GetDropCnt(Mathf.CeilToInt( _NormalMatBase * level * _LevelParam * modifyRate));
                break;
            case MOTION_TYPE.Elite:
                dropCnt = GetDropCnt(Mathf.CeilToInt(_EliteMatBase * level * _LevelParam * modifyRate));
                break;
            case MOTION_TYPE.Hero:
                dropCnt = GetDropCnt(Mathf.CeilToInt(_BossMatBase * level * _LevelParam * modifyRate));
                break;
        }

        return dropCnt;
    }

    private static int GetDropCnt(int rate)
    {
        
        if (rate >= GetMaxRate())
        {
            int baseDrop = Mathf.CeilToInt(rate / GetMaxRate());
            int exRate = (rate - GetMaxRate() * baseDrop) / GetMaxRate();
            if (exRate > 0)
            {
                var exRandom = Random.Range(0, GetMaxRate());
                if (exRandom < exRate)
                    baseDrop = baseDrop + 1;
            }
            return baseDrop;
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

        return destoryMatCnt + equip.EquipRefreshCostMatrial;
    }
    #endregion

    #region gem

    public static float _GemConsumeV = 1;
    public static float _GemConsumeA = 2;
    public static int _DropGemLevel = 10;

    public static float _LevelGemParam = 0.02f;
    public static int _NormalGemBase = 1;
    public static int _EliteGemBase = 2;
    public static int _SpecialGemBase = 2;
    public static int _BossGemBase = 10;
    //public static int _NormalGemBase = 0;
    //public static int _EliteGemBase = 0;
    //public static int _SpecialGemBase = 0;
    //public static int _BossGemBase = 150000;

    public static int _GemConsumeGoldBase = 200;
    public static float _GemConsumeGoldStep = 300;

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
        //int dropCnt = 0;
        //if (level < _DropGemLevel)
        //    return dropCnt;

        //float modifyRate = (ConfigIntToFloat(RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.ExGemDrop)) + 1);
        //switch (motionType)
        //{
        //    case MOTION_TYPE.Normal:
        //        dropCnt = GetDropCnt(Mathf.CeilToInt(_NormalGemBase * level * _LevelGemParam * modifyRate));
        //        break;
        //    case MOTION_TYPE.Elite:
        //        dropCnt = GetDropCnt(Mathf.CeilToInt(_EliteGemBase * level * _LevelGemParam * modifyRate));
        //        break;
        //    case MOTION_TYPE.Hero:
        //        dropCnt = GetDropCnt(Mathf.CeilToInt(_BossGemBase * level * _LevelGemParam * modifyRate));
        //        break;
        //}

        //return dropCnt;

        var monAttrRecord = TableReader.MonsterAttr.GetRecord(level.ToString());
        if (monAttrRecord == null)
        {
            monAttrRecord = TableReader.MonsterAttr.GetRecord("200");
        }
        int dropCnt = 0;
        var rate = monAttrRecord.Drops[2];
        switch (motionType)
        {
            case MOTION_TYPE.Normal:
                dropCnt = GetDropCnt(Mathf.CeilToInt(_NormalGemBase * rate));
                break;
            case MOTION_TYPE.Elite:
                dropCnt = GetDropCnt(Mathf.CeilToInt(_EliteGemBase * rate));
                break;
            case MOTION_TYPE.Hero:
                dropCnt = GetDropCnt(Mathf.CeilToInt(_BossGemBase * rate));
                break;
        }
        return dropCnt;
    }

    public static int GetGemConsume(int level)
    {
        //int consumeCnt = 0;
        //consumeCnt = Mathf.CeilToInt(_GemConsumeV + _GemConsumeA * (level));
        //return consumeCnt;

        int levelID = Mathf.Clamp(level, 0, 200);
        var gemAttrRecord = TableReader.GemBaseAttr.GetRecord(levelID.ToString());
        if (gemAttrRecord == null)
        {
            gemAttrRecord = TableReader.GemBaseAttr.GetRecord("200");
        }

        return gemAttrRecord.LvUpCost;
    }

    public static int GetGemGoldConsume(int level)
    {
        //int gold = (int)(_GemConsumeGoldBase + _GemConsumeGoldStep * (level));

        //return gold;

        int levelID = Mathf.Clamp(level, 0, 200);
        var gemAttrRecord = TableReader.GemBaseAttr.GetRecord(levelID.ToString());
        if (gemAttrRecord == null)
        {
            gemAttrRecord = TableReader.GemBaseAttr.GetRecord("200");
        }

        return gemAttrRecord.LvUpCostGold;
    }

    #endregion

    #region gold

    public static float _GoldLevelParam = 10f;
    public static int _NormalGoldBase = 3500;
    public static int _EliteGoldBase = 10000;
    public static int _SpecialGoldBase = 10000;
    public static int[] _BossGoldBase = {0, 0, 3000, 3000, 4000};
    

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
        var monAttrRecord = TableReader.MonsterAttr.GetRecord(level.ToString());
        if (monAttrRecord == null)
        {
            monAttrRecord = TableReader.MonsterAttr.GetRecord("200");
        }

        float random = Random.Range(0.6f, 1.5f);
        var exGoldDrop = RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.ExGoldDrop);
        float rate = ConfigIntToFloat(exGoldDrop) + 1;
        float dropNum = ConfigIntToFloat(monAttrRecord.Attrs[1]);

        return Mathf.CeilToInt(dropNum * random * rate);
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

    public static int GetSkillLvUpGold(SkillInfoRecord skillRecord, int skillLv)
    {
        if (skillLv == 0)
            return skillRecord.CostStep[1];
        else
            return skillRecord.CostStep[2] * skillLv;
    }

    #endregion

    #endregion

    #region monster attr

    public static int _BossStageStarLevel;

    public static int GetStageLevel(int difficult, int stageIdx, STAGE_TYPE stageMode)
    {
        int diffLv = (difficult - 2) * 20;
        diffLv = Mathf.Clamp(diffLv, 0, 200);
        int level = 0;
        if (stageMode == STAGE_TYPE.NORMAL)
        {
            level = diffLv + stageIdx;
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
        int hpMax = 0;
        float hpBase = ConfigIntToFloat(monsterBase.BaseAttr[2]);
        MonsterAttrRecord attrRecord = null;
        if (!TableReader.MonsterAttr.ContainsKey(roleLv.ToString()))
        {
            if (roleLv <= 0)
            {
                attrRecord = TableReader.MonsterAttr.GetRecord("1");
            }
            else
            {
                attrRecord = TableReader.MonsterAttr.GetRecord("200");
            }
        }
        else
        {
            attrRecord = TableReader.MonsterAttr.GetRecord(roleLv.ToString());
        }
        hpMax = (int)(attrRecord.Attrs[2] * hpBase);
        //if (roleLv <= 5)
        //{
        //    hpMax = (int)(CalWeaponAttack(roleLv) * hpBase + roleLv * 15 * hpBase);
        //}
        //else if (roleLv <= 10)
        //{
        //    var hpTemp = GetMonsterHP(monsterBase, 5, monsterType);
        //    hpMax = (int)(hpTemp + (roleLv - 5) * 20 * hpBase);
        //}
        //else if (roleLv <= 20)
        //{
        //    var hpTemp = GetMonsterHP(monsterBase, 10, monsterType);
        //    hpMax = (int)(hpTemp + (roleLv - 10) * 40 * hpBase);
        //}
        //else if (roleLv <= 30)
        //{
        //    var hpTemp = GetMonsterHP(monsterBase, 20, monsterType);
        //    hpMax = (int)(hpTemp + (roleLv - 20) * 80 * hpBase);
        //}
        //else if (roleLv <= 40)
        //{
        //    var hpTemp = GetMonsterHP(monsterBase, 30, monsterType);
        //    hpMax = (int)(hpTemp + (roleLv - 30) * 80 * hpBase);
        //}
        //else if (roleLv <= 50)
        //{
        //    var hpTemp = GetMonsterHP(monsterBase, 40, monsterType);
        //    hpMax = (int)(hpTemp + (roleLv - 40) * 150 * hpBase);
        //}
        //else if (roleLv <= 60)
        //{
        //    var hpTemp = GetMonsterHP(monsterBase, 50, monsterType);
        //    hpMax = (int)(hpTemp + (roleLv - 50) * 200 * hpBase);
        //}
        //else if (roleLv <= 70)
        //{
        //    var hpTemp = GetMonsterHP(monsterBase, 60, monsterType);
        //    hpMax = (int)(hpTemp + (roleLv - 60) * 280 * hpBase);
        //}
        //else if (roleLv <= 80)
        //{
        //    var hpTemp = GetMonsterHP(monsterBase, 70, monsterType);
        //    hpMax = (int)(hpTemp + (roleLv - 70) * 400 * hpBase);
        //}
        //else if (roleLv <= 90)
        //{
        //    var hpTemp = GetMonsterHP(monsterBase, 80, monsterType);
        //    hpMax = (int)(hpTemp + (roleLv - 80) * 600 * hpBase);
        //}
        //else if (roleLv <= 100)
        //{
        //    var hpTemp = GetMonsterHP(monsterBase, 90, monsterType);
        //    hpMax = (int)(hpTemp + (roleLv - 90) * 900 * hpBase);
        //}
        //else
        //{
        //    var hpTemp = GetMonsterHP(monsterBase, 100, monsterType);
        //    hpMax = (int)(hpTemp + (roleLv - 100) * 1500 * hpBase);
        //}

        return hpMax;
    }

    public static int GetMonsterAtk(MonsterBaseRecord monsterBase, int roleLv, MOTION_TYPE monsterType)
    {
        int atkValue = 0;
        float atkBase = ConfigIntToFloat(monsterBase.BaseAttr[0]);
        MonsterAttrRecord attrRecord = null;
        if (!TableReader.MonsterAttr.ContainsKey(roleLv.ToString()))
        {
            if (roleLv <= 0)
            {
                attrRecord = TableReader.MonsterAttr.GetRecord("1");
            }
            else
            {
                attrRecord = TableReader.MonsterAttr.GetRecord("200");
            }
        }
        else
        {
            attrRecord = TableReader.MonsterAttr.GetRecord(roleLv.ToString());
        }
        atkValue = (int)(attrRecord.Attrs[0] * atkBase);
        //if (roleLv <= 10)
        //{
        //    atkValue = (int)(CalEquipTorsoHP(roleLv) * 2.0f + _HPRoleLevelBase);
        //}
        //else if (roleLv <= 20)
        //{
        //    var hpTemp = GetMonsterAtk(monsterBase, 10, monsterType);
        //    atkValue = (int)(hpTemp + (roleLv - 10) * 30);
        //}
        //else if (roleLv <= 30)
        //{
        //    var hpTemp = GetMonsterAtk(monsterBase, 20, monsterType);
        //    atkValue = (int)(hpTemp + (roleLv - 20) * 60);
        //}
        //else if (roleLv <= 40)
        //{
        //    var hpTemp = GetMonsterAtk(monsterBase, 30, monsterType);
        //    atkValue = (int)(hpTemp + (roleLv - 30) * 80);
        //}
        //else if (roleLv <= 50)
        //{
        //    var hpTemp = GetMonsterAtk(monsterBase, 40, monsterType);
        //    atkValue = (int)(hpTemp + (roleLv - 40) * 150);
        //}
        //else if (roleLv <= 60)
        //{
        //    var hpTemp = GetMonsterAtk(monsterBase, 50, monsterType);
        //    atkValue = (int)(hpTemp + (roleLv - 50) * 200);
        //}
        //else if (roleLv <= 70)
        //{
        //    var hpTemp = GetMonsterAtk(monsterBase, 60, monsterType);
        //    atkValue = (int)(hpTemp + (roleLv - 60) * 280);
        //}
        //else if (roleLv <= 80)
        //{
        //    var hpTemp = GetMonsterAtk(monsterBase, 70, monsterType);
        //    atkValue = (int)(hpTemp + (roleLv - 70) * 400);
        //}
        //else if (roleLv <= 90)
        //{
        //    var hpTemp = GetMonsterAtk(monsterBase, 80, monsterType);
        //    atkValue = (int)(hpTemp + (roleLv - 80) * 600);
        //}
        //else if (roleLv <= 100)
        //{
        //    var hpTemp = GetMonsterAtk(monsterBase, 90, monsterType);
        //    atkValue = (int)(hpTemp + (roleLv - 90) * 900);
        //}
        //else
        //{
        //    var hpTemp = GetMonsterAtk(monsterBase, 100, monsterType);
        //    atkValue = (int)(hpTemp + (roleLv - 100) * 1500);
        //}
        return atkValue;
    }

    public static int GetMonsterDef(MonsterBaseRecord monsterBase, int roleLv, MOTION_TYPE monsterType)
    {
        int defValue = 0;
        float defBase = ConfigIntToFloat(monsterBase.BaseAttr[1]);
        MonsterAttrRecord attrRecord = null;
        if (!TableReader.MonsterAttr.ContainsKey(roleLv.ToString()))
        {
            if (roleLv <= 0)
            {
                attrRecord = TableReader.MonsterAttr.GetRecord("1");
            }
            else
            {
                attrRecord = TableReader.MonsterAttr.GetRecord("200");
            }
        }
        else
        {
            attrRecord = TableReader.MonsterAttr.GetRecord(roleLv.ToString());
        }
        defValue = (int)(attrRecord.Attrs[1] * defBase);
        return defValue;
    }

    #endregion

    #region test 

    public static float GetSkillDamageRate(int skillIdx, bool isEnhance = false)
    {
        var attackRecord = Tables.TableReader.SkillInfo.GetRecord("10001");
        var skillRecord = Tables.TableReader.SkillInfo.GetRecord(TestFight.GetTestSkill(skillIdx).ToString());
        var atkData = SkillData.Instance.GetSkillInfo("10001");
        var skillData = SkillData.Instance.GetSkillInfo(TestFight.GetTestSkill(skillIdx).ToString());
        var atkDamage = GameDataValue.ConfigIntToFloat(GameDataValue.GetSkillDamageRate(atkData.SkillLevel, attackRecord.EffectValue));
        var skillDamage = GameDataValue.ConfigIntToFloat(GameDataValue.GetSkillDamageRate(skillData.SkillLevel, skillRecord.EffectValue));

        if (skillIdx == 1)
        {
            atkDamage *= 0.33f;
        }
        else if (skillIdx == 2)
        {
            atkDamage *= 0.54f;
        }
        else if (skillIdx == 3)
        {
            atkDamage *= 0.75f;
        }

        float damage = 0;

        if (isEnhance)
        {
            int eleLevel = (atkData.SkillLevel / 2) + 1;
            damage = atkDamage + 0.5f + (0.05f * eleLevel) + skillDamage * 1.26f;
        }
        else
        {
            damage = atkDamage + skillDamage;
        }

        return damage;
    }

    #endregion

}
