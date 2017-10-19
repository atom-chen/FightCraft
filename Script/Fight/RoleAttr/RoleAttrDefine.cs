 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoleAttrEnum
{
    None,
    Strength = 1,
    Dexterity,
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

    FireDamageReduse,
    ColdDamageReduse,
    LightingDamageReduse,
    WindDamageReduse,
    FireAbsorbsPersent,
    ColdAbsorbsPersent,
    LightingAbsorbsPersent,
    WindAbsorbsPersent,

    IgnoreDefenceAttack,
    FinalDamageReduse,

    Skill1FireDamagePersent = 200,
    Skill2FireDamagePersent,
    Skill3FireDamagePersent,
    Skill1ColdDamagePersent,
    Skill2ColdDamagePersent,
    Skill3ColdDamagePersent,
    Skill1LightingDamagePersent,
    Skill2LightingDamagePersent,
    Skill3LightingDamagePersent,
    Skill1WindDamagePersent,
    Skill2WindDamagePersent,
    Skill3WindDamagePersent,

    BASE_ATTR_MAX = 999,

    Skill1FireBoom = 1000,
    Skill1FireBall,
    Skill1FireExplore,
    Skill1FireRandomBoom,
    Skill1FireAimTarget,
    Skill2FireBoom,
    Skill2FireBall,
    Skill2FireExplore,
    Skill2FireRandomBoom,
    Skill2FireAimTarget,
    Skill3FireBoom,
    Skill3FireBall,
    Skill3FireExplore,
    Skill3FireRandomBoom,
    Skill3FireAimTarget,
    Skill1IceBoom,
    Skill1IceBall,
    Skill1IceExplore,
    Skill1IceRandomBoom,
    Skill1IceAimTarget,
    Skill2IceBoom,
    Skill2IceBall,
    Skill2IceExplore,
    Skill2IceRandomBoom,
    Skill2IceAimTarget,
    Skill3IceBoom,
    Skill3IceBall,
    Skill3IceExplore,
    Skill3IceRandomBoom,
    Skill3IceAimTarget,
    Skill1LightBoom,
    Skill1LightBall,
    Skill1LightExplore,
    Skill1LightRandomBoom,
    Skill1LightAimTarget,
    Skill2LightBoom,
    Skill2LightBall,
    Skill2LightExplore,
    Skill2LightRandomBoom,
    Skill2LightAimTarget,
    Skill3LightBoom,
    Skill3LightBall,
    Skill3LightExplore,
    Skill3LightRandomBoom,
    Skill3LightAimTarget,
    Skill1WindBoom,
    Skill1WindBall,
    Skill1WindExplore,
    Skill1WindRandomBoom,
    Skill1WindAimTarget,
    Skill2WindBoom,
    Skill2WindBall,
    Skill2WindExplore,
    Skill2WindRandomBoom,
    Skill2WindAimTarget,
    Skill3WindBoom,
    Skill3WindBall,
    Skill3WindExplore,
    Skill3WindRandomBoom,
    Skill3WindAimTarget,

    SkillSP1,
    SkillSP2,
    SkillSP3,
    SkillSP4,
    SkillSP5,
    SkillSP6,
    SkillSP7,
    SkillSP8,
    SkillSP9,

    AttackSkillLevel,
    ExSkillLevel,
    BuffSkillLevel,
    DodgeSkillLevel,
    DefenceSkillLevel,
}

