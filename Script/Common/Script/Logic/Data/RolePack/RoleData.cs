using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tables;
using System;

public class RoleData : SaveItemBase
{
    //default info
    public string MainBaseName;
    public string MotionFold;
    public string ModelName;
    public PROFESSION Profession;
    public string DefaultWeaponModel;

    public static RoleData SelectRole
    {
        get
        {
            return PlayerDataPack.Instance._SelectedRole;
        }
    }

    public void InitRoleData()
    {
        bool needSave = false;
        needSave |= InitLevel();
        needSave |= InitEquipList();

        if (needSave)
        {
            SaveClass(true);
        }
    }

    #region equipManager

    [SaveField(1)]
    public List<ItemEquip> _EquipList;

    private bool InitEquipList()
    {
        int equipSlotCnt = Enum.GetValues(typeof(EQUIP_SLOT)).Length;
        if (_EquipList == null || _EquipList.Count != equipSlotCnt)
        {
            if (_EquipList == null)
            {
                _EquipList = new List<ItemEquip>();
            }

            int startIdx = _EquipList.Count;
            for (int i = startIdx; i < equipSlotCnt; ++i)
            {
                ItemEquip newItemEquip = new ItemEquip("-1");
                //newItemEquip._SaveFileName = _SaveFileName + ".Equip" + i;
                _EquipList.Add(newItemEquip);
            }

            var equipItem = ItemEquip.CreateBaseWeapon(Profession);
            PutOnEquip(EQUIP_SLOT.WEAPON, equipItem);

            return true;
        }
        foreach (var itemEquip in _EquipList)
        {
            if (itemEquip.IsVolid())
            {
                itemEquip.CalculateSet();
                itemEquip.CalculateCombatValue();
            }
        }
        return false;
    }

    public ItemEquip GetEquipItem(EQUIP_SLOT equipSlot)
    {
        return _EquipList[(int)equipSlot];
    }

    public bool IsCanEquipItem(EQUIP_SLOT equipSlot, ItemEquip equipItem)
    {
        if (equipItem == null)
            return false;

        if (equipItem.EquipItemRecord == null)
            return false;

        if (equipItem.EquipItemRecord.Slot != equipSlot)
            return false;

        if (equipItem.RequireLevel > _RoleLevel)
        {
            return false;
        }

        if (equipItem.EquipItemRecord.ProfessionLimit > 0 &&
            ((equipItem.EquipItemRecord.ProfessionLimit >> (int)Profession) & 1) == 0)
        {
            return false;
        }

        return true;
    }

    public void PutOnEquip(EQUIP_SLOT equipSlot, ItemEquip equipItem)
    {
        if (!IsCanEquipItem(equipSlot, equipItem))
            return;

        _EquipList[(int)equipSlot].ExchangeInfo(equipItem);

        UIEquipPack.RefreshBagItems();

        CalculateAttr();
    }

    public void PutOffEquip(EQUIP_SLOT equipSlot, ItemEquip equipItem)
    {
        var backPackPos = BackBagPack.Instance.GetEmptyPageEquip();
        backPackPos.ExchangeInfo(equipItem);

        UIEquipPack.RefreshBagItems();

        CalculateAttr();
    }



    public string GetWeaponModelName()
    {
        var equip = GetEquipItem(EQUIP_SLOT.WEAPON);
        if (equip != null && equip.EquipItemRecord != null)
        {
            return equip.EquipItemRecord.Model;
        }
        else
        {
            return DefaultWeaponModel;
        }
    }

    #endregion

    #region role attr

    public float GetBaseMoveSpeed()
    {
        return 4.5f;
    }

    public int GetBaseAttackSpeed()
    {
        return 1;
    }

    public int GetBaseAttack()
    {
        return 10;
    }

    public int GetBaseHP()
    {
        return _RoleLevel * 10 + 100;
    }

    public int GetBaseDefence()
    {
        return _RoleLevel * 5 + 5;
    }

    //baseAttrs
    public RoleAttrStruct _BaseAttr = new RoleAttrStruct();

    public void CalculateAttr()
    {
        _BaseAttr.ResetBaseAttr();
        SetRoleLevelAttr(_BaseAttr);
        SkillData.Instance.SetSkillAttr(_BaseAttr);
        SetEquipAttr(_BaseAttr);
        GemData.Instance.SetGemAttr(_BaseAttr);
        LegendaryData.Instance.SetAttr(_BaseAttr);

        CalculateSecondAttr(_BaseAttr);
    }

