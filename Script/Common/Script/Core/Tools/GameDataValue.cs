using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tables;

public class GameDataValue
{
    public static float ConfigIntToFloat(int val)
    {
         return val * 0.0001f;
    }

    public static float ConfigIntToFloatDex1(int val)
    {
        int dex = Mathf.CeilToInt(val * 0.1f);
        return dex * 0.001f;
    }

    public static int ConfigFloatToInt(float val)
    {
        return (int)(val * 10000);
    }

    public static int GetMaxRate()
    {
        return 10000;
    }

    #region fight numaric

    #region level -> baseAttr

    private static int _MaxLv = 100;
    private static float _AttackPerLevel = 55.36f;
    private static float _HPPerLevel = 487.86f;
    private static float _DefencePerLevel = 24.72f;
    private static float _ValuePerLevel = 50;
    private static float _ValuePerAttack = 1;

    private static float _LegHPToTorso = 0.5f;
    private static float _LegDefenceToTorso = 0.5f;

    public static int CalWeaponAttack(int equiplevel)
    {
        var attackValue = _AttackPerLevel* equiplevel;
        return Mathf.CeilToInt(attackValue);
    }

    public static int CalEquipTorsoHP(int equiplevel)
    {
        var value = _HPPerLevel * equiplevel;
        return Mathf.CeilToInt(value);
    }

    public static int CalEquipTorsoDefence(int equiplevel)
    {
        var value = _DefencePerLevel * equiplevel;
        return Mathf.CeilToInt(value);
    }

    public static int CalEquipLegsHP(int equiplevel)
    {
        var value = _HPPerLevel * equiplevel * 0.5f;
        return Mathf.CeilToInt(value);
    }

    public static int CalEquipLegsDefence(int equiplevel)
    {
        var value = _DefencePerLevel * equiplevel * 0.5f;
        return Mathf.CeilToInt(value);
    }

    #endregion

    #region baseAttr -> exAttr atk

    private static float _AttackPerStrength = 0.25f;
    private static float _DmgEnhancePerStrength = 0.5f;
    private static float _StrToAtk = 1;

    private static float _IgnoreAtkPerDex = 0.15f;
    private static float _CriticalRatePerDex = 0.1f;
    private static float _CriticalDmgPerDex = 1.5f;
    private static float _DexToAtk = 1;

    private static float _EleAtkPerInt = 0.1f;
    private static float _EleEnhancePerInt = 0.1f;
    private static float _IntToAtk = 1;

    private static float _HPPerVit = 3;
    private static float _FinalDmgRedusePerVit = 0.3f;
    private static float _VitToAtk = 1;

    private static float _CriticalDmgToAtk = 2f;

    private static float _ElementToAtk = 1;
    private static float _DmgEnhancePerElementEnhance = 10;
    private static float _EleEnhanceToAtk = 0.3f;
    private static float _EleResistToAtk = 0.3f;

    private static float _IgnoreDefenceToAtk = 0.5f;

    private static float _HpToAtk = 11.0f;
    private static float _DefToAtk = 0.5f;
    private static float _MoveSpeedToAtk = 1.35f;
    private static float _AtkSpeedToAtk = 1;
    private static float _CriticalChanceToAtk = 1;
    private static float _DamageEnhance = 1;

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