public class AttrDisplay
{
    public static string GetAttrDisplayStr(RoleAttrEnum attrEnum, int subClass, int value1)
    {
        string attrStr = "";
        if ((int)attrEnum >= (int)RoleAttrEnum.Skill1FireDamagePersent
            && (int)attrEnum <= (int)RoleAttrEnum.Skill3WindDamagePersent)
        {
            attrStr = Tables.StrDictionary.GetFormatStr((int)attrEnum, value1 / 100.0f);
            return attrStr;
        }

        if ((int)attrEnum >= (int)RoleAttrEnum.Skill1FireBoom
            && (int)attrEnum <= (int)RoleAttrEnum.Skill3WindAimTarget)
        {
            attrStr = Tables.StrDictionary.GetFormatStr((int)attrEnum);
            return attrStr;
        }

        switch (attrEnum)
        {
            case RoleAttrEnum.HPMaxPersent:
            case RoleAttrEnum.AttackPersent:
            case RoleAttrEnum.DefensePersent:
            case RoleAttrEnum.MoveSpeed:
            case RoleAttrEnum.AttackSpeed:
            case RoleAttrEnum.CriticalHitChance:
            case RoleAttrEnum.CriticalHitDamge:
            case RoleAttrEnum.FireAbsorbsPersent:
            case RoleAttrEnum.ColdAbsorbsPersent:
            case RoleAttrEnum.LightingAbsorbsPersent:
            case RoleAttrEnum.WindAbsorbsPersent:
                attrStr = Tables.StrDictionary.GetFormatStr((int)attrEnum, value1 / 100.0f);
                break;
            default:
                attrStr = Tables.StrDictionary.GetFormatStr((int)attrEnum, value1);
                break;
        }

        return attrStr;
    }
}

public class RandomAttrs
{

    public class RandomAttrInfo
    {
        public RoleAttrEnum AttrID;
        public bool CanRepeat;
        public int BaseValue;
        public int Step;

        public RandomAttrInfo()
        {

        }

        public RandomAttrInfo(RoleAttrEnum attr, bool repeat, int baseValue, int step)
        {
            AttrID = attr;
            CanRepeat = repeat;
            Step = step;
            BaseValue = baseValue;
        }

        public EquipExAttr GetAttrRandom(int value)
        {
            EquipExAttr exAttr = new EquipExAttr();
            exAttr.AttrID = AttrID;
            float randomValue = (value * Random.Range(0.85f, 1.15f));
            int attrValue = BaseValue + (int)(randomValue * Step);
            exAttr.AttrValue1 = attrValue;
            return exAttr;
        }
    }
    public static List<EquipExAttr> GetRandomEquipExAttrs(Tables.EQUIP_SLOT equipSlot, int level, int equipValue, Tables.ITEM_QUALITY quality, Tables.PROFESSION profession)
    {
        List<EquipExAttr> exAttrs = new List<EquipExAttr>();

        switch (equipSlot)
        {
            case Tables.EQUIP_SLOT.WEAPON:
                return GetWeaponRandomAttr(level, equipValue, quality, profession);
            case Tables.EQUIP_SLOT.TORSO:
                return GetTorseRandomAttr(level, equipValue, quality, profession);
            case Tables.EQUIP_SLOT.LEGS:
                return GetLegsRandomAttr(level, equipValue, quality, profession);
            case Tables.EQUIP_SLOT.AMULET:
                return GetAmuletRandomAttr(level, equipValue, quality, profession);
            case Tables.EQUIP_SLOT.RING:
                return GetAmuletRandomAttr(level, equipValue, quality, profession);
        }

        return exAttrs;
    }

    #region weapon

    static List<RandomAttrInfo> _WeaponRandomBase = new List<RandomAttrInfo>()
    {
        new RandomAttrInfo(RoleAttrEnum.Attack, true, 0, 5),
        new RandomAttrInfo(RoleAttrEnum.AttackPersent, true, 0, 100),
    };