    public void SetRoleLevelAttr(RoleAttrStruct roleAttr)
    {
        roleAttr.SetValue(RoleAttrEnum.Strength, Strength);
        roleAttr.SetValue(RoleAttrEnum.Dexterity, Dexterity);
        roleAttr.SetValue(RoleAttrEnum.Vitality, Vitality);
        roleAttr.SetValue(RoleAttrEnum.Intelligence, Intelligence);
        roleAttr.SetValue(RoleAttrEnum.Attack, _RoleLevel * GameDataValue._AtkPerRoleLevel + GameDataValue._AtkRoleLevelBase);
        roleAttr.SetValue(RoleAttrEnum.HPMax, _RoleLevel * GameDataValue._HPPerRoleLevel + GameDataValue._HPRoleLevelBase);
    }

    public void SetEquipAttr(RoleAttrStruct roleAttr)
    {
        foreach (var equipInfo in _EquipList)
        {
            equipInfo.SetEquipAttr(roleAttr);
        }
    }

    public void SetGemAttr(RoleAttrStruct roleAttr)
    {
        GemData.Instance.SetGemAttr(roleAttr);
    }

    public void CalculateSecondAttr(RoleAttrStruct roleAttr)
    {
        var strength = roleAttr.GetValue(RoleAttrEnum.Strength);
        var baseAttack = roleAttr.GetValue(RoleAttrEnum.Attack);
        float attackByStrength = strength * GameDataValue._AttackPerStrength;
        float PhyEnhanceByStrength = strength * GameDataValue._DmgEnhancePerStrength;
        roleAttr.AddValue(RoleAttrEnum.Attack, (int)attackByStrength);
        roleAttr.AddValue(RoleAttrEnum.PhysicDamageEnhance, (int)PhyEnhanceByStrength);

        var dexteriry = roleAttr.GetValue(RoleAttrEnum.Dexterity);
        int criticalRate = (int)(dexteriry * GameDataValue._CriticalRatePerDex);
        int criticalDamage = (int)(dexteriry * GameDataValue._CriticalDmgPerDex);
        int ignoreAttack = (int)(dexteriry * GameDataValue._IgnoreAtkPerDex);
        roleAttr.AddValue(RoleAttrEnum.CriticalHitChance, criticalRate);
        roleAttr.AddValue(RoleAttrEnum.CriticalHitDamge, criticalDamage);
        roleAttr.AddValue(RoleAttrEnum.IgnoreDefenceAttack, ignoreAttack);

        var intelligence = roleAttr.GetValue(RoleAttrEnum.Intelligence);
        int eleAtk = (int)(intelligence * GameDataValue._EleAtkPerInt);
        int eleEnhance = (int)(intelligence * GameDataValue._EleEnhancePerInt);

        roleAttr.AddValue(RoleAttrEnum.FireAttackAdd, eleAtk);
        roleAttr.AddValue(RoleAttrEnum.FireEnhance, eleEnhance);
        roleAttr.AddValue(RoleAttrEnum.ColdAttackAdd, eleAtk);
        roleAttr.AddValue(RoleAttrEnum.ColdEnhance, eleEnhance);
        roleAttr.AddValue(RoleAttrEnum.LightingAttackAdd, eleAtk);
        roleAttr.AddValue(RoleAttrEnum.LightingEnhance, eleEnhance);
        roleAttr.AddValue(RoleAttrEnum.WindAttackAdd, eleAtk);
        roleAttr.AddValue(RoleAttrEnum.WindEnhance, eleEnhance);

        var vitality = roleAttr.GetValue(RoleAttrEnum.Vitality);
        int hpByVitality = (int)(vitality * GameDataValue._HPPerVit);
        int finalDamageReduse = (int)(vitality * GameDataValue._FinalDmgRedusePerVit);
        roleAttr.AddValue(RoleAttrEnum.HPMax, hpByVitality);
        roleAttr.AddValue(RoleAttrEnum.FinalDamageReduse, finalDamageReduse);

        var allEleAtk = roleAttr.GetValue(RoleAttrEnum.AllEleAtk);
        if (allEleAtk > 0)
        {
            roleAttr.AddValue(RoleAttrEnum.FireAttackAdd, allEleAtk);
            roleAttr.AddValue(RoleAttrEnum.ColdAttackAdd, allEleAtk);
            roleAttr.AddValue(RoleAttrEnum.LightingAttackAdd, allEleAtk);
            roleAttr.AddValue(RoleAttrEnum.WindAttackAdd, allEleAtk);
        }
        var allEleResist = roleAttr.GetValue(RoleAttrEnum.AllResistan);
        if (allEleResist > 0)
        {
            roleAttr.AddValue(RoleAttrEnum.FireResistan, allEleResist);
            roleAttr.AddValue(RoleAttrEnum.ColdResistan, allEleResist);
            roleAttr.AddValue(RoleAttrEnum.LightingResistan, allEleResist);
            roleAttr.AddValue(RoleAttrEnum.WindResistan, allEleResist);
        }
        var allEleEnhance = roleAttr.GetValue(RoleAttrEnum.AllEnhance);
        if (allEleEnhance > 0)
        {
            roleAttr.AddValue(RoleAttrEnum.FireEnhance, allEleEnhance);
            roleAttr.AddValue(RoleAttrEnum.ColdEnhance, allEleEnhance);
            roleAttr.AddValue(RoleAttrEnum.LightingEnhance, allEleEnhance);
            roleAttr.AddValue(RoleAttrEnum.WindEnhance, allEleEnhance);
        }
    }

