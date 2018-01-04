using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Tables;
using System;

public class EquipExAttr
{

    public string AttrType;

    public int Value;

    public List<int> AttrParams;


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

    public string GetAttrStr()
    {
        var impactType = Type.GetType(AttrType);
        Debug.Log("AttrType:" + AttrType);
        if (impactType == null)
            return "";

        var method = impactType.GetMethod("GetAttrDesc");
        if (method == null)
            return "";

        return method.Invoke(null, new object[] { AttrParams }) as string;
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

    public List<int> DynamicDataInt
    {
        get
        {
            if (_DynamicDataInt == null || _DynamicDataInt.Count == 0)
            {
                _DynamicDataInt = new List<int>() { 0, 0, 0 };
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
    }

    public string GetEquipNameWithColor()
    {
        return CommonDefine.GetQualityColorStr(EquipQuality) + EquipItemRecord.CommonItem.Name + "</color>";
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
            attrStr = StrDictionary.GetFormatStr((int)RoleAttrEnum.Attack, attackValue);
        }
        else if (EquipItemRecord.Slot == EQUIP_SLOT.TORSO || EquipItemRecord.Slot == EQUIP_SLOT.LEGS)
        {
            string hpValue = BaseHP.ToString();
            if (_ExBaseHp)
            {
                hpValue = CommonDefine.GetQualityColorStr(EquipQuality) + hpValue + "</color>";
            }
            attrStr = StrDictionary.GetFormatStr((int)RoleAttrEnum.Defense, hpValue);

            string defenceValue = BaseDefence.ToString();
            if (_ExBaseDef)
            {
                defenceValue = CommonDefine.GetQualityColorStr(EquipQuality) + defenceValue + "</color>";
            }
            attrStr += "\n" + StrDictionary.GetFormatStr((int)RoleAttrEnum.HPMax, defenceValue);
        }
        return attrStr;

    }
    #endregion

    #region fun

    public override void RefreshItemData()
    {
        base.RefreshItemData();
        _EquipItemRecord = null;
    }

    public override void ResetItem()
    {
        base.ResetItem();
        _EquipItemRecord = null;
    }

    #endregion

    #region equipAttr

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

    private int _RequireLevel = -1;
    public int RequireLevel
    {
        get
        {
            //if (_RequireLevel < 0)
            //{
            //    int exValue = 0;
            //    _RequireLevel = EquipItemRecord.LevelLimit;
            //    //foreach (var exAttr in _DynamicDataVector)
            //    //{
            //    //    if (exAttr.AttrID == FightAttr.FightAttrType.LEVEL_REQUIRE)
            //    //    {
            //    //        exValue += exAttr.AttrValue1;
            //    //    }
            //    //}
            //    _RequireLevel -= exValue;
            //}
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

    #region equip reset

    private CommonItemRecord _ResetCostItem = null;
    public CommonItemRecord ResetCostItem
    {
        get
        {
            if (_ResetCostItem == null)
            {
                InitEquipResetCostItemData();
            }
            return _ResetCostItem;
        }
    }

    private int _ResetCostItemNum = -1;
    public int ResetCostItemNum
    {
        get
        {
            if (_ResetCostItemNum < 0)
            {
                InitEquipResetCostItemData();
            }
            return _ResetCostItemNum;
        }
    }

    private int _ResetCostMoney = -1;
    public int ResetCostMoney
    {
        get
        {
            if (_ResetCostMoney < 0)
            {
                InitEquipResetCostItemData();
            }
            return _ResetCostMoney;
        }
    }

    private void InitEquipResetCostItemData()
    {
        if (EquipItemRecord.Slot == EQUIP_SLOT.WEAPON)
        {
            if (EquipLevel < 40)
            {
                _ResetCostItem = TableReader.CommonItem.GetRecord("20000");
                _ResetCostItemNum = 4;
                _ResetCostMoney = 400;
            }
            else if (EquipLevel < 70)
            {
                _ResetCostItem = TableReader.CommonItem.GetRecord("20001");
                _ResetCostItemNum = 7;
                _ResetCostMoney = 700;
            }
            else if (EquipLevel < 90)
            {
                _ResetCostItem = TableReader.CommonItem.GetRecord("20002");
                _ResetCostItemNum = 9;
                _ResetCostMoney = 900;
            }
            else if (EquipLevel < 100)
            {
                _ResetCostItem = TableReader.CommonItem.GetRecord("20003");
                _ResetCostItemNum = 10;
                _ResetCostMoney = 1000;
            }
            else
            {
                _ResetCostItem = TableReader.CommonItem.GetRecord("20004");
                _ResetCostItemNum = 10;
                _ResetCostMoney = 1000;
            }
        }
        else if (EquipItemRecord.Slot == EQUIP_SLOT.TORSO
            || EquipItemRecord.Slot == EQUIP_SLOT.LEGS)
        {
            if (EquipLevel < 40)
            {
                _ResetCostItem = TableReader.CommonItem.GetRecord("20005");
                _ResetCostItemNum = 4;
                _ResetCostMoney = 400;
            }
            else if (EquipLevel < 70)
            {
                _ResetCostItem = TableReader.CommonItem.GetRecord("20006");
                _ResetCostItemNum = 7;
                _ResetCostMoney = 700;
            }
            else if (EquipLevel < 90)
            {
                _ResetCostItem = TableReader.CommonItem.GetRecord("20007");
                _ResetCostItemNum = 9;
                _ResetCostMoney = 900;
            }
            else if (EquipLevel < 100)
            {
                _ResetCostItem = TableReader.CommonItem.GetRecord("20008");
                _ResetCostItemNum = 10;
                _ResetCostMoney = 1000;
            }
            else
            {
                _ResetCostItem = TableReader.CommonItem.GetRecord("20009");
                _ResetCostItemNum = 10;
                _ResetCostMoney = 1000;
            }
        }
        else if (EquipItemRecord.Slot == EQUIP_SLOT.AMULET
            || EquipItemRecord.Slot == EQUIP_SLOT.RING)
        {
            if (EquipLevel < 40)
            {
                _ResetCostItem = TableReader.CommonItem.GetRecord("20010");
                _ResetCostItemNum = 4;
                _ResetCostMoney = 400;
            }
            else if (EquipLevel < 70)
            {
                _ResetCostItem = TableReader.CommonItem.GetRecord("20011");
                _ResetCostItemNum = 7;
                _ResetCostMoney = 700;
            }
            else if (EquipLevel < 90)
            {
                _ResetCostItem = TableReader.CommonItem.GetRecord("20012");
                _ResetCostItemNum = 9;
                _ResetCostMoney = 900;
            }
            else if (EquipLevel < 100)
            {
                _ResetCostItem = TableReader.CommonItem.GetRecord("20013");
                _ResetCostItemNum = 10;
                _ResetCostMoney = 1000;
            }
            else
            {
                _ResetCostItem = TableReader.CommonItem.GetRecord("20014");
                _ResetCostItemNum = 10;
                _ResetCostMoney = 1000;
            }
        }
    }

    public void ResetEquipAttr()
    {
        _DynamicDataEx.Clear();
        RandomEquipAttr(this);
    }

    #endregion

    #region create equip

    public static ItemEquip CreateEquipByMonster(int level, Tables.ITEM_QUALITY quality, int value)
    {
        return null;
    }

    public static ItemEquip CreateEquip(int level, Tables.ITEM_QUALITY quality, int value, int legencyEquipID = -1, int equipSlotIdx = -1)
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
        if (legencyEquip != null)
        {
            baseEquip = legencyEquip;
        }
        else
        {

            if (equipSlotIdx < 0)
            {
                var equipSlot = GetRandomItemSlot();
                baseEquip = GetRandomItem(equipSlot, level);
            }
            else
            {
                EQUIP_SLOT equipSlot = (EQUIP_SLOT)equipSlotIdx;
                baseEquip = GetRandomItem(equipSlot, level);
            }
            if (baseEquip == null)
                return null;
        }

        ItemEquip itemEquip = new ItemEquip(baseEquip.Id);
        itemEquip.EquipLevel = level;
        itemEquip.EquipQuality = equipQuality;
        itemEquip.EquipValue = value;
        itemEquip.RequireLevel = level;

        //RandomEquipAttr(itemEquip);
        itemEquip.AddExAttr(RandomAttrs.GetRandomEquipExAttrs(baseEquip.Slot, level, value, equipQuality, RoleData.SelectRole.Profession));
        if (legencyEquip != null)
        {
            EquipExAttr legencyAttr = legencyEquip.ExAttr.GetExAttr(itemEquip.EquipLevel);
            itemEquip.AddExAttr(legencyAttr);
        }

        return itemEquip;
    }

    public static void RandomEquipAttr(ItemEquip itemEquip)
    {
        int attrCnt = GetRandomAttrCnt(itemEquip.EquipQuality);

        List<FightAttrRecord> canGetAttrs = new List<FightAttrRecord>();
        foreach (var record in TableReader.FightAttr.Records.Values)
        {
            if (record.LevelMin > 0 && record.LevelMin > itemEquip.EquipLevel)
                continue;

            if (record.LevelMax > 0 && record.LevelMax < itemEquip.EquipLevel)
                continue;

            if (record.SlotLimit >= 0)
            {
                if (((record.SlotLimit >> (int)itemEquip.EquipItemRecord.Slot) & 1) == 0)
                {
                    continue;
                }
            }

            if (record.ProfessionLimit >= 0)
            {
                if (record.ProfessionLimit != (int)itemEquip.EquipItemRecord.ProfessionLimit)
                    continue;
            }

            canGetAttrs.Add(record);
        }

        for (int i = 0; i < attrCnt; ++i)
        {
            int randomIdx = UnityEngine.Random.Range(0, canGetAttrs.Count);
            itemEquip.AddExAttr(GetRandomAttr(canGetAttrs[randomIdx], itemEquip.EquipValue));
            RemoveRandomAttr(canGetAttrs, canGetAttrs[randomIdx]);
        }
    }

    private static EquipExAttr GetRandomAttr(FightAttrRecord attrRecord, int value)
    {
        EquipExAttr attrItem = new EquipExAttr();
        //attrItem.AttrID = (RoleAttrEnum)attrRecord.AttrID;

        //Vector3 attrValue = new Vector3();
        //for (int i = attrRecord.Values.Count - 1; i >= 0; --i)
        //{
        //    if (attrRecord.Values[i].z > 0 && value > attrRecord.Values[i].z)
        //    {
        //        attrValue = attrRecord.Values[i];
        //        break;
        //    }
        //}

        //attrItem.AttrValues[0] = UnityEngine.Random.Range((int)attrValue.x, (int)attrValue.y + 1);
        return attrItem;
    }

    private static List<FightAttrRecord> RemoveRandomAttr(List<FightAttrRecord> attrList, FightAttrRecord attrRecord)
    {
        attrList.Remove(attrRecord);
        if (attrRecord.Conflict <= 0)
            return attrList;

        List<FightAttrRecord> conflicts = new List<FightAttrRecord>();
        for (int i = 0; i < attrList.Count; ++i)
        {
            if (attrList[i].Conflict == attrRecord.Conflict)
            {
                conflicts.Add(attrList[i]);
            }
        }

        foreach (var conflictRecord in conflicts)
        {
            attrList.Remove(conflictRecord);
        }

        return attrList;
    }

    private static EquipItemRecord GetRandomItem(EQUIP_SLOT equipSlot, int level)
    {
        Dictionary<int, EquipItemRecord> professionEquips = new Dictionary<int, EquipItemRecord>();
        foreach (var equipRecord in TableReader.EquipItem.ClassedEquips[equipSlot])
        {
            if (!professionEquips.ContainsKey(equipRecord.ProfessionLimit))
            {
                professionEquips.Add(equipRecord.ProfessionLimit, equipRecord);
                continue;
            }

            if (equipRecord.LevelLimit <= level
                && equipRecord.LevelLimit > professionEquips[equipRecord.ProfessionLimit].LevelLimit)
            {
                professionEquips[equipRecord.ProfessionLimit] = equipRecord;
            }
        }

        float singleRate = 0;
        if (professionEquips.Count == 1)
        {
            singleRate = 1;

        }
        else
        {
            if (professionEquips.ContainsKey((int)PROFESSION.NONE))
            {
                singleRate = (1 - 0.5f) / (professionEquips.Count - 1);
            }
            else
            {
                singleRate = 1.0f / (professionEquips.Count);
            }
        }

        float randomRate = UnityEngine.Random.Range(0, 1.0f);
        float rateTotal = 0;
        foreach (var equipRecord in professionEquips.Values)
        {
            //if (equipRecord.ProfessionLimit == (int)PROFESSION.NONE)
            //{
            //    rateTotal += 0.5f;
            //}
            //else
            {
                rateTotal += singleRate;
            }

            if (rateTotal >= randomRate)
                return equipRecord;
        }
        return null;
    }

    public static EQUIP_SLOT GetRandomItemSlot()
    {
        int slotTypeCnt = Enum.GetValues(typeof(EQUIP_SLOT)).Length;
        int randomSlot = UnityEngine.Random.Range(0, slotTypeCnt);
        return (EQUIP_SLOT)randomSlot;
    }

    private static int GetRandomAttrCnt(Tables.ITEM_QUALITY quality)
    {
        switch (quality)
        {
            case Tables.ITEM_QUALITY.WHITE:
                return 0;
            case Tables.ITEM_QUALITY.BLUE:
                return UnityEngine.Random.Range(1, 2);
            case Tables.ITEM_QUALITY.PURPER:
                return UnityEngine.Random.Range(3, 4);
            case Tables.ITEM_QUALITY.ORIGIN:
                return 5;
            default:
                return 0;
        }
    }

    #endregion
}

