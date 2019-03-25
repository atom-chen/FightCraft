﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UISummonSkillToolTips : UIBase
{

    #region static funs

    public static void ShowAsyn(SummonMotionData summonDatas, bool isLvUp)
    {
        Hashtable hash = new Hashtable();
        hash.Add("SummonData", summonDatas);
        hash.Add("IsLVUp", isLvUp);
        GameCore.Instance.UIManager.ShowUI("LogicUI/SummonSkill/UISummonSkillToolTips", UILayer.SubPopUI, hash);
    }

    public static void ShowAddExp(SummonMotionData summonData, int exp)
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UISummonSkillToolTips>("LogicUI/SummonSkill/UISummonSkillToolTips");
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.UpdateExp(summonData, exp);
    }

    public static void Refresh()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UISummonSkillToolTips>("LogicUI/SummonSkill/UISummonSkillToolTips");
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.ShowItem();
    }

    #endregion

    #region info

    public UISummonSkillItem _SummonItem;
    public Text _SkillDesc;
    public UIContainerBase _AttrContainer;

    public Text _Exp;
    public Slider _ExpProcess;

    public Text _StarExp;

    public GameObject _BtnLevelUp;

    private SummonMotionData _SummonData;
    private bool _IsLvUp;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        _SummonData = (SummonMotionData)hash["SummonData"];
        _IsLvUp = (bool)hash["IsLVUp"];
        ShowItem();
    }

    private void ShowItem()
    {
        _Exp.text = _SummonData.CurLvExp.ToString();

        var tabLevel = Tables.TableReader.SummonSkillAttr.GetRecord(_SummonData.Level.ToString());
        float process = (float)_SummonData.CurLvExp / tabLevel.Cost[0];
        _ExpProcess.value = process;

        _SummonItem.ShowSummonData(_SummonData);

        _StarExp.text = string.Format("({0}/{1})", _SummonData.CurStarExp, _SummonData.CurStarLevelExp());

        if (_IsLvUp)
        {
            _BtnLevelUp.SetActive(true);
        }
        else
        {
            _BtnLevelUp.SetActive(false);
        }

        _SkillDesc.text = Tables.StrDictionary.GetFormatStr(_SummonData.SummonRecord.SkillDesc, _SummonData.SummonRecord.SkillRate[_SummonData.StarLevel] * 100);
        _AttrContainer.InitContentItem(_SummonData.SummonAttrs);

    }

    public void UpdateExp(SummonMotionData summonMotion, int exp)
    {
        if (_SummonData != summonMotion)
            return;

        ShowItem();
    }

    #endregion

    #region 

    public void BtnSell()
    {
        SummonSkillData.Instance.SellSummonItem(_SummonData);
    }

    public void BtnStage()
    {
        UISummonStageUp.ShowAsyn(_SummonData);
    }

    public void BtnLevelUp()
    {
        UISummonLevelUpSelect.ShowAsyn(_SummonData);
    }

    #endregion


}

