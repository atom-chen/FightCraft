using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UISummonSkillLottery : UIBase
{

    #region static funs

    public static void ShowGoldAsyn()
    {
        Hashtable hash = new Hashtable();
        hash.Add("GoldPanel", 1);
        GameCore.Instance.UIManager.ShowUI("LogicUI/SummonSkill/UISummonSkillLottery", UILayer.SubPopUI, hash);
    }

    public static void ShowDiamondAsyn()
    {
        Hashtable hash = new Hashtable();
        hash.Add("DiamondPanel", 1);
        GameCore.Instance.UIManager.ShowUI("LogicUI/SummonSkill/UISummonSkillLottery", UILayer.SubPopUI, hash);
    }

    public static void RefreshPack()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UISummonSkillLottery>("LogicUI/SummonSkill/UISummonSkillLottery");
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.RefreshItems();
    }

    #endregion

    #region 

    public GameObject _GoldPanel;
    public GameObject _DiamondPanel;
    public UICurrencyItem _CostOne;
    public UICurrencyItem _CostTen;
    public UICurrencyItem _CostItem;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        if (hash.ContainsKey("GoldPanel"))
        {
            ShowGoldPanel();
        }
        else if (hash.ContainsKey("DiamondPanel"))
        {
            ShowDiamondPanel();
        }
    }

    private void ShowGoldPanel()
    {
        _GoldPanel.SetActive(true);
        _DiamondPanel.SetActive(false);

        _CostOne.ShowCurrency(MONEYTYPE.GOLD, SummonSkillData._GoldCostOne);
        _CostTen.ShowCurrency(MONEYTYPE.GOLD, SummonSkillData._GoldCostOne * 10);
        _CostItem.ShowOwnCurrency(SummonSkillData._GoldCostItem);
    }

    private void ShowDiamondPanel()
    {
        _GoldPanel.SetActive(false);
        _DiamondPanel.SetActive(true);

        _CostOne.ShowCurrency(MONEYTYPE.DIAMOND, SummonSkillData._DiamondCostOne);
        _CostTen.ShowCurrency(MONEYTYPE.DIAMOND, SummonSkillData._DiamondCostOne * 10);
        _CostItem.ShowOwnCurrency(SummonSkillData._DiamondCostItem);
    }

    public void RefreshItems()
    {
        if (_GoldPanel.activeSelf)
        {
            ShowGoldPanel();
        }
        else
        {
            ShowDiamondPanel();
        }
    }

    #endregion

    #region interface

    public void OnBtnBuyOne()
    {
        List<SummonMotionData> summonData = null;
        if (_GoldPanel.activeSelf)
        {
            summonData = SummonSkillData.Instance.LotteryGold(1);
        }
        else
        {
            summonData = SummonSkillData.Instance.LotteryDiamond(1);
        }

        if (summonData != null)
        {
            PlayerSummonAnim(summonData);
        }
    }

    public void OnBtnBuyTen()
    {
        List<SummonMotionData> summonData = null;
        if (_GoldPanel.activeSelf)
        {
            summonData = SummonSkillData.Instance.LotteryGold(10);
        }
        else
        {
            summonData = SummonSkillData.Instance.LotteryDiamond(10);
        }

        if (summonData != null)
        {
            PlayerSummonAnim(summonData);
        }
    }

    public void PlayerSummonAnim(List<SummonMotionData> summonDatas)
    {
        UISummonGotAnim.ShowAsyn(summonDatas);
        //RefreshItems();
        UISummonSkillPack.RefreshPack();
    }

    #endregion

}

