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

    public UIContainerSelect _SkillClass;
    public UIContainerSelect _SkillInfos;

    private SkillInfoItem _SelectedSkill;

    private void InitSkillClass()
    {
        _SkillClass.InitSelectContent(RoleData.SelectRole.SkillClassItems.Keys, new List<Tables.SKILL_CLASS>() { Tables.SKILL_CLASS.NORMAL_ATTACK }, SelectSkillClass);
    }

    private void SelectSkillClass(object selectGO)
    {
        var skillClass = (string)selectGO;
        InitSkillItems(skillClass);
    }

    private void InitSkillItems(string skillClass)
    {
        _SkillInfos.InitSelectContent(RoleData.SelectRole.SkillClassItems[skillClass], null, SelectSkillItem);
    }

    private void SelectSkillItem(object selectItem)
    {
        var skillInfo = selectItem as SkillInfoItem;
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

        var skillTab = Tables.TableReader.SkillInfo.GetRecord(_SelectedSkill._SkillID);
        string skillDesc = skillTab.Desc;
        skillDesc += " +" + skillTab.EffectValue[0] * _SelectedSkill._SkillLevel;
        _Desc.text = skillDesc;

        if (_SelectedSkill._SkillLevel >= skillTab.MaxLevel)
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

        RoleData.SelectRole.SkillLevelUp(_SelectedSkill._SkillID);
        RefreshSkillInfos();
    }

    private void RefreshSkillInfos()
    {
        _SkillInfos.RefreshItems();
    }
    #endregion

}

