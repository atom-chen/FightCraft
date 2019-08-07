using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIMainFun : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIMainFun, UILayer.BaseUI, hash);
    }

    public static void UpdateMoney()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIMainFun>(UIConfig.UIMainFun);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.UpdateMoneyInner();
    }

    public static void RefreshGift()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIMainFun>(UIConfig.UIMainFun);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.RefreshGiftBtns();
    }

    #endregion

    #region 

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        UpdateMoneyInner();
        RefreshGiftBtns();
    }

    #endregion

    #region info

    public UICurrencyItem _GoldItem;
    public UICurrencyItem _DiamondItem;

    private void UpdateMoneyInner()
    {
        _GoldItem.ShowOwnCurrency(MONEYTYPE.GOLD);
        _DiamondItem.ShowOwnCurrency(MONEYTYPE.DIAMOND);
    }

    #endregion

    #region event

    //fight
    public void BtnFight()
    {
        //UIStageSelect.ShowAsyn();
        //LogicManager.Instance.EnterFight("Stage_01_01");
        ActData.Instance.StartDefaultStage();
    }

    public void BtnBagPack()
    {
        UIEquipPack.ShowAsyn();
    }

    public void BtnShop()
    {
        UIShopPack.ShowAsyn();
    }

    public void BtnGem()
    {
        UIGemPack.ShowAsyn();
    }

    public void BtnSoul()
    {
        //UISoulPack.ShowAsyn();
        UISummonSkillPack.ShowAsyn();
    }

    public void BtnTestPanel()
    {
        UITestEquip.ShowAsyn();
    }

    public void BtnSkill()
    {
        UISkillLevelUp.ShowAsyn();
    }

    public void BtnAttr()
    {
        UIRoleAttr.ShowAsyn();
    }

    public void BtnElement()
    {
        UIFiveElement.ShowAsyn();
    }

    public void BtnBossStage()
    {
        UIBossStageSelect.ShowAsyn();
    }

    public void BtnMission()
    {
        UIDailyMission.ShowAsyn();
    }

    public void BtnAchieve()
    {
        UIAchievement.ShowAsyn();
    }

    public void BtnSetting()
    {
        UISystemSetting.ShowAsyn();
    }

    public void BtnBuffT()
    {
        UIGlobalBuff.ShowTelantAsyn();
    }

    public void BtnBuffA()
    {
        UIGlobalBuff.ShowAttrAsyn();
    }

    public void BtnStage()
    {
        if (ActData.Instance._NormalStageIdx == 0)
            return;

        UIStageSelect.ShowAsyn();
    }

    public void BtnAct()
    {
        UIActPanel.ShowAsyn();
    }
    #endregion

    #region gift 

    public GameObject _AdGift;
    public GameObject _PurchGift;

    public void RefreshGiftBtns()
    {
        if (GiftData.Instance._GiftItems != null)
        {
            _AdGift.SetActive(true);
            _PurchGift.SetActive(true);
        }
        else
        {
            _AdGift.SetActive(false);
            _PurchGift.SetActive(false);
        }
    }

    public void OnBtnAdGift()
    {
        UIGiftPack.ShowAsyn();
    }

    public void OnBtnPurchGift()
    {
        UIGiftPack.ShowAsyn();
    }

    #endregion
}

