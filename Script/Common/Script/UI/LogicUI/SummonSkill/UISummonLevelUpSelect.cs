using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UISummonLevelUpSelect : UIBase
{

    #region static funs

    public static void ShowAsyn(SummonMotionData summonMotion)
    {
        Hashtable hash = new Hashtable();
        hash.Add("SummonMotion", summonMotion);
        GameCore.Instance.UIManager.ShowUI("LogicUI/SummonSkill/UISummonLevelUpSelect", UILayer.Sub2PopUI, hash);
    }

    public static void RefreshPack()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UISummonSkillPack>("LogicUI/SummonSkill/UISummonLevelUpSelect");
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.RefreshItems();
    }

    #endregion

    #region pack

    public UIContainerSelect _SummonItemContainer;

    public Text _SelectedExp;

    private SummonMotionData _LevelUpMotion;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        _LevelUpMotion = (SummonMotionData)hash["SummonMotion"];

        ShowItemPack();
    }

    public void RefreshItems()
    {
        ShowItemPack();
    }

    private void ShowItemPack()
    {
        List<SummonMotionData> unusedMotions = new List<SummonMotionData>();
        for (int i = 0; i < SummonSkillData.Instance._SummonMotionList.Count; ++i)
        {
            if (!SummonSkillData.Instance._UsingSummon.Contains(SummonSkillData.Instance._SummonMotionList[i])
                && _LevelUpMotion != SummonSkillData.Instance._SummonMotionList[i])
            {
                unusedMotions.Add(SummonSkillData.Instance._SummonMotionList[i]);
            }
        }
        SummonSkillData.Instance.SortSummonMotionsInExp(unusedMotions, _LevelUpMotion);

        Hashtable hash = new Hashtable();
        hash.Add("ShowSelect", true);
        _SummonItemContainer.InitSelectContent(unusedMotions, null, OnItemSelected, null, hash);
    }

    private void OnItemSelected(object selectItem)
    {
        var selectItems = _SummonItemContainer.GetSelecteds<SummonMotionData>();

        int exp = SummonSkillData.Instance.GetItemsExp(selectItems);
        _SelectedExp.text = exp.ToString();
    }

    #endregion

    #region 

    public void OnBtnOk()
    {
        var selectItems = _SummonItemContainer.GetSelecteds<SummonMotionData>();
        if (selectItems.Count == 0)
            return;

        int exp = SummonSkillData.Instance.LevelUpSummonItem(_LevelUpMotion, selectItems);
        UISummonSkillToolTips.ShowAddExp(_LevelUpMotion, exp);

        Hide();
    }

    public void OnBtnAutoSelect()
    {
        List<SummonMotionData> unusedMotions = new List<SummonMotionData>();
        for (int i = 0; i < SummonSkillData.Instance._SummonMotionList.Count; ++i)
        {
            if (SummonSkillData.Instance._SummonMotionList[i].SummonRecord.Quality == Tables.ITEM_QUALITY.WHITE)
            {
                unusedMotions.Add(SummonSkillData.Instance._SummonMotionList[i]);
            }
        }
        _SummonItemContainer.SetSelect(unusedMotions);
    }

    #endregion
}

