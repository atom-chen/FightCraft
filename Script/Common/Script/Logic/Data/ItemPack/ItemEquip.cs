using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Tables;
using System;

public class EquipExAttr
{
    public static EquipExAttr GetBaseExAttr(RoleAttrEnum roleAttr, int value)
    {
        EquipExAttr exAttr = new EquipExAttr();
        exAttr.AttrType = "RoleAttrImpactBaseAttr";
        exAttr.Value = value;
        exAttr.AttrParams.Add((int)roleAttr);
        exAttr.AttrParams.Add(Mathf.CeilToInt(GameDataValue.GetAttrToValue(roleAttr) * value));

        return exAttr;
    }

    public string AttrType;

    public int Value;

    public List<int> AttrParams;

    public ITEM_QUALITY AttrQuality = ITEM_QUALITY.BLUE;

    public EquipExAttr()
    {
        AttrParams = new List<int>();
        //AttrValues.Add(0);
    }

    public EquipExAttr(string attrType, int value, params int[] attrValues)
    {
        AttrType = attrType;
        Value = value;
        AttrParams = new List<int>(attrValues);
    }

    public EquipExAttr(EquipExAttr copyInstance)
    {
        AttrType = copyInstance.AttrType;
        Value = copyInstance.Value;
        AttrParams = copyInstance.AttrParams;
    }

    public string GetAttrStr(bool eqiupAttr = true)
    {
        var impactType = Type.GetType(AttrType);
        if (impactType == null)
            return "";

        var method = impactType.GetMethod("GetAttrDesc");
        if (method == null)
            return "";

        var attrStr = method.Invoke(null, new object[] { AttrParams }) as string;

        if (eqiupAttr)
        {
            if (!AttrType.Equals("RoleAttrImpactBaseAttr") && !AttrType.Equals("RoleAttrImpactSetAttrByEquip"))
            {
                attrStr += " Lv." + AttrParams[1];
            }
        }

        return attrStr;
    }

    public bool Add(EquipExAttr d)
    {
        if (d.AttrType != AttrType)
            return false;

        if (AttrType == "RoleAttrImpactBaseAttr")
        {
            for (int i = 0; i < AttrParams.Count; ++i)
            {
                AttrParams[i] += d.AttrParams[i];
            }
            return true;
        }

        return false;
    }
}

public class ItemEquip : ItemBase
{
    public ItemEquip(string datID) : base(datID)
    {

    }

    public ItemEquip():base()
    {

    }

    #region equipData

    private static int MAX_INT_CNT = 5;
    public List<int> DynamicDataInt
    {
        get
        {
            if (_DynamicDataInt == null || _DynamicDataInt.Count == 0)
            {
                _DynamicDataInt = new List<int>() { 0, 0, 0, 0 };
            }
            else if (_DynamicDataInt.Count < MAX_INT_CNT)
            {
                for (int i = 0; i < MAX_INT_CNT; ++i)
                {
                    _DynamicDataInt.Add(0);
                }
            }
            return _DynamicDataInt;
        }
    }

    private EquipItemRecord _EquipItemRecord;
    public EquipItemRecord EquipItemRecord
    {
        get
        {
            if (_EquipItemRecord == null)
            {
                if (string.IsNullOrEmpty(_ItemDataID))
                    return null;

                if (_ItemDataID == "-1")
                    return null;

                _EquipItemRecord = TableReader.EquipItem.GetRecord(_ItemDataID);
            }
            return _EquipItemRecord;
        }
    }

    public override CommonItemRecord CommonItemRecord
    {
        get
        {
            if (_CommonItemRecord == null)
            {
                if (string.IsNullOrEmpty(CommonItemDataID.ToString()))
                    return null;

                if (CommonItemDataID < 0)
                    return null;

                _CommonItemRecord = TableReader.CommonItem.GetRecord(CommonItemDataID.ToString());
            }
            return _CommonItemRecord;
        }
    }

    public int EquipLevel
    {
        get
        {
            return DynamicDataInt[0];
        }
        set
        {
            DynamicDataInt[0] = value;
        }
    }

