
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

 
using UnityEngine.EventSystems;
using System;
using Tables;

public enum ToolTipsShowType
{
    ShowForInfo = 0,
    ShowInBackPack,
    ShowInEquipPack,
    ShowInStoreRight,
    ShowInStoreLeft,
    ShowInShopRight,
    ShowInShopLeft,
}

public class UIItemTooltips : UIBase
{

    #region static funs

    public static void ShowAsyn(ItemBase itembase, ToolTipsShowType showType = ToolTipsShowType.ShowForInfo)
    {
        Hashtable hash = new Hashtable();
        hash.Add("ItemBase", itembase);
        hash.Add("ShowType", showType);
        GameCore.Instance.UIManager.ShowUI("LogicUI/BagPack/UIItemTooltips", UILayer.MessageUI, hash);
    }

    public static void HideAsyn()
    {
        UIManager.Instance.HideUI("LogicUI/BagPack/UIItemTooltips");
    }

    #endregion

    #region 

    public UIItemInfo _UIItemInfo;

    public GameObject _BtnPanel;

    public Button _BtnUse;
    public Button _BtnSale;
    public Button _BtnBuy;
    public Button _BtnPutStore;
    public Button _BtnGetStore;

    #endregion

    #region 

    protected ItemBase _ShowItem;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);
        _ShowItem = hash["ItemBase"] as ItemBase;
        ToolTipsShowType showType = (ToolTipsShowType)hash["ShowType"];
        ShowTips(_ShowItem);
        ShowByType(showType);
    }

    protected virtual void ShowByType(ToolTipsShowType showType)
    {
        _BtnPanel.SetActive(true);
        SetGOActive(_BtnUse, false);
        SetGOActive(_BtnSale, false);
        SetGOActive(_BtnBuy, false);
        SetGOActive(_BtnPutStore, false);
        SetGOActive(_BtnGetStore, false);

        switch (showType)
        {
            case ToolTipsShowType.ShowForInfo:
                _BtnPanel.SetActive(true);
                break;
            case ToolTipsShowType.ShowInBackPack:
                SetGOActive(_BtnUse, true);
                SetGOActive(_BtnSale, true);
                break;
            case ToolTipsShowType.ShowInStoreRight:
                SetGOActive(_BtnPutStore, true);
                break;
            case ToolTipsShowType.ShowInStoreLeft:
                SetGOActive(_BtnGetStore, true);
                break;
            case ToolTipsShowType.ShowInShopRight:
                SetGOActive(_BtnSale, true);
                break;
            case ToolTipsShowType.ShowInShopLeft:
                SetGOActive(_BtnBuy, true);
                break;
        }
    }

    private void ShowTips(ItemBase itemBase)
    {
        if (itemBase == null)
        {
            _ShowItem = null;
            return;
        }
        _ShowItem = itemBase;

        _UIItemInfo.ShowTips(_ShowItem);
    }

    #endregion

    #region operate



    public void OnUse()
    {

    }

    public void OnSale()
    {
        if (_ShowItem != null)
        {
            _ShowItem.ResetItem();
        }
        Hide();
    }

    public void OnBuy()
    {
        UIShopPack.BuyItemStatic(_ShowItem);
        Hide();
    }

    public void OnPutStore()
    { }

    public void OnGetStore()
    { }

    #endregion

}

