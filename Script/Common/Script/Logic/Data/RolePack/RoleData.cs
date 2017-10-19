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
        if (_RoleLevel < 0)
        {
            _RoleLevel = 0;
        }

        InitEquipList();
        InitSkill();
    }

    #region equipManager

    [SaveField(1)]
    public List<ItemEquip> _EquipList;

    private void InitEquipList()
    {
        if (_EquipList == null || _EquipList.Count == 0)
        {
            int equipSlotCnt = Enum.GetValues(typeof(EQUIP_SLOT)).Length;
            _EquipList = new List<ItemEquip>();
            for (int i = 0; i < equipSlotCnt; ++i)
            {
                ItemEquip newItemEquip = new ItemEquip();
                newItemEquip.ItemDataID = "-1";
                newItemEquip._SaveFileName = _SaveFileName + ".Equip" + i;
                _EquipList.Add(newItemEquip);
            }
        }
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

        if (equipItem.EquipItemRecord.LevelLimit > _RoleLevel)
            return false;

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
        
        UIBagPack.RefreshBagItems();

        CalculateAttr();
    }

    public void PutOffEquip(EQUIP_SLOT equipSlot, ItemEquip equipItem)
    {
        var backPackPos = BackBagPack.Instance.GetEmptyPageEquip();
        backPackPos.ExchangeInfo(equipItem);

        UIBagPack.RefreshBagItems();

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
        return _RoleLevel * 10 + 10;
    }

    public int GetBaseHP()
    {
        return _RoleLevel * 100 + 100;
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
        SetSkillAttr(_BaseAttr);
        SetEquipAttr(_BaseAttr);

        CalculateSecondAttr();
    }

    public void SetRoleLevelAttr(RoleAttrStruct _BaseAttr)
    {
        _BaseAttr.SetValue(RoleAttrEnum.Strength, Strength);
        _BaseAttr.SetValue(RoleAttrEnum.Dexterity, Dexterity);
        _BaseAttr.SetValue(RoleAttrEnum.Vitality, Vitality);
        _BaseAttr.SetValue(RoleAttrEnum.Attack, _RoleLevel * 1 + 10);
        _BaseAttr.SetValue(RoleAttrEnum.HPMax, _RoleLevel * 100 + 5000);
        _BaseAttr.SetValue(RoleAttrEnum.Defense, 10);
    }

    public void SetEquipAttr(RoleAttrStruct _BaseAttr)
    {
        foreach (var equipInfo in _EquipList)
        {
            equipInfo.SetEquipAttr(_BaseAttr);
        }
    }

    private void CalculateSecondAttr()
    {
        var strength = _BaseAttr.GetValue(RoleAttrEnum.Strength);
        var baseAttack = _BaseAttr.GetValue(RoleAttrEnum.Attack);
        float attackByStrength = (strength / 1000.0f) * baseAttack + strength * 2;
        _BaseAttr.AddValue(RoleAttrEnum.Attack, (int)attackByStrength);

        var dexteriry = _BaseAttr.GetValue(RoleAttrEnum.Dexterity);
        int criticalRate = (int)((dexteriry / 1000.0f) * 2500);
        int criticalDamage = (int)((dexteriry / 1000.0f) * 10000);
        int attackSpeed = (int)((dexteriry / 1000.0f) * 1000);
        int moveSpeed = (int)((dexteriry / 1000.0f) * 1000);
        int ignoreAttack = (int)(dexteriry * 0.5f);
        _BaseAttr.AddValue(RoleAttrEnum.CriticalHitChance, criticalRate);
        _BaseAttr.AddValue(RoleAttrEnum.CriticalHitDamge, criticalDamage);
        _BaseAttr.AddValue(RoleAttrEnum.AttackSpeed, attackSpeed);
        _BaseAttr.AddValue(RoleAttrEnum.MoveSpeed, moveSpeed);
        _BaseAttr.AddValue(RoleAttrEnum.IgnoreDefenceAttack, ignoreAttack);

        var vitality = _BaseAttr.GetValue(RoleAttrEnum.Vitality);
        int baseHP = _BaseAttr.GetValue(RoleAttrEnum.HPMax);
        int hpByVitality = (int)((vitality / 500.0f) * baseHP);
        int finalDamageReduse = (int)(vitality * 0.1f);
        _BaseAttr.AddValue(RoleAttrEnum.HPMax, hpByVitality);
        _BaseAttr.AddValue(RoleAttrEnum.FinalDamageReduse, finalDamageReduse);
    }

    #endregion

    #region attr Points

    public static int MAX_ROLE_LEVEL = 100;
    public static int POINT_PER_ROLE_LEVEL = 5;
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
    private int _UnDistrubutePoint = 0;
    public int UnDistrubutePoint
    {
        get
        {
            return _UnDistrubutePoint;
        }
    }

    public void AddExp(int value)
    {
        if (_RoleLevel < value)
        {
            _CurExp += value;
            var expRecord = TableReader.RoleExp.GetRecord((_RoleLevel + 1).ToString());
            if (_CurExp >= expRecord.ExpValue)
            {
                _CurExp -= expRecord.ExpValue;
                RoleLevelUp();
            }
        }
        else
        {
            _CurExp += value;
            var expRecord = TableReader.RoleExp.GetRecord((_AttrLevel + MAX_ROLE_LEVEL + 1).ToString());
            if (expRecord == null)
                return;

            if (_CurExp >= expRecord.ExpValue)
            {
                _CurExp -= expRecord.ExpValue;
                AttrLevelUp();
            }
        }

    }

    private void RoleLevelUp()
    {
        ++_RoleLevel;
        _UnDistrubutePoint += 5;

        CalculateAttr();
    }

    private void AttrLevelUp()
    {
        ++_AttrLevel;
        _UnDistrubutePoint += 1;
    }

    public void ResetPoints()
    {
        _AddStrength = 0;
        _AddDexterity = 0;
        _AddVitality = 0;
        _UnDistrubutePoint = _RoleLevel * POINT_PER_ROLE_LEVEL + _AttrLevel * POINT_PER_ATTR_LEVEL;

        CalculateAttr();
    }

    public void DistributePoint(int distriAttr)
    {
        --_UnDistrubutePoint;
        switch (distriAttr)
        {
            case 1:
                ++_AddStrength;
                break;
            case 2:
                ++_AddDexterity;
                break;
            case 3:
                ++_AddVitality;
                break;
        }

        CalculateAttr();
    }

    #endregion

    #region skill

    [SaveField(9)]
    private List<SkillInfoItem> _SkillItems = new List<SkillInfoItem>();

    private Dictionary<string, List<SkillInfoItem>> _SkillClassItems;
    public Dictionary<string, List<SkillInfoItem>> SkillClassItems
    {
        get
        {
            if (_SkillClassItems == null)
            {
                InitSkill();
            }
            return _SkillClassItems;
        }
    }

    private void InitSkill()
    {
        _SkillClassItems = new Dictionary<string, List<SkillInfoItem>>();
        foreach (var skillPair in Tables.TableReader.SkillInfo.Records)
        {
            if (skillPair.Value.Profession == Profession)
            {
                if (!_SkillClassItems.ContainsKey(skillPair.Value.SkillClass))
                {
                    _SkillClassItems.Add(skillPair.Value.SkillClass, new List<SkillInfoItem>());
                }

                if (_SkillClassItems.ContainsKey(skillPair.Value.SkillClass))
                {
                    var skillInfo = GetSkillInfo(skillPair.Value.Id);
                    if (skillInfo == null)
                    {
                        skillInfo = new SkillInfoItem(skillPair.Value.Id);
                        skillInfo._SkillLevel = 0;
                    }
                    _SkillClassItems[skillPair.Value.SkillClass].Add(skillInfo);
                }
            }
        }
    }

    public List<string> GetRoleSkills()
    {
        List<string> skillMotions = new List<string>() { "Attack", "Buff1", "Buff2", "Dush", "AttackEx" };
        if (Profession == PROFESSION.BOY_DEFENCE || Profession == PROFESSION.GIRL_DEFENCE)
        {
            skillMotions.Add("Defence");
        }
        else
        {
            skillMotions.Add("Roll");
        }

        bool spSkill1 = false;
        bool spSkill2 = false;
        bool spSkill3 = false;
        foreach (var skillInfo in _SkillItems)
        {
            if (skillInfo.SkillRecord.Profession != Profession)
                continue;

            if (skillInfo._SkillLevel == 0)
                continue;

            if (skillInfo.SkillRecord.SkillAttr == "RoleAttrImpactSP")
            {
                if (skillInfo.SkillRecord.SkillClass == "1")
                {
                    spSkill1 = true;
                }
                if (skillInfo.SkillRecord.SkillClass == "2")
                {
                    spSkill2 = true;
                }
                if (skillInfo.SkillRecord.SkillClass == "3")
                {
                    spSkill3 = true;
                }
            }
        }

        if (Profession == PROFESSION.GIRL_DOUGE || Profession == PROFESSION.BOY_DEFENCE)
        {
            if (spSkill1)
            {
                skillMotions.Add("Skill1.1.2");
            }
            else
            {
                skillMotions.Add("Skill1");
            }

            if (spSkill2)
            {
                skillMotions.Add("Skill2.1.2");
            }
            else
            {
                skillMotions.Add("Skill2");
            }

            if (spSkill3)
            {
                skillMotions.Add("Skill3.1.2");
            }
            else
            {
                skillMotions.Add("Skill3");
            }
        }
        else
        {
            if (spSkill1)
            {
                skillMotions.Add("Skill1.2.2");
            }
            else
            {
                skillMotions.Add("Skill1.2");
            }

            if (spSkill2)
            {
                skillMotions.Add("Skill2.2.2");
            }
            else
            {
                skillMotions.Add("Skill2.2");
            }

            if (spSkill3)
            {
                skillMotions.Add("Skill3.2.2");
            }
            else
            {
                skillMotions.Add("Skill3.2");
            }
        }


        return skillMotions;
    }

    public SkillInfoItem GetSkillInfo(string skillID)
    {
        var skillItem = _SkillItems.Find((skillInfo) =>
        {
            if (skillInfo._SkillID == skillID)
            {
                return true;
            }
            return false;
        });

        if (skillItem == null)
        {
            skillItem = new SkillInfoItem(skillID);
            skillItem._SkillLevel = 0;
            _SkillItems.Add(skillItem);
        }

        return skillItem;
    }

    public void SkillLevelUp(string skillID)
    {
        //cost

        var findSkill = _SkillItems.Find((skillInfo) =>
        {
            if (skillInfo._SkillID == skillID)
            {
                return true;
            }
            return false;
        });

        if (findSkill == null)
        {
            findSkill = new SkillInfoItem(skillID);
            _SkillItems.Add(findSkill);
        }
        else
        {
            var skillTab = Tables.TableReader.SkillInfo.GetRecord(skillID);
            if (skillTab.MaxLevel > findSkill._SkillLevel)
            {
                ++findSkill._SkillLevel;
            }

        }
    }

    public void SetSkillAttr(RoleAttrStruct _BaseAttr)
    {
        foreach (var skillItem in _SkillItems)
        {
            if (skillItem.SkillActureLevel == 0)
                continue;

            var attrImpact = RoleAttrImpactManager.GetAttrImpact(skillItem);
            if (attrImpact != null)
            {
                _BaseAttr.AddExAttr(attrImpact);
            }
            //switch (skillItem.SkillRecord.SkillAttr)
            //{
            //    case "RoleAttrImpactAccumulate":
            //        RoleAttrImpactAccumulate impactAccumulate = new RoleAttrImpactAccumulate();
            //        impactAccumulate.InitImpact(skillItem.SkillRecord.SkillClass, 0.5f, 0.4f);
            //        _BaseAttr.AddExAttr(impactAccumulate);
            //        break;
            //    case "RoleAttrImpactExAttack":
            //        RoleAttrImpactExAttack attrImpact = new RoleAttrImpactExAttack();
            //        attrImpact.InitImpact(skillItem.SkillRecord.SkillClass, 1, 0.4f);
            //        _BaseAttr.AddExAttr(attrImpact);
            //        break;
            //    case "RoleAttrImpactSkillSpeed":
            //        break;
            //}
        }
    }

    #endregion
}

