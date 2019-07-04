using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIGiftPack : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        UIGlobalBuff._ShowType = 1;
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI("LogicUI/Gift/UIGiftTipPack", UILayer.BaseUI, hash);
    }

    #endregion

    #region 

    public override void Show(Hashtable hash)
    {
        base.Show(hash);
    }
    
    public void OnBtnAdGift()
    {
        GiftData.Instance.BuyGift(true);
        Hide();
    }

    public void OnBtnPurchGift()
    {
        GiftData.Instance.BuyGift(false);
        Hide();
    }

    #endregion
}