    static List<RandomAttrInfo> _WeaponRandomEx = new List<RandomAttrInfo>()
    {
        new RandomAttrInfo(RoleAttrEnum.Strength, true, 0, 1),
        new RandomAttrInfo(RoleAttrEnum.Dexterity, true, 0, 1),
        new RandomAttrInfo(RoleAttrEnum.Vitality, true, 0, 1),
        new RandomAttrInfo(RoleAttrEnum.Attack, true, 0, 5),
        new RandomAttrInfo(RoleAttrEnum.AttackSpeed, false, 0, 10),
        new RandomAttrInfo(RoleAttrEnum.CriticalHitChance, false, 0, 10),
        new RandomAttrInfo(RoleAttrEnum.CriticalHitDamge, true, 0, 10),
        new RandomAttrInfo(RoleAttrEnum.FireAttackAdd, true, 0, 3),
        new RandomAttrInfo(RoleAttrEnum.ColdAttackAdd, true, 0, 3),
        new RandomAttrInfo(RoleAttrEnum.LightingAttackAdd, true, 0, 3),
        new RandomAttrInfo(RoleAttrEnum.WindAttackAdd, true, 0, 3),

        new RandomAttrInfo(RoleAttrEnum.Skill1FireDamagePersent, false, 0, 5),
        new RandomAttrInfo(RoleAttrEnum.Skill2FireDamagePersent, false, 0, 5),
        new RandomAttrInfo(RoleAttrEnum.Skill3FireDamagePersent, false, 0, 5),
        new RandomAttrInfo(RoleAttrEnum.Skill1ColdDamagePersent, false, 0, 5),
        new RandomAttrInfo(RoleAttrEnum.Skill2ColdDamagePersent, false, 0, 5),
        new RandomAttrInfo(RoleAttrEnum.Skill3ColdDamagePersent, false, 0, 5),
        new RandomAttrInfo(RoleAttrEnum.Skill1LightingDamagePersent, false, 0, 5),
        new RandomAttrInfo(RoleAttrEnum.Skill2LightingDamagePersent, false, 0, 5),
        new RandomAttrInfo(RoleAttrEnum.Skill3LightingDamagePersent, false, 0, 5),
        new RandomAttrInfo(RoleAttrEnum.Skill1WindDamagePersent, false, 0, 5),
        new RandomAttrInfo(RoleAttrEnum.Skill2WindDamagePersent, false, 0, 5),
        new RandomAttrInfo(RoleAttrEnum.Skill3WindDamagePersent, false, 0, 5),
    };

    private static List<EquipExAttr> GetWeaponRandomAttr(int level, int equipValue, Tables.ITEM_QUALITY quality, Tables.PROFESSION profession)
    {
        List<EquipExAttr> exAttrs = new List<EquipExAttr>();

        int exAttrCnt = 0;
        if (quality == Tables.ITEM_QUALITY.BLUE)
        {
            exAttrCnt = Random.Range(1, 3);
            if (equipValue < level)
            {
                exAttrCnt = 2;
            }
        }
        else if (quality == Tables.ITEM_QUALITY.PURPER)
        {
            exAttrCnt = Random.Range(3, 5);
            if (equipValue < level)
            {
                exAttrCnt = 4;
            }
        }
        else if (quality == Tables.ITEM_QUALITY.ORIGIN)
        {
            exAttrCnt = 5;
        }

        EquipExAttr defaultBase = null;
        EquipExAttr defaultEx = null;
        EquipExAttr defaultSp = null;

        int defaultCnt = 0;
        if (exAttrCnt > 3)
        {
            defaultBase = GetRandomAttr(_WeaponRandomBase, 1, equipValue)[0];
            //if (exAttrCnt == 4)
            {
                defaultCnt += 2;
                //int attrIDX = Random.Range((int)RoleAttrEnum.Skill1FireBoom, (int)RoleAttrEnum.Skill3WindAimTarget + 1);
                //defaultEx = new EquipExAttr((RoleAttrEnum)attrIDX, 1, 1);
                defaultEx = RoleAttrImpactEleBullet.GetRandomExAttr(level, equipValue, quality);
            }
            if (exAttrCnt == 5)
            {
                defaultCnt += 1;
                int attrIDX = Random.Range((int)RoleAttrEnum.SkillSP1, (int)RoleAttrEnum.SkillSP9 + 1);
                defaultSp = new EquipExAttr((RoleAttrEnum)attrIDX, 1, 1);
            }
        }

        var baseRandoms = GetRandomAttr(_WeaponRandomEx, exAttrCnt - defaultCnt, equipValue);

        if (defaultBase != null)
        {
            exAttrs.Add(defaultBase);
        }
        exAttrs.AddRange(baseRandoms);
        if (defaultEx != null)
        {
            exAttrs.Add(defaultEx);
        }
        if (defaultSp != null)
        {
            exAttrs.Add(defaultSp);
        }

        return exAttrs;
    }

