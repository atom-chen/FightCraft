using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    #region numaric

    #region level -> baseAttr

    private static float _AttackPerLevel = 11.36f;
    private static float _HPPerLevel = 99.86f;
    private static float _DefencePerLevel = 6.72f;
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

    private static float _AttackPerStrength = 0.5f;
    private static float _DmgEnhancePerStrength = 0.0005f;
    private static float _StrToAtk = 1;

    private static float _IgnoreAtkPerDex = 0.25f;
    private static float _CriticalDmgPerDex = 0.001f;
    private static float _DexToAtk = 1;

    private static float _EleAtkPerInt = 0.1f;
    private static float _EleEnhancePerInt = 0.3f;
    private static float _IntToAtk = 1;

    private static float _HPPerVit = 3;
    private static float _FinalDmgRedusePerVit = 0.3f;
    private static float _VitToAtk = 1;

    private static float _CriticalDmgToAtk = 5;

    private static float _ElementToAtk = 1;
    private static float _DmgEnhancePerElementEnhance = 0.003f;
    private static float _EleEnhanceToAtk = 3;
    private static float _EleResistToAtk = 3;

    private static float _IgnoreDefenceToAtk = 2f;

    private static float _HpToAtk = 0.1f;
    private static float _DefToAtk = 2f;
    private static float _MoveSpeedToAtk = 1;
    private static float _AtkSpeedToAtk = 1;
    private static float _CriticalChanceToAtk = 1;

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
            attrValue = Mathf.Min(value, 10000);
        }
        else if (roleAttr == RoleAttrEnum.HPMaxPersent)
        {
            attrValue = Mathf.Min(value, 10000);
        }
        else
        {
            attrValue = Mathf.CeilToInt(value / GetAttrToValue(roleAttr));
            if (roleAttr == RoleAttrEnum.AttackSpeed)
            {
                attrValue = Mathf.Min(attrValue, 1000);
            }
            else if (roleAttr == RoleAttrEnum.MoveSpeed)
            {
                attrValue = Mathf.Min(attrValue, 2000);
            }
            else if (roleAttr == RoleAttrEnum.CriticalHitChance)
            {
                attrValue = Mathf.Min(attrValue, 1000);
            }
        }
        return attrValue;
    }
    #endregion

    #region ex -> base

    private float _ExToBase = 0.2f;

    #endregion

    #region equip

    public static int GetExAttrRandomValue(RoleAttrEnum roleAttr, int baseValue, float lowPersent = 0.4f, float upPersent = 0.6f)
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
        else
        {
            return Mathf.CeilToInt(baseValue * randomValue);
        }
    }

    public static List<RoleAttrEnum> GetRandomEquipAttrsType(Tables.EQUIP_SLOT equipSlot, Tables.ITEM_QUALITY quality)
    {
        List<EquipExAttr> exAttrs = new List<EquipExAttr>();

        int exAttrCnt = 0;
        
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
                if (randomVar > temp)
                {
                    attr = attrRandom;
                    break;
                }
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

    public class EquipExAttrRandom
    {
        public RoleAttrEnum AttrID;
        public bool CanRepeat;
        public int MaxValue;
        public int Random;

        public EquipExAttrRandom(RoleAttrEnum attr, bool repeat, int maxValue, int randomVal)
        {
            AttrID = attr;
            CanRepeat = repeat;
            MaxValue = maxValue;
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
        new EquipExAttrRandom(RoleAttrEnum.AttackSpeed, false, 1000, 10),
        new EquipExAttrRandom(RoleAttrEnum.CriticalHitChance, false, 1000, 10),
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
        new EquipExAttrRandom(RoleAttrEnum.AttackSpeed, false, 1000, 10),
        new EquipExAttrRandom(RoleAttrEnum.CriticalHitChance, false, 1000, 10),
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

    #endregion


}