        return value * _ValuePerAttack;
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
        return skillLv * skillParam[0];
    }

    #endregion

    #endregion

    #region product & consume

    #region exp

    public static int _NormalExpBase = 50;
    public static int _EliteExpBase = 250;
    public static int _SpecialExpBase = 500;
    public static int _BossExpBase = 1250;

    public static int _LevelExpBase = 10000;
    public static float _Level30ExpRate = 0.1f;
    public static float _Level60ExpRate = 0.11f;
    public static float _Level90ExpRate = 0.12f;
    public static float _Level100ExpRate = 0.15f;
    public static float _Level999ExpRate = 0.2f;

    public static int GetLvUpExp(int playerLv, int attrLv)
    {
        int realLv = playerLv + attrLv;
        int levelExp = 0;
        if (realLv <= 30)
        {
            levelExp = Mathf.CeilToInt((1 + _Level30ExpRate * realLv) * _LevelExpBase);
        }
        else if (realLv <= 60)
        {
            levelExp = Mathf.CeilToInt((1 + _Level60ExpRate * realLv) * _LevelExpBase);
        }
        else if (realLv <= 90)
        {
            levelExp = Mathf.CeilToInt((1 + _Level90ExpRate * realLv) * _LevelExpBase);
        }
        else if (realLv <= 100)
        {
            levelExp = Mathf.CeilToInt((1 + _Level100ExpRate * realLv) * _LevelExpBase);
        }
        else
        {
            levelExp = Mathf.CeilToInt((1 + _Level999ExpRate * realLv) * _LevelExpBase);
        }

        return levelExp;
    }

    public static int GetMonsterExp(MotionType motionType, int level, int playerLv)
    {
        int levelDelta = Mathf.Clamp(playerLv - level,0, 10);
        int monExpLevel = Mathf.Clamp(level, 1, 100);
        int expBase = 0;
        switch (motionType)
        {
            case MotionType.Normal:
                expBase = _NormalExpBase;
                break;
            case MotionType.Elite:
                expBase = _EliteExpBase;
                break;
            case MotionType.Special:
                expBase = _SpecialExpBase;
                break;
            case MotionType.Hero:
                expBase = _BossExpBase;
                break;
        }
        int levelExp = 0;
        if (monExpLevel <= 30)
        {
            levelExp = Mathf.CeilToInt((1 + _Level30ExpRate * monExpLevel) * expBase);
        }
        else if (monExpLevel <= 60)
        {
            levelExp = Mathf.CeilToInt((1 + _Level60ExpRate * monExpLevel) * expBase);
        }
        else if (monExpLevel <= 90)
        {
            levelExp = Mathf.CeilToInt((1 + _Level90ExpRate * monExpLevel) * expBase);
        }
        else if (monExpLevel <= 100)
        {
            levelExp = Mathf.CeilToInt((1 + _Level100ExpRate * monExpLevel) * expBase);
        }
        else
        {
            levelExp = Mathf.CeilToInt((1 + _Level999ExpRate * monExpLevel) * expBase);
        }

        levelExp = Mathf.CeilToInt(levelExp * (1 + levelDelta * 0.1f));
        return levelExp;
    }

    #endregion

    #region equip

    private static int _EqiupLvWeapon = 1;
    private static int _EqiupLvTorso = 2;
    private static int _EqiupLvShoes = 3;
    private static int _EqiupLvRing = 4;
    private static int _EqiupLvAmulate = 4;

    private static List<ITEM_QUALITY> GetDropQualitys(MotionType motionType, MonsterBaseRecord monsterRecord, int level, int dropActType = 1)
    {
        List<ITEM_QUALITY> dropEquipQualitys = new List<ITEM_QUALITY>();
        int dropCnt = 0;
        int dropQuality = 0;
        switch (motionType)
        {
            case MotionType.Normal:
                dropCnt = GameRandom.GetRandomLevel(95, 5);
                for (int i = 0; i < dropCnt; ++i)
                {
                    dropQuality = GameRandom.GetRandomLevel(70, 30);
                    dropEquipQualitys.Add((ITEM_QUALITY)dropQuality);
                }
                
                break;
            case MotionType.Elite:
                dropCnt = GameRandom.GetRandomLevel(10, 70, 20);
                for (int i = 0; i < dropCnt; ++i)
                {
                    dropQuality = GameRandom.GetRandomLevel(30, 65, 5);
                    dropEquipQualitys.Add((ITEM_QUALITY)dropQuality);
                }

                break;
            case MotionType.Special:
                dropCnt = GameRandom.GetRandomLevel(10, 70, 20);
                for (int i = 0; i < dropCnt; ++i)
                {
                    dropQuality = GameRandom.GetRandomLevel(30, 65, 5);
                    dropEquipQualitys.Add((ITEM_QUALITY)dropQuality);
                }

                break;
            case MotionType.Hero:
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

    public static List<ItemEquip> GetMonsterDropEquipNormal(MotionType motionType, MonsterBaseRecord monsterRecord, int level, int dropActType = 1)
    {
        List<ItemEquip> dropEquipList = new List<ItemEquip>();
        var dropEquipQualitys = GetDropQualitys(motionType, monsterRecord, level, dropActType);
        if (dropEquipQualitys.Count == 0)
            return dropEquipList;
        
        for (int i = 0; i < dropEquipQualitys.Count; ++i)
        {
            if (dropEquipQualitys[i] == ITEM_QUALITY.ORIGIN)
            {
                if (monsterRecord.SpDrops.Count == 0)
                    continue;

                int dropIdx = Random.Range(0, monsterRecord.ValidSpDrops.Count);
                var dropItem = monsterRecord.ValidSpDrops[dropIdx];
                var dropEquipTab = TableReader.EquipItem.GetRecord(dropItem.Id);
                var equipLevel = GetEquipLv(dropEquipTab.Slot, level);
                var equipValue = CalLvValue(level);
                var dropEquip = ItemEquip.CreateEquip(level, dropEquipQualitys[i], equipValue, int.Parse(dropItem.Id), (int)dropEquipTab.Slot);
                dropEquipList.Add(dropEquip);
            }
            else
            {
                var equipSlot = GetRandomItemSlot(dropEquipQualitys[i]);
                var equipLevel = GetEquipLv(equipSlot, level);
                var equipValue = CalLvValue(level);
                var dropEquip = ItemEquip.CreateEquip(level, dropEquipQualitys[i], equipValue, -1, (int)equipSlot);
                dropEquipList.Add(dropEquip);
            }
        }

        return dropEquipList;
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
    public static int _ConsumeOnTime = 50;

    public static float _LevelParam = 0.01f;
    public static int _NormalMatBase = 2500;
    public static int _EliteMatBase = 10000;
    public static int _SpecialMatBase = 10000;
    public static int _BossMatBase = 100000;

    public static int GetEquipMatDropCnt(MotionType motionType, MonsterBaseRecord monsterRecord, int level)
    {
        int dropCnt = 0;
        if (level < _DropMatLevel)
            return dropCnt;

        switch (motionType)
        {
            case MotionType.Normal:
                dropCnt = GetDropCnt(Mathf.CeilToInt( _NormalMatBase * level * _LevelParam));
                break;
            case MotionType.Elite:
                dropCnt = GetDropCnt(Mathf.CeilToInt(_EliteMatBase * level * _LevelParam));
                break;
            case MotionType.Special:
                dropCnt = GetDropCnt(Mathf.CeilToInt(_SpecialMatBase * level * _LevelParam));
                break;
            case MotionType.Hero:
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
        return _ConsumeOnTime;
    }

    public static int GetDestoryGetMatCnt(ItemEquip equip)
    {
        if (equip.EquipLevel < _DropMatLevel)
            return 0;

        if (equip.EquipLevel < 100)
        {
            if (equip.EquipQuality == ITEM_QUALITY.PURPER)
                return Mathf.CeilToInt(_ConsumeOnTime * 0.8f);
            else if (equip.EquipQuality == ITEM_QUALITY.ORIGIN)
                return _ConsumeOnTime * 3;
        }
        else
        {
            if (equip.EquipQuality == ITEM_QUALITY.PURPER)
                return _ConsumeOnTime;
            else if (equip.EquipQuality == ITEM_QUALITY.ORIGIN)
                return _ConsumeOnTime * 5;
        }

        return 0;
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

    public static int GetGemMatDropCnt(MotionType motionType, MonsterBaseRecord monsterRecord, int level)
    {
        int dropCnt = 0;
        if (level < _DropGemLevel)
            return dropCnt;

        switch (motionType)
        {
            case MotionType.Normal:
                dropCnt = GetDropCnt(Mathf.CeilToInt(_NormalGemBase * level * _LevelParam));
                break;
            case MotionType.Elite:
                dropCnt = GetDropCnt(Mathf.CeilToInt(_EliteGemBase * level * _LevelParam));
                break;
            case MotionType.Special:
                dropCnt = GetDropCnt(Mathf.CeilToInt(_SpecialGemBase * level * _LevelParam));
                break;
            case MotionType.Hero:
                dropCnt = GetDropCnt(Mathf.CeilToInt(_BossGemBase * level * _LevelParam));
                break;
        }

        return dropCnt;
    }

    public static int GetGemConsume(int level)
    {
        int consumeCnt = Mathf.CeilToInt( _GemConsumeV  + _GemConsumeA * level);
        return consumeCnt;
    }

    #endregion

    #endregion
}