    private static List<EquipExAttr> GetRandomAttr(List<RandomAttrInfo> randomList, int randomCnt, int equipValue)
    {
        List<EquipExAttr> randomExs = new List<EquipExAttr>();
        List<int> randomNums = new List<int>();
        for (int i = 0; i < randomList.Count; ++i)
        {
            randomNums.Add(i);
        }

        for (int i = 0; i < randomCnt; ++i)
        {
            var randomIdx = Random.Range(0, randomNums.Count);
            randomExs.Add(randomList[randomNums[randomIdx]].GetAttrRandom(equipValue));
            if (!randomList[randomNums[randomIdx]].CanRepeat)
            {
                randomNums.Remove(randomIdx);
            }
        }

        return randomExs;
    }

    #endregion

    #region torse & legs

    static List<RandomAttrInfo> _TorseRandomBase = new List<RandomAttrInfo>()
    {
        new RandomAttrInfo(RoleAttrEnum.Defense, true, 0, 5),
        new RandomAttrInfo(RoleAttrEnum.DefensePersent, true, 0, 100),
        new RandomAttrInfo(RoleAttrEnum.HPMax, true, 0, 5),
        new RandomAttrInfo(RoleAttrEnum.HPMaxPersent, true, 0, 100),
    };

    static List<RandomAttrInfo> _LegRandomBase = new List<RandomAttrInfo>()
    {
        new RandomAttrInfo(RoleAttrEnum.MoveSpeed, true, 0, 5),
    };

    static List<RandomAttrInfo> _TorseRandomEx = new List<RandomAttrInfo>()
    {
        new RandomAttrInfo(RoleAttrEnum.Strength, true, 0, 1),
        new RandomAttrInfo(RoleAttrEnum.Dexterity, true, 0, 1),
        new RandomAttrInfo(RoleAttrEnum.Vitality, true, 0, 1),
        new RandomAttrInfo(RoleAttrEnum.Defense, true, 0, 1),
        new RandomAttrInfo(RoleAttrEnum.HPMax, true, 0, 1),
        new RandomAttrInfo(RoleAttrEnum.CriticalHitChance, false, 0, 10),
        new RandomAttrInfo(RoleAttrEnum.CriticalHitDamge, true, 0, 10),
        new RandomAttrInfo(RoleAttrEnum.FireResistan, true, 0, 3),
        new RandomAttrInfo(RoleAttrEnum.ColdResistan, true, 0, 3),
        new RandomAttrInfo(RoleAttrEnum.LightingResistan, true, 0, 3),
        new RandomAttrInfo(RoleAttrEnum.WindResistan, true, 0, 3),
    };

    private static List<EquipExAttr> GetTorseRandomAttr(int level, int equipValue, Tables.ITEM_QUALITY quality, Tables.PROFESSION profession)
    {
        List<EquipExAttr> exAttrs = new List<EquipExAttr>();

        int exAttrCnt = 0;
        if (quality == Tables.ITEM_QUALITY.BLUE)
        {
            exAttrCnt = 1;
        }
        else if (quality == Tables.ITEM_QUALITY.PURPER)
        {
            exAttrCnt = Random.Range(2, 4);
            if (equipValue < level)
            {
                exAttrCnt = 3;
            }
        }
        else if (quality == Tables.ITEM_QUALITY.ORIGIN)
        {
            exAttrCnt = 4;
        }

        EquipExAttr defaultBase = null;
        EquipExAttr defaultEx = null;
        EquipExAttr defaultSp = null;

        int defaultCnt = 0;
        if (exAttrCnt > 2)
        {
            defaultBase = GetRandomAttr(_TorseRandomBase, 1, equipValue)[0];
            defaultCnt = 1;
            //if (exAttrCnt == 4)
            //{
            //    defaultCnt += 2;
            //    int attrIDX = Random.Range((int)RoleAttrEnum.Skill1FireBoom, (int)RoleAttrEnum.Skill3WindAimTarget + 1);
            //    defaultEx = new EquipExAttr((RoleAttrEnum)attrIDX, 1, 1);
            //}
            //if (exAttrCnt == 5)
            //{
            //    defaultCnt += 1;
            //    int attrIDX = Random.Range((int)RoleAttrEnum.SkillSP1, (int)RoleAttrEnum.SkillSP9 + 1);
            //    defaultSp = new EquipExAttr((RoleAttrEnum)attrIDX, 1, 1);
            //}
        }

        var baseRandoms = GetRandomAttr(_TorseRandomEx, exAttrCnt - defaultCnt, equipValue);

        if (defaultBase != null)
        {
            exAttrs.Add(defaultBase);
        }
        exAttrs.AddRange(baseRandoms);
        if (defaultEx != null)
        {
            exAttrs.Add(defaultEx);
        }
        if (defaultSp != null)
        {
            exAttrs.Add(defaultSp);
        }

        return exAttrs;
    }

