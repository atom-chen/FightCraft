using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;

public class UISellShopPack : UIBase
{
    #region static

    public static void ShowSync()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI("LogicUI/BagPack/UISellShopPack", UILayer.SubPopUI, hash);
    }

    public static UISellShopPack GetUIBackPackInstance(Transform parentTrans)
    {
        var tempGO = ResourceManager.Instance.GetUI("LogicUI/BagPack/UISellShopPack");
        if (tempGO != null)
        {
            var uiGO = GameObject.Instantiate(tempGO);

            uiGO.transform.SetParent(parentTrans);
            uiGO.transform.localPosition = Vector3.zero;
            uiGO.transform.localRotation = Quaternion.Euler(Vector3.zero);
            uiGO.transform.localScale = Vector3.one;

            var backPack = uiGO.GetComponent<UISellShopPack>();
            return backPack;
        }
        return null;
    }

    #endregion

    #region 

    public UIBackPack _BackPack;
    public Transform _BackPackPos;
    public UIContainerBase _ItemsContainer;

    #endregion

    #region 

    public override void Init()
    {
        base.Init();

        _BackPack = UIBackPack.GetUIBackPackInstance(_BackPackPos);
        _BackPack.SetBackPackSellMode(Tables.ITEM_QUALITY.BLUE);
        _BackPack._BtnPanel.SetActive(false);
    }

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        _BackPack.Show(null);
        _BackPack.SetBackPackSellMode(Tables.ITEM_QUALITY.BLUE);
        _ItemsContainer.InitContentItem(ShopData.Instance.BuyBackList, ShowSellBackTooltips, hash, null);
    }

    public override void Hide()
    {
        base.Hide();

        UIEquipPack.RefreshBagItems();
    }

    private void ShowSellBackTooltips(object equipObj)
    {
        ItemBase equipItem = equipObj as ItemBase;
        //UIEquipTooltips.ShowShopAsyn(equipItem, true, MONEYTYPE.GOLD,0,  new ToolTipFunc[1] {  });
    }

    public void OnBtnSell()
    {
        var sellList = _BackPack.GetSellList();
        foreach (var sellItem in sellList)
        {
            ShopData.Instance.SellItem(sellItem, false);
        }

        _BackPack.RefreshItems();
        _ItemsContainer.RefreshItems();

        Hide();

        //UIEquipPack.RefreshBagItems();
    }

    #endregion


}