    public ITEM_QUALITY EquipQuality
    {
        get
        {
            return (ITEM_QUALITY)DynamicDataInt[1];
        }
        set
        {
            DynamicDataInt[1] = (int)value;
        }
    }

    public int EquipValue
    {
        get
        {
            return DynamicDataInt[2];
        }
        set
        {
            DynamicDataInt[2] = value;
        }
    }

    public int EquipRefreshCostMatrial
    {
        get
        {
            return DynamicDataInt[3];
        }
        set
        {
            DynamicDataInt[3] = value;
        }
    }

    public int CommonItemDataID
    {
        get
        {
            return DynamicDataInt[4];
        }
        set
        {
            DynamicDataInt[4] = value;
        }
    }

    private int _CombatValue = 0;
    public int CombatValue
    {
        get
        {
            return _CombatValue;
        }
    }

    public void CalculateCombatValue()
    {
        _CombatValue = 0;
        _CombatValue += GameDataValue.GetAttrValue(RoleAttrEnum.Attack, BaseAttack);
        _CombatValue += GameDataValue.GetAttrValue(RoleAttrEnum.HPMax, BaseHP);
        _CombatValue += GameDataValue.GetAttrValue(RoleAttrEnum.Defense, BaseDefence);

        foreach (var exAttrs in EquipExAttr)
        {
            if (exAttrs.AttrType == "RoleAttrImpactBaseAttr")
            {
                _CombatValue += GameDataValue.GetAttrValue((RoleAttrEnum)exAttrs.AttrParams[0], exAttrs.AttrParams[1]);
            }
            else
            {
                _CombatValue += 0;
            }
        }

        if (EquipQuality == ITEM_QUALITY.ORIGIN)
        {
            _CombatValue = Mathf.CeilToInt(_CombatValue * 1.2f);
        }
    }

    private List<EquipExAttr> _EquipExAttr;
    public List<EquipExAttr> EquipExAttr
    {
        get
        {
            if (_EquipExAttr == null)
            {
                _EquipExAttr = new List<global::EquipExAttr>();
                foreach (var strParam in _DynamicDataEx)
                {
                    EquipExAttr exAttr = new global::EquipExAttr();
                    exAttr.AttrType = strParam._StrParams[0];
                    exAttr.Value = int.Parse(strParam._StrParams[1]);
                    for (int i = 2; i < strParam._StrParams.Count; ++i)
                    {
                        exAttr.AttrParams.Add(int.Parse(strParam._StrParams[i]));
                    }
                    _EquipExAttr.Add(exAttr);
                }
            }
            return _EquipExAttr;
        }
        set
        {
            _EquipExAttr = value;
            BakeExAttr();
        }
    }

    public bool IsLegandaryEquip()
    {
        if (ItemDataID.Equals(CommonItemDataID.ToString()))
        {
            return false;
        }
        return true;
    }

    public string GetEquipLegandaryName()
    {
        string equipName = StrDictionary.GetFormatStr(EquipItemRecord.NameStrDict);
        return CommonDefine.GetQualityColorStr(EquipQuality) + equipName + "</color>";
    }

    public string GetEquipNameWithColor()
    {
        string equipName = StrDictionary.GetFormatStr(CommonItemRecord.NameStrDict);
        if (SpSetRecord != null)
        {
            equipName = StrDictionary.GetFormatStr(SpSetRecord.Name) + "-" + equipName;
        }
        return CommonDefine.GetQualityColorStr(EquipQuality) + equipName + "</color>";
    }