    private static List<EquipExAttr> GetLegsRandomAttr(int level, int equipValue, Tables.ITEM_QUALITY quality, Tables.PROFESSION profession)
    {
        List<EquipExAttr> exAttrs = new List<EquipExAttr>();

        int exAttrCnt = 0;
        if (quality == Tables.ITEM_QUALITY.BLUE)
        {
            exAttrCnt = 1;
        }
        else if (quality == Tables.ITEM_QUALITY.PURPER)
        {
            exAttrCnt = Random.Range(2, 4);
            if (equipValue < level)
            {
                exAttrCnt = 3;
            }
        }
        else if (quality == Tables.ITEM_QUALITY.ORIGIN)
        {
            exAttrCnt = 4;
        }

        EquipExAttr defaultBase = null;
        EquipExAttr defaultEx = null;
        EquipExAttr defaultSp = null;

        int defaultCnt = 0;
        if (exAttrCnt > 2)
        {
            defaultBase = GetRandomAttr(_LegRandomBase, 1, equipValue)[0];
            defaultCnt = 1;
            //if (exAttrCnt == 4)
            //{
            //    defaultCnt += 2;
            //    int attrIDX = Random.Range((int)RoleAttrEnum.Skill1FireBoom, (int)RoleAttrEnum.Skill3WindAimTarget + 1);
            //    defaultEx = new EquipExAttr((RoleAttrEnum)attrIDX, 1, 1);
            //}
            //if (exAttrCnt == 5)
            //{
            //    defaultCnt += 1;
            //    int attrIDX = Random.Range((int)RoleAttrEnum.SkillSP1, (int)RoleAttrEnum.SkillSP9 + 1);
            //    defaultSp = new EquipExAttr((RoleAttrEnum)attrIDX, 1, 1);
            //}
        }

        var baseRandoms = GetRandomAttr(_TorseRandomEx, exAttrCnt - defaultCnt, equipValue);

        if (defaultBase != null)
        {
            exAttrs.Add(defaultBase);
        }
        exAttrs.AddRange(baseRandoms);
        if (defaultEx != null)
        {
            exAttrs.Add(defaultEx);
        }
        if (defaultSp != null)
        {
            exAttrs.Add(defaultSp);
        }

        return exAttrs;
    }

    #endregion

    #region amulet & ring

