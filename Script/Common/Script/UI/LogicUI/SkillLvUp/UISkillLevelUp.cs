using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using Tables;

public class UISkillLevelUp : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI("LogicUI/SkillLvUp/UISkillLevelUp", UILayer.PopUI, hash);
    }

    #endregion

    #region 

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        InitSkillClass();
        ShowSkillInfo();
    }

    #endregion

    public UISubScollMenu _SkillClass;
    public UIContainerSelect _SkillInfos;

    private Dictionary<string, List< ItemSkill>> _SkillClasses = new Dictionary<string, List<ItemSkill>>();
    private ItemSkill _SelectedSkill;

    private void InitSkillClass()
    {
        _SkillClasses.Clear();
        _SkillClass.Clear();
        foreach (var skillItem in SkillData.Instance.ProfessionSkills)
        {
            string dicStr = Tables.StrDictionary.GetFormatStr(skillItem.SkillRecord.SkillType);

            if (!_SkillClasses.ContainsKey(dicStr))
            {
                _SkillClasses.Add(dicStr, new List<ItemSkill>());
                _SkillClass.PushMenu(dicStr);
            }

            _SkillClasses[dicStr].Add(skillItem);
        }
        _SkillClass.ShowDefaultFirst();
    }

    public void SelectSkillClass(object selectGO)
    {
        var skillClass = (string)selectGO;
        InitSkillItems(skillClass);
    }

    private void InitSkillItems(string skillClass)
    {
        _SkillInfos.InitSelectContent(_SkillClasses[skillClass], null, SelectSkillItem);
    }

    private void SelectSkillItem(object selectItem)
    {
        var skillInfo = selectItem as ItemSkill;
        if (skillInfo == null)
            return;

        _SelectedSkill = skillInfo;
        ShowSkillInfo();
    }

    #region skill lv up

    public Text _Desc;
    public Text _NeedRoleLv;
    public Text _NeedSkillLv;
    public UICurrencyItem _MoneyCost;
    public Text _MoneyText;
    public Button _LevelUp;

    private void ShowSkillInfo()
    {
        if (_SelectedSkill == null)
        {
            _Desc.text = "";
            _NeedRoleLv.text = "";
            _NeedSkillLv.text = "";
            _MoneyCost.ShowCurrency(MONEYTYPE.GOLD, 0);
            //_LevelUp.interactable = false;
            return;
        }

        var skillTab = Tables.TableReader.SkillInfo.GetRecord(_SelectedSkill.SkillID);
        var impactType = Type.GetType(_SelectedSkill.SkillRecord.SkillAttr);
        if (impactType != null)
        {

            var method = impactType.GetMethod("GetAttrDesc");
            if (method != null)
            {
                string skillDesc = method.Invoke(null, new object[] { new List<int>() { int.Parse(_SelectedSkill.SkillID), _SelectedSkill.SkillLevel } }) as string;
                _Desc.text = skillDesc;
            }
        }
        else
        {
            var skillDesc = StrDictionary.GetFormatStr(skillTab.DescStrDict);
            _Desc.text = skillDesc;
        }

        string roleLvStr = StrDictionary.GetFormatStr(62000);
        int nextLv = skillTab.StartRoleLevel + (_SelectedSkill.SkillActureLevel) * skillTab.NextLvInterval;
        if (RoleData.SelectRole._RoleLevel >= nextLv)
        {
            roleLvStr += StrDictionary.GetFormatStr(1000005, nextLv);
        }
        else
        {
            roleLvStr += StrDictionary.GetFormatStr(1000004, nextLv);
        }
        _NeedRoleLv.text = roleLvStr;

        if (skillTab.StartPreSkill > 0)
        {
            string skillLv = StrDictionary.GetFormatStr(62001);
            var preSkillTab = TableReader.SkillInfo.GetRecord(skillTab.StartPreSkill.ToString());
            string nextSkill = StrDictionary.GetFormatStr(62005, StrDictionary.GetFormatStr(preSkillTab.NameStrDict), skillTab.StartPreSkillLv);
            var skillItem = SkillData.Instance.GetSkillInfo(preSkillTab.Id);
            if (skillItem.SkillActureLevel >= skillTab.StartPreSkillLv)
            {
                skillLv += StrDictionary.GetFormatStr(1000005, nextSkill);
            }
            else
            {
                skillLv += StrDictionary.GetFormatStr(1000004, nextSkill);
            }
            _NeedSkillLv.text = skillLv;

        }
        else
        {
            _NeedSkillLv.text = "";
        }

        if (skillTab.CostStep[0] == (int)MONEYTYPE.GOLD)
        {
            int costValue = GameDataValue.GetSkillLvUpGold(skillTab, _SelectedSkill.SkillLevel);
            _MoneyCost.ShowCurrency(MONEYTYPE.GOLD, costValue);
            if (costValue > PlayerDataPack.Instance.Gold)
            {
                _MoneyText.color = Color.red;
            }
            else
            {
                _MoneyText.color = Color.green;
            }
        }
        else
        {
            int costValue = skillTab.CostStep[1];
            _MoneyCost.ShowCurrency(MONEYTYPE.DIAMOND, costValue);
            if (costValue > PlayerDataPack.Instance.Diamond)
            {
                _MoneyText.color = Color.red;
            }
            else
            {
                _MoneyText.color = Color.green;
            }
        }
    }

    public void OnBtnSkillLvUp()
    {
        if (_SelectedSkill == null)
            return;

        var skillTab = Tables.TableReader.SkillInfo.GetRecord(_SelectedSkill.SkillID);
        int nextLv = skillTab.StartRoleLevel + (_SelectedSkill.SkillActureLevel) * skillTab.NextLvInterval;
        if (RoleData.SelectRole._RoleLevel < nextLv)
        {
            UIMessageTip.ShowMessageTip(62002);
            return;
        }

        if (skillTab.StartPreSkill > 0)
        {
            var preSkillTab = TableReader.SkillInfo.GetRecord(skillTab.StartPreSkill.ToString());
            var skillItem = SkillData.Instance.GetSkillInfo(preSkillTab.Id);
            if (skillItem.SkillActureLevel < skillTab.StartPreSkillLv)
            {
                UIMessageTip.ShowMessageTip(62003);
                return;
            }

        }

        if (skillTab.CostStep[0] == (int)MONEYTYPE.GOLD)
        {
            int costValue = GameDataValue.GetSkillLvUpGold(skillTab, _SelectedSkill.SkillLevel);
            if (!PlayerDataPack.Instance.DecGold(costValue))
                return;
        }
        else
        {
            int costValue = skillTab.CostStep[1];
            if (!PlayerDataPack.Instance.DecDiamond(costValue))
                return;
        }

        SkillData.Instance.SkillLevelUp(_SelectedSkill.SkillID);
        RefreshSkillInfos();
        ShowSkillInfo();
    }

    private void RefreshSkillInfos()
    {
        _SkillInfos.RefreshItems();
    }
    #endregion

}

