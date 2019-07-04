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

        _CostOne.ShowCurrency(MONEYTYPE.GOLD, GameDataValue.GetSummonCostGold(SummonSkillData.Instance.SummonLevel));
        _CostTen.ShowCurrency(MONEYTYPE.GOLD, GameDataValue.GetSummonCostGold(SummonSkillData.Instance.SummonLevel) * 10);
        _CostItem.ShowOwnCurrency(SummonSkillData._GoldCostItem);
    }

    private void ShowDiamondPanel()
    {
        _GoldPanel.SetActive(false);
        _DiamondPanel.SetActive(true);

        _CostOne.ShowCurrency(MONEYTYPE.DIAMOND, GameDataValue.GetSummonCostDiamond(SummonSkillData.Instance.SummonLevel));
        _CostTen.ShowCurrency(MONEYTYPE.DIAMOND, GameDataValue.GetSummonCostDiamond(SummonSkillData.Instance.SummonLevel) * 10);
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

    private SummonSkillData.LotteryResult _LotteryResult;

    public void OnBtnBuyOne()
    {
        if (_LotteryResult != null)
            return;

        if (_GoldPanel.activeSelf)
        {
            _LotteryResult = SummonSkillData.Instance.LotteryGold(1);
        }
        else
        {
            _LotteryResult = SummonSkillData.Instance.LotteryDiamond(1);
        }

        if (_LotteryResult != null)
        {
            PlayerSummonAnim(_LotteryResult._SummonData);
        }
    }

    public void OnBtnBuyTen()
    {
        if (_LotteryResult != null)
            return;

        if (_GoldPanel.activeSelf)
        {
            _LotteryResult = SummonSkillData.Instance.LotteryGold(10);
        }
        else
        {
            _LotteryResult = SummonSkillData.Instance.LotteryDiamond(10);
        }

        if (_LotteryResult != null)
        {
            PlayerSummonAnim(_LotteryResult._SummonData);
        }
    }

    public void PlayerSummonAnim(List<SummonMotionData> summonDatas)
    {
        UISummonGotAnim.ShowAsyn(summonDatas, AnimFinish);
        //RefreshItems();
        UISummonSkillPack.RefreshPack();
        RefreshItems();
    }

    public void AnimFinish()
    {
        if (_LotteryResult._ReturnItemNum > 0)
        {
            UISummonLotteryReturn.ShowAsyn(_LotteryResult._ReturnItem, _LotteryResult._ReturnItemNum);
        }
        _LotteryResult = null;
    }

    #endregion

}