    #endregion

    #region attr Points

    public static int MAX_ROLE_LEVEL = 50;
    public static int POINT_PER_ROLE_LEVEL = 4;
    public static int POINT_PER_ATTR_LEVEL = 1;

    [SaveField(2)]
    public int _RoleLevel;

    [SaveField(3)]
    public int _AttrLevel;

    [SaveField(4)]
    public int _CurExp;

    [SaveField(5)]
    private int _AddStrength = 0;
    public int Strength
    {
        get
        {
            return _RoleLevel * 1 + _AddStrength;
        }
    }

    [SaveField(6)]
    private int _AddDexterity = 0;
    public int Dexterity
    {
        get
        {
            return _RoleLevel * 1 + _AddDexterity;
        }
    }

    [SaveField(7)]
    private int _AddVitality = 0;
    public int Vitality
    {
        get
        {
            return _RoleLevel * 1 + _AddVitality;
        }
    }

    [SaveField(8)]
    private int _AddIntelligence = 0;
    public int Intelligence
    {
        get
        {
            return _RoleLevel * 1 + _AddIntelligence;
        }
    }

    [SaveField(9)]
    private int _UnDistrubutePoint = 0;
    public int UnDistrubutePoint
    {
        get
        {
            return _UnDistrubutePoint;
        }
    }

    private static int MAX_LEVEL = 100;
    private int _LvUpExp = 0;
    public int LvUpExp
    {
        get
        {
            return _LvUpExp;
        }
    }

    public bool InitLevel()
    {
        if (_RoleLevel <= 0)
        {
            _RoleLevel = 1;
        }
        _LvUpExp = GameDataValue.GetLvUpExp(_RoleLevel, _AttrLevel);
        return false;
    }

    public void AddExp(int value)
    {
        if (_RoleLevel < MAX_LEVEL)
        {
            _CurExp += value;
            if (_CurExp >= _LvUpExp)
            {
                _CurExp -= _LvUpExp;
                RoleLevelUp();
            }
        }
        else
        {
            _CurExp += value;
            if (_CurExp >= _LvUpExp)
            {
                _CurExp -= _LvUpExp;
                AttrLevelUp();
            }
        }

        if (_CurExp > _LvUpExp)
        {
            AddExp(0);
        }

        SaveClass(false);
    }

    private void RoleLevelUp()
    {
        ++_RoleLevel;
        _UnDistrubutePoint += POINT_PER_ROLE_LEVEL;

        CalculateAttr();

        _LvUpExp = GameDataValue.GetLvUpExp(_RoleLevel, _AttrLevel);
        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_ROLE_LEVEL_UP, this, null);
    }

    private void AttrLevelUp()
    {
        ++_AttrLevel;
        _UnDistrubutePoint += POINT_PER_ATTR_LEVEL;

        _LvUpExp = GameDataValue.GetLvUpExp(_RoleLevel, _AttrLevel);
        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_ROLE_LEVEL_UP, this, null);
    }

    public void ResetPoints()
    {
        _AddStrength = 0;
        _AddDexterity = 0;
        _AddVitality = 0;
        _AddIntelligence = 0;
        _UnDistrubutePoint = _RoleLevel * POINT_PER_ROLE_LEVEL + _AttrLevel * POINT_PER_ATTR_LEVEL;

        CalculateAttr();
    }

    public void DistributePoint(int distriAttr, int point)
    {
        _UnDistrubutePoint -= point;
        switch (distriAttr)
        {
            case 1:
                _AddStrength += point;
                break;
            case 2:
                _AddDexterity += point;
                break;
            case 3:
                _AddIntelligence += point;
                break;
            case 4:
                _AddVitality += point;
                break;
        }

        CalculateAttr();
    }

    #endregion

    
}