    public string GetBaseAttrStr()
    {
        string attrStr = "";
        if (EquipItemRecord.Slot == EQUIP_SLOT.WEAPON)
        {
            string attackValue = BaseAttack.ToString();
            if (_ExBaseAtk)
            {
                attackValue = CommonDefine.GetQualityColorStr(EquipQuality) + attackValue + "</color>";
            }
            attrStr = RandomAttrs.GetAttrName(RoleAttrEnum.Attack) + " + " + attackValue;
        }
        else if (EquipItemRecord.Slot == EQUIP_SLOT.TORSO || EquipItemRecord.Slot == EQUIP_SLOT.LEGS)
        {
            string hpValue = BaseHP.ToString();
            if (_ExBaseHp)
            {
                hpValue = CommonDefine.GetQualityColorStr(EquipQuality) + hpValue + "</color>";
            }
            attrStr = RandomAttrs.GetAttrName(RoleAttrEnum.HPMax) + " + " + hpValue;

            string defenceValue = BaseDefence.ToString();
            if (_ExBaseDef)
            {
                defenceValue = CommonDefine.GetQualityColorStr(EquipQuality) + defenceValue + "</color>";
            }
            attrStr += "\n" + RandomAttrs.GetAttrName(RoleAttrEnum.Defense) + " + " + defenceValue;
        }
        return attrStr;

    }
    #endregion

    #region fun

    public override void RefreshItemData()
    {
        base.RefreshItemData();
        _EquipItemRecord = null;
        _EquipExAttr = null;
        _BaseAttack = -1;
        _BaseHP = -1;
        _BaseDefence = -1;

        if (EquipItemRecord != null)
        {
            CalculateCombatValue();
        }
    }

    public override void ResetItem()
    {
        base.ResetItem();
        _EquipItemRecord = null;
    }

    #endregion

    #region equipExAttr

    public EquipExAttr GetExAttr(int idx)
    {
        if (EquipExAttr.Count > idx)
            return EquipExAttr[idx];
        return null;
    }

    public void AddExAttr(EquipExAttr attr)
    {
        EquipExAttr.Add(attr);
        ItemExData exData = new ItemExData();
        exData._StrParams.Add(attr.AttrType);
        exData._StrParams.Add(attr.Value.ToString());
        for (int i = 0; i < attr.AttrParams.Count; ++i)
        {
            exData._StrParams.Add(attr.AttrParams[i].ToString());
        }
        _DynamicDataEx.Add(exData);
    }

    public void AddExAttr(List<EquipExAttr> attrs)
    {
        foreach (var exAttr in attrs)
        {
            AddExAttr(exAttr);
        }
    }

    public void BakeExAttr()
    {
        _DynamicDataEx.Clear();
        foreach (var exAttr in EquipExAttr)
        {
            ItemExData exData = new ItemExData();
            exData._StrParams.Add(exAttr.AttrType);
            exData._StrParams.Add(exAttr.Value.ToString());
            for (int i = 0; i < exAttr.AttrParams.Count; ++i)
            {
                exData._StrParams.Add(exAttr.AttrParams[i].ToString());
            }
            _DynamicDataEx.Add(exData);
        }
        SaveClass(true);
    }


    #endregion

    #region equipBase

    private int _BaseAttack = -1;
    private bool _ExBaseAtk = false;
    public int BaseAttack
    {
        get
        {
            if (_BaseAttack < 0)
            {
                if (EquipItemRecord.Slot == EQUIP_SLOT.WEAPON)
                {
                    _BaseAttack = GameDataValue.CalWeaponAttack(EquipLevel);
                    if (EquipExAttr.Count > 0 && EquipExAttr[0].AttrType == "RoleAttrImpactBaseAttr" && EquipExAttr[0].AttrParams[0] == (int)RoleAttrEnum.AttackPersent)
                    {
                        _ExBaseAtk = true;
                        _BaseAttack += Mathf.CeilToInt(_BaseAttack * GameDataValue.ConfigIntToFloatDex1(EquipExAttr[0].AttrParams[1]));
                    }
                }
                else
                {
                    _BaseAttack = 0;
                }
                
            }
            return _BaseAttack;
        }
    }