    static List<RandomAttrInfo> _AmuletRandomEx = new List<RandomAttrInfo>()
    {
        new RandomAttrInfo(RoleAttrEnum.Strength, true, 0, 1),
        new RandomAttrInfo(RoleAttrEnum.Dexterity, true, 0, 1),
        new RandomAttrInfo(RoleAttrEnum.Vitality, true, 0, 1),

        new RandomAttrInfo(RoleAttrEnum.Attack, true, 0, 1),
        new RandomAttrInfo(RoleAttrEnum.Defense, true, 0, 1),
        new RandomAttrInfo(RoleAttrEnum.HPMax, true, 0, 1),

        new RandomAttrInfo(RoleAttrEnum.CriticalHitChance, false, 0, 10),
        new RandomAttrInfo(RoleAttrEnum.CriticalHitDamge, true, 0, 10),
        new RandomAttrInfo(RoleAttrEnum.FireResistan, true, 0, 3),
        new RandomAttrInfo(RoleAttrEnum.ColdResistan, true, 0, 3),
        new RandomAttrInfo(RoleAttrEnum.LightingResistan, true, 0, 3),
        new RandomAttrInfo(RoleAttrEnum.WindResistan, true, 0, 3),

        new RandomAttrInfo(RoleAttrEnum.FireAttackAdd, true, 0, 3),
        new RandomAttrInfo(RoleAttrEnum.ColdAttackAdd, true, 0, 3),
        new RandomAttrInfo(RoleAttrEnum.LightingAttackAdd, true, 0, 3),
        new RandomAttrInfo(RoleAttrEnum.WindAttackAdd, true, 0, 3),

        new RandomAttrInfo(RoleAttrEnum.Skill1FireDamagePersent, false, 0, 5),
        new RandomAttrInfo(RoleAttrEnum.Skill2FireDamagePersent, false, 0, 5),
        new RandomAttrInfo(RoleAttrEnum.Skill3FireDamagePersent, false, 0, 5),
        new RandomAttrInfo(RoleAttrEnum.Skill1ColdDamagePersent, false, 0, 5),
        new RandomAttrInfo(RoleAttrEnum.Skill2ColdDamagePersent, false, 0, 5),
        new RandomAttrInfo(RoleAttrEnum.Skill3ColdDamagePersent, false, 0, 5),
        new RandomAttrInfo(RoleAttrEnum.Skill1LightingDamagePersent, false, 0, 5),
        new RandomAttrInfo(RoleAttrEnum.Skill2LightingDamagePersent, false, 0, 5),
        new RandomAttrInfo(RoleAttrEnum.Skill3LightingDamagePersent, false, 0, 5),
        new RandomAttrInfo(RoleAttrEnum.Skill1WindDamagePersent, false, 0, 5),
        new RandomAttrInfo(RoleAttrEnum.Skill2WindDamagePersent, false, 0, 5),
        new RandomAttrInfo(RoleAttrEnum.Skill3WindDamagePersent, false, 0, 5),
    };

    private static List<EquipExAttr> GetAmuletRandomAttr(int level, int equipValue, Tables.ITEM_QUALITY quality, Tables.PROFESSION profession)
    {
        List<EquipExAttr> exAttrs = new List<EquipExAttr>();

        int exAttrCnt = 0;
        if (quality == Tables.ITEM_QUALITY.BLUE)
        {
            exAttrCnt = 1;
        }
        else if (quality == Tables.ITEM_QUALITY.PURPER)
        {
            exAttrCnt = Random.Range(2, 4);
            if (equipValue < level)
            {
                exAttrCnt = 3;
            }
        }
        else if (quality == Tables.ITEM_QUALITY.ORIGIN)
        {
            exAttrCnt = 4;
        }

        EquipExAttr defaultBase = null;
        EquipExAttr defaultEx = null;
        EquipExAttr defaultSp = null;

        int defaultCnt = 0;
        if (exAttrCnt > 2)
        {
            //defaultBase = GetRandomAttr(_AmuletRandomEx, 1, equipValue)[0];
            //defaultCnt = 1;
            //if (exAttrCnt == 4)
            //{
            //    defaultCnt += 2;
            //    int attrIDX = Random.Range((int)RoleAttrEnum.Skill1FireBoom, (int)RoleAttrEnum.Skill3WindAimTarget + 1);
            //    defaultEx = new EquipExAttr((RoleAttrEnum)attrIDX, 1, 1);
            //}
            //if (exAttrCnt == 5)
            //{
            //    defaultCnt += 1;
            //    int attrIDX = Random.Range((int)RoleAttrEnum.SkillSP1, (int)RoleAttrEnum.SkillSP9 + 1);
            //    defaultSp = new EquipExAttr((RoleAttrEnum)attrIDX, 1, 1);
            //}
        }

        var baseRandoms = GetRandomAttr(_AmuletRandomEx, exAttrCnt - defaultCnt, equipValue);

        if (defaultBase != null)
        {
            exAttrs.Add(defaultBase);
        }
        exAttrs.AddRange(baseRandoms);
        if (defaultEx != null)
        {
            exAttrs.Add(defaultEx);
        }
        if (defaultSp != null)
        {
            exAttrs.Add(defaultSp);
        }

        return exAttrs;
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
            existAttr.AddData(exAttr);
        }
        
    }
}