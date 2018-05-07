﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tables;
using System;

public class SkillData : SaveItemBase
{
    #region 单例

    private static SkillData _Instance;
    public static SkillData Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new SkillData();
            }
            return _Instance;
        }
    }

    private SkillData()
    {
        _SaveFileName = "SkillData";
    }

    #endregion

    public void InitSkills()
    {
        InitSkill();
    }

    #region skill

    [SaveField(1)]
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
        if (Tables.TableReader.SkillInfo == null)
            return false;

        Debug.Log("_SkillItems:" + _SkillItems.Count);
        bool isNeedSave = false;
        _ProfessionSkills = new List<ItemSkill>();
        foreach (var skillPair in Tables.TableReader.SkillInfo.Records)
        {
            if (skillPair.Value.Profession > 0 &&
            ((skillPair.Value.Profession >> (int)RoleData.SelectRole.Profession) & 1) != 0)
            {
                var skillInfo = GetSkillInfo(skillPair.Value.Id, ref isNeedSave);
                _ProfessionSkills.Add(skillInfo);

                if (skillInfo.SkillRecord.SkillAttr == "RoleAttrImpactSkillDamage")
                {
                    if (skillInfo.SkillLevel == 0)
                    {
                        SkillLevelUp(skillInfo.SkillID);
                        isNeedSave = true;
                    }
                }
            }
        }

        if(isNeedSave)
        {
            SaveClass(true);
        }

        return isNeedSave;
    }

    public List<string> GetRoleSkills()
    {
        List<string> skillMotions = new List<string>() { "Attack", "Dush" };
        if (RoleData.SelectRole.Profession == PROFESSION.BOY_DEFENCE || RoleData.SelectRole.Profession == PROFESSION.GIRL_DEFENCE)
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
            ((skillInfo.SkillRecord.Profession >> (int)RoleData.SelectRole.Profession) & 1) == 0)
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

            if (skillInfo.SkillRecord.SkillAttr == "RoleAttrImpactBuffInHit")
            {
                skillMotions.Add("BuffInHit");
            }
        }

        if (RoleData.SelectRole.Profession == PROFESSION.GIRL_DOUGE || RoleData.SelectRole.Profession == PROFESSION.BOY_DEFENCE)
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

    public ItemSkill GetSkillInfo(string skillID)
    {
        bool needSave = false;
        return GetSkillInfo(skillID, ref needSave);
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

    public ItemSkill GetSkillByInput(string skillInput)
    {
        var skillItem = ProfessionSkills.Find((skillInfo) =>
        {
            if (skillInfo.SkillRecord.SkillInput == skillInput)
            {
                return true;
            }
            return false;
        });
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

        RoleData.SelectRole.CalculateAttr();
    }

    public void SetSkillAttr(RoleAttrStruct _BaseAttr)
    {
        foreach (var skillItem in _SkillItems)
        {
            if (skillItem.SkillActureLevel == 0)
                continue;

            if (skillItem.SkillRecord.Profession > 0 &&
            ((skillItem.SkillRecord.Profession >> (int)RoleData.SelectRole.Profession) & 1) != 0)
            {
                var attrImpact = RoleAttrImpactManager.GetAttrImpact(skillItem);
                if (attrImpact != null)
                {
                    _BaseAttr.AddExAttr(attrImpact);
                }
            }
        }
    }

    #endregion
}