    private int _BaseHP = -1;
    private bool _ExBaseHp = false;
    public int BaseHP
    {
        get
        {
            if (_BaseHP < 0)
            {
                _BaseHP = 0;
                if (EquipItemRecord.Slot == EQUIP_SLOT.TORSO)
                {
                    _BaseHP = GameDataValue.CalEquipTorsoHP(EquipLevel);
                }
                else if(EquipItemRecord.Slot == EQUIP_SLOT.LEGS)
                {
                    _BaseHP = GameDataValue.CalEquipLegsHP(EquipLevel);
                }
                if (EquipExAttr.Count > 0 && EquipExAttr[0].AttrType == "RoleAttrImpactBaseAttr" && EquipExAttr[0].AttrParams[0] == (int)RoleAttrEnum.AttackPersent)
                {
                    _ExBaseHp = true;
                    _BaseHP += Mathf.CeilToInt(_BaseAttack * GameDataValue.ConfigIntToFloatDex1(EquipExAttr[0].AttrParams[1]));
                }
            }
            return _BaseHP;
        }
    }

    private int _BaseDefence = -1;
    private bool _ExBaseDef = false;
    public int BaseDefence
    {
        get
        {
            if (_BaseDefence < 0)
            {
                _BaseDefence = 0;
                if (EquipItemRecord.Slot == EQUIP_SLOT.TORSO)
                {
                    _BaseDefence = GameDataValue.CalEquipTorsoDefence(EquipLevel);
                }
                else if (EquipItemRecord.Slot == EQUIP_SLOT.LEGS)
                {
                    _BaseDefence = GameDataValue.CalEquipLegsDefence(EquipLevel);
                }
                if (EquipExAttr.Count > 0 && EquipExAttr[0].AttrType == "RoleAttrImpactBaseAttr" && EquipExAttr[0].AttrParams[0] == (int)RoleAttrEnum.AttackPersent)
                {
                    _ExBaseDef = true;
                    _BaseDefence += Mathf.CeilToInt(_BaseAttack * GameDataValue.ConfigIntToFloatDex1(EquipExAttr[0].AttrParams[1]));
                }
            }
            return _BaseDefence;
        }
    }

    public void RefreshEquip()
    {
        _BaseAttack = -1;
        _BaseHP = -1;
        _BaseDefence = -1;
    }

    private int _RequireLevel = -1;
    public int RequireLevel
    {
        get
        {
            if (_RequireLevel < 0)
            {
                int exValue = 0;
                _RequireLevel = EquipLevel;
                //foreach (var exAttr in _DynamicDataVector)
                //{
                //    if (exAttr.AttrID == FightAttr.FightAttrType.LEVEL_REQUIRE)
                //    {
                //        exValue += exAttr.AttrValue1;
                //    }
                //}
                _RequireLevel -= exValue;
            }
            return _RequireLevel;
        }
        set
        {
            _RequireLevel = value;
        }
    }

    public void SetEquipAttr(RoleAttrStruct roleAttr)
    {
        if (!IsVolid())
            return;

        roleAttr.AddValue(RoleAttrEnum.Attack, BaseAttack);
        roleAttr.AddValue(RoleAttrEnum.HPMax, BaseHP);
        roleAttr.AddValue(RoleAttrEnum.Defense, BaseDefence);

        foreach (var exAttrs in EquipExAttr)
        {
            if (exAttrs.AttrType == "RoleAttrImpactBaseAttr")
            {
                roleAttr.AddValue((RoleAttrEnum)exAttrs.AttrParams[0], exAttrs.AttrParams[1]);
            }
            else
            {
                roleAttr.AddExAttr(RoleAttrImpactManager.GetAttrImpact(exAttrs));
            }
        }
    }
    #endregion

    #region create equip

    public static ItemEquip CreateEquipByMonster(int level, Tables.ITEM_QUALITY quality, int value)
    {
        return null;
    }

    public static ItemEquip CreateBaseWeapon(PROFESSION profession)
    {
        int weapon = 1;
        if (profession == PROFESSION.GIRL_DEFENCE || profession == PROFESSION.GIRL_DOUGE)
        {
            weapon = 1001;
        }
        ItemEquip itemEquip = new ItemEquip(weapon.ToString());
        itemEquip.EquipLevel = 1;
        itemEquip.EquipQuality = ITEM_QUALITY.WHITE;
        itemEquip.EquipValue = 10;
        itemEquip.RequireLevel = 1;
        itemEquip.CommonItemDataID = weapon;

        itemEquip.CalculateCombatValue();

        return itemEquip;
    }

