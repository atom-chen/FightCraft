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

        bool needSave = false;
        needSave |= InitEquipList();
        needSave |= InitSkill();

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
            return true;
        }
        foreach (var itemEquip in _EquipList)
        {
            if (itemEquip.IsVolid())
            {
                itemEquip.CalculateSet();
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

        CalculateSecondAttr(_BaseAttr);
    }

    public void SetRoleLevelAttr(RoleAttrStruct roleAttr)
    {
        roleAttr.SetValue(RoleAttrEnum.Strength, Strength);
        roleAttr.SetValue(RoleAttrEnum.Dexterity, Dexterity);
        roleAttr.SetValue(RoleAttrEnum.Vitality, Vitality);
        roleAttr.SetValue(RoleAttrEnum.Attack, _RoleLevel * 1 + 10);
        roleAttr.SetValue(RoleAttrEnum.HPMax, _RoleLevel * 100 + 5000);
        roleAttr.SetValue(RoleAttrEnum.Defense, 10);
    }

    public void SetEquipAttr(RoleAttrStruct roleAttr)
    {
        foreach (var equipInfo in _EquipList)
        {
            equipInfo.SetEquipAttr(roleAttr);
        }
    }

    public void CalculateSecondAttr(RoleAttrStruct roleAttr)
    {
        var strength = roleAttr.GetValue(RoleAttrEnum.Strength);
        var baseAttack = roleAttr.GetValue(RoleAttrEnum.Attack);
        float attackByStrength = (strength / 1000.0f) * baseAttack + strength * 2;
        roleAttr.AddValue(RoleAttrEnum.Attack, (int)attackByStrength);

        var dexteriry = roleAttr.GetValue(RoleAttrEnum.Dexterity);
        int criticalRate = (int)((dexteriry / 1000.0f) * 2500);
        int criticalDamage = (int)((dexteriry / 1000.0f) * 10000);
        int attackSpeed = (int)((dexteriry / 1000.0f) * 1000);
        int moveSpeed = (int)((dexteriry / 1000.0f) * 1000);
        int ignoreAttack = (int)(dexteriry * 0.5f);
        roleAttr.AddValue(RoleAttrEnum.CriticalHitChance, criticalRate);
        roleAttr.AddValue(RoleAttrEnum.CriticalHitDamge, criticalDamage);
        roleAttr.AddValue(RoleAttrEnum.AttackSpeed, attackSpeed);
        roleAttr.AddValue(RoleAttrEnum.MoveSpeed, moveSpeed);
        roleAttr.AddValue(RoleAttrEnum.IgnoreDefenceAttack, ignoreAttack);

        var vitality = roleAttr.GetValue(RoleAttrEnum.Vitality);
        int baseHP = roleAttr.GetValue(RoleAttrEnum.HPMax);
        int hpByVitality = (int)((vitality / 500.0f) * baseHP);
        int finalDamageReduse = (int)(vitality * 0.1f);
        roleAttr.AddValue(RoleAttrEnum.HPMax, hpByVitality);
        roleAttr.AddValue(RoleAttrEnum.FinalDamageReduse, finalDamageReduse);
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

        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_ROLE_LEVEL_UP, this, null);
    }

    private void AttrLevelUp()
    {
        ++_AttrLevel;
        _UnDistrubutePoint += 1;

        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_ROLE_LEVEL_UP, this, null);
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
    private List<ItemSkill> _SkillItems = new List<ItemSkill>();

    private List<ItemSkill> _ProfessionSkills;
    public List<ItemSkill> ProfessionSkills
    {
        get
        {
            if (_ProfessionSkills == null)
            {
                InitSkill();
            }
            return _ProfessionSkills;
        }
    }

    private bool InitSkill()
    {
        bool isNeedSave = false;
        _ProfessionSkills = new List<ItemSkill>();
        foreach (var skillPair in Tables.TableReader.SkillInfo.Records)
        {
            if (skillPair.Value.Profession > 0 &&
            ((skillPair.Value.Profession >> (int)Profession) & 1) != 0)
            {
                var skillInfo = GetSkillInfo(skillPair.Value.Id, ref isNeedSave);
                _ProfessionSkills.Add(skillInfo);

                if (skillInfo.SkillRecord.SkillAttr == "RoleAttrImpactSkillDamage")
                {
                    if (skillInfo.SkillLevel == 0)
                    {
                        skillInfo.LevelUp();
                    }
                }
            }
        }

        return isNeedSave;
    }

    public List<string> GetRoleSkills()
    {
        List<string> skillMotions = new List<string>() { "Attack", "Dush", "AttackEx" };
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
            if (skillInfo.SkillRecord.Profession > 0 &&
            ((skillInfo.SkillRecord.Profession >> (int)Profession) & 1) == 0)
                continue;

            if (skillInfo.SkillLevel == 0)
                continue;

            if (skillInfo.SkillRecord.SkillInput == "5")
            {
                if (!skillMotions.Contains("Buff1"))
                {
                    skillMotions.Add("Buff1");
                }
            }
            if (skillInfo.SkillRecord.SkillInput == "6")
            {
                if (!skillMotions.Contains("Buff2"))
                {
                    skillMotions.Add("Buff2");
                }
            }

            if (skillInfo.SkillRecord.SkillAttr == "RoleAttrImpactSP")
            {
                if (skillInfo.SkillRecord.SkillInput == "1")
                {
                    spSkill1 = true;
                }
                if (skillInfo.SkillRecord.SkillInput == "2")
                {
                    spSkill2 = true;
                }
                if (skillInfo.SkillRecord.SkillInput == "3")
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

    public ItemSkill GetSkillInfo(string skillID, ref bool isNeedSave)
    {
        var skillItem = _SkillItems.Find((skillInfo) =>
        {
            if (skillInfo.SkillID == skillID)
            {
                return true;
            }
            return false;
        });

        if (skillItem == null)
        {
            skillItem = new ItemSkill(skillID, 0);
            _SkillItems.Add(skillItem);
            isNeedSave |= true;
        }

        return skillItem;
    }

    public void SkillLevelUp(string skillID)
    {
        //cost

        var findSkill = _SkillItems.Find((skillInfo) =>
        {
            if (skillInfo.SkillID == skillID)
            {
                return true;
            }
            return false;
        });

        if (findSkill == null)
        {
            findSkill = new ItemSkill(skillID);
            _SkillItems.Add(findSkill);

            SaveClass(false);
        }

        var skillTab = Tables.TableReader.SkillInfo.GetRecord(skillID);
        if (skillTab.MaxLevel > findSkill.SkillLevel)
        {
            findSkill.LevelUp();
        }

        CalculateAttr();
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

