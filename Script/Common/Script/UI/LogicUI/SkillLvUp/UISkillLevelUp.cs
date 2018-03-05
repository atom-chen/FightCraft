using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

 
 


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
        foreach (var skillItem in RoleData.SelectRole.ProfessionSkills)
        {
            if (!_SkillClasses.ContainsKey(skillItem.SkillRecord.SkillType))
            {
                _SkillClasses.Add(skillItem.SkillRecord.SkillType, new List<ItemSkill>());
                _SkillClass.PushMenu(skillItem.SkillRecord.SkillType);
            }

            _SkillClasses[skillItem.SkillRecord.SkillType].Add(skillItem);
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
    public Button _LevelUp;

    private void ShowSkillInfo()
    {
        if (_SelectedSkill == null)
        {
            _Desc.text = "";
            _LevelUp.interactable = false;
            return;
        }

        var skillTab = Tables.TableReader.SkillInfo.GetRecord(_SelectedSkill.SkillID);
        string skillDesc = skillTab.Desc;
        skillDesc += " +" + skillTab.EffectValue[0] * _SelectedSkill.SkillLevel;
        _Desc.text = skillDesc;

        if (_SelectedSkill.SkillLevel >= skillTab.MaxLevel)
        {
            _LevelUp.interactable = false;
        }
        else
        {
            _LevelUp.interactable = true;
        }
    }

    public void OnBtnSkillLvUp()
    {
        if (_SelectedSkill == null)
            return;

        RoleData.SelectRole.SkillLevelUp(_SelectedSkill.SkillID);
        RefreshSkillInfos();
    }

    private void RefreshSkillInfos()
    {
        _SkillInfos.RefreshItems();
    }
    #endregion

}