    public static ItemEquip CreateEquip(int level, Tables.ITEM_QUALITY quality, int legencyEquipID = -1, int equipSlotIdx = -1, int prePro = -1)
    {
        Tables.ITEM_QUALITY equipQuality = quality;
        EquipItemRecord legencyEquip = null;
        if (legencyEquipID > 0)
        {
            legencyEquip = TableReader.EquipItem.GetRecord(legencyEquipID.ToString());
        }
        if (legencyEquip != null)
        {
            equipQuality = ITEM_QUALITY.ORIGIN;
        }

        EquipItemRecord baseEquip = null;
        CommonItemRecord commonItemRecord = null;

        EQUIP_SLOT equipSlot = EQUIP_SLOT.AMULET;
        int profession = prePro;
        if (equipSlotIdx < 0)
        {
            equipSlot = GameDataValue.GetRandomItemSlot(quality);
        }
        else
        {
            equipSlot = (EQUIP_SLOT)equipSlotIdx;
        }
        if (legencyEquip != null)
        {
            profession = legencyEquip.ProfessionLimit;
        }

        if(profession <= 0)
        {
            if (equipSlot == EQUIP_SLOT.WEAPON)
            {
                int randomVal = UnityEngine.Random.Range(0, 2);
                if (randomVal == 0)
                {
                    profession = 5;
                }
                else
                {
                    profession = 10;
                }
            }
        }

        int value = GameDataValue.CalLvValue(level, equipSlot);
        baseEquip = GetRandomItem(equipSlot, level, profession);
        if (baseEquip == null)
            return null;

        commonItemRecord = TableReader.CommonItem.GetRecord(baseEquip.Id);
        if (legencyEquip != null)
        {
            baseEquip = legencyEquip;
        }

        ItemEquip itemEquip = new ItemEquip(baseEquip.Id);
        itemEquip.EquipLevel = level;
        itemEquip.EquipQuality = equipQuality;
        itemEquip.EquipValue = value;
        itemEquip.RequireLevel = level;
        itemEquip.CommonItemDataID = int.Parse(commonItemRecord.Id);

        //RandomEquipAttr(itemEquip);
        itemEquip.AddExAttr(RandomAttrs.GetRandomEquipExAttrs(baseEquip.Slot, level, value, equipQuality, RoleData.SelectRole.Profession));
        if (legencyEquip != null)
        {
            EquipExAttr legencyAttr = legencyEquip.ExAttr.GetExAttr(GameDataValue.GetLegencyLv( itemEquip.EquipLevel));
            itemEquip.AddExAttr(legencyAttr);
        }

        itemEquip.CalculateCombatValue();

        return itemEquip;
    }

    public static ItemEquip GetBaseEquip(string id, int level, ITEM_QUALITY quality, int value, int requireLv)
    {
        ItemEquip itemEquip = new ItemEquip(id);
        itemEquip.EquipLevel = level;
        itemEquip.EquipQuality = quality;
        itemEquip.EquipValue = value;
        itemEquip.RequireLevel = requireLv;

        return itemEquip;
    }


    private static EquipItemRecord GetRandomItem(EQUIP_SLOT equipSlot, int level, int profession)
    {
        List<EquipItemRecord> randomItems = null;
        if (equipSlot == EQUIP_SLOT.WEAPON)
        {
            if (profession == 5)
            {
                randomItems = TableReader.EquipItem.AxeWeapons;
            }
            else
            {
                randomItems = TableReader.EquipItem.SwordWeapons;
            }
        }
        else
        {
            randomItems = TableReader.EquipItem.ClassedEquips[equipSlot];
        }

        List<int> randomVals = new List<int>();
        if (level < 30)
        {
            for (int i = 0; i < 4; ++i)
            {
                randomVals.Add(10);
            }
        }
        else if (level < 60)
        {
            for (int i = 0; i < 4; ++i)
            {
                randomVals.Add(10);
            }
            for (int i = 0; i < 4; ++i)
            {
                randomVals.Add(20);
            }
        }
        else
        {
            for (int i = 0; i < 8; ++i)
            {
                randomVals.Add(10);
            }
            for (int i = 8; i < randomItems.Count; ++i)
            {
                randomVals.Add(15);
            }
        }

        int randomIdx = GameRandom.GetRandomLevel(randomVals.ToArray());

        return randomItems[randomIdx];
    }

    

    #endregion

    #region equip sp attr

    public static string _DefauletSpSetID = "1";
    public static int _ActSetLeastExCnt = 2;
    public static float _ActSetValPersent = 0.9f;

    private EquipSpAttrRecord _SpSetRecord;
    public EquipSpAttrRecord SpSetRecord
    {
        get
        {
            return _SpSetRecord;
        }
    }

    public void CalculateSet()
    {
        if (_SpSetRecord != null)
        {
            EquipSet.Instance.RemoveActingSpAttr(_SpSetRecord, EquipValue);
        }
        _SpSetRecord = null;
        if (EquipExAttr.Count < _ActSetLeastExCnt)
        {
            return;
        }

        int valAttrCnt = 0;
        List<EquipExAttr> randomAttrs = new List<global::EquipExAttr>();
        foreach (var exAttr in EquipExAttr)
        {
            if (exAttr.AttrType != "RoleAttrImpactBaseAttr")
            {
                exAttr.AttrQuality = ITEM_QUALITY.ORIGIN;
                continue;
            }

            var valuePersent = GameDataValue.GetExAttrPersent(this, exAttr);
            if (valuePersent > _ActSetValPersent)
            {
                exAttr.AttrQuality = ITEM_QUALITY.ORIGIN;
                ++valAttrCnt;
            }
            else
            {
                exAttr.AttrQuality = ITEM_QUALITY.BLUE;
            }

            if ((exAttr.AttrParams[0] != (int)RoleAttrEnum.AttackPersent
                && exAttr.AttrParams[0] != (int)RoleAttrEnum.HPMaxPersent
                && exAttr.AttrParams[0] != (int)RoleAttrEnum.MoveSpeed))
            {
                randomAttrs.Add(exAttr);
            }
        }
        if (valAttrCnt < _ActSetLeastExCnt)
        {
            return;
        }


        foreach (var setRecord in TableReader.EquipSpAttr.Records.Values)
        {
            if (setRecord.Id == _DefauletSpSetID)
                continue;

            _SpSetRecord = setRecord;
            foreach (var exAttr in randomAttrs)
            {
                bool isAttrOk = false;
                for (int i = 0; i < setRecord.ExAttrEnum.Count; ++i)
                {
                    if (exAttr.AttrParams[0] == setRecord.ExAttrEnum[i])
                    {
                        isAttrOk = true;
                        break;
                    }
                }
                if (!isAttrOk)
                {
                    _SpSetRecord = null;
                    break;
                }
            }
            if (_SpSetRecord != null)
            {
                break;
            }
        }

        if (_SpSetRecord == null)
        {
            _SpSetRecord = TableReader.EquipSpAttr.GetRecord(_DefauletSpSetID);
        }
        EquipSet.Instance.ActingSpAttr(_SpSetRecord, EquipValue);
    }

    public static bool IsAttrSpToEquip(EquipExAttr exAttr)
    {
        if (exAttr.AttrType != "RoleAttrImpactBaseAttr")
            return false;

        if (((RoleAttrEnum)exAttr.AttrParams[0] == RoleAttrEnum.AttackPersent
                        || (RoleAttrEnum)exAttr.AttrParams[0] == RoleAttrEnum.DefensePersent
                        || (RoleAttrEnum)exAttr.AttrParams[0] == RoleAttrEnum.HPMaxPersent
                        || (RoleAttrEnum)exAttr.AttrParams[0] == RoleAttrEnum.MoveSpeed))
        {
            return true;
        }
        return false;
    }

    public static bool IsAttrBaseAttr(EquipExAttr exAttr)
    {
        if (exAttr.AttrType != "RoleAttrImpactBaseAttr")
            return false;

        return true;
    }
    #endregion
}

