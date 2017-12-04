
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

 
using UnityEngine.EventSystems;
using System;
using Tables;

 
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
    public Button _BtnPutOn;
    public Button _BtnPutOff;
    public Button _BtnSale;
    public Button _BtnBuy;
    public Button _BtnPutStore;
    public Button _BtnGetStore;

    #endregion

    #region 

    private ItemBase _ShowItem;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);
        ItemBase itemBase = hash["ItemBase"] as ItemBase;
        ToolTipsShowType showType = (ToolTipsShowType)hash["ShowType"];
        ShowTips(itemBase);
        ShowByType(showType);
    }

    private void ShowByType(ToolTipsShowType showType)
    {
        _BtnPanel.SetActive(true);
        _BtnPutOn.gameObject.SetActive(false);
        _BtnPutOff.gameObject.SetActive(false);
        _BtnSale.gameObject.SetActive(false);
        _BtnBuy.gameObject.SetActive(false);
        _BtnPutStore.gameObject.SetActive(false);
        _BtnGetStore.gameObject.SetActive(false);

        switch (showType)
        {
            case ToolTipsShowType.ShowForInfo:
                _BtnPanel.SetActive(false);
                break;
            case ToolTipsShowType.ShowInBackPack:
                _BtnPutOn.gameObject.SetActive(true);
                _BtnSale.gameObject.SetActive(true);
                break;
            case ToolTipsShowType.ShowInEquipPack:
                _BtnPutOff.gameObject.SetActive(true);
                break;
            case ToolTipsShowType.ShowInStoreRight:
                _BtnPutStore.gameObject.SetActive(true);
                break;
            case ToolTipsShowType.ShowInStoreLeft:
                _BtnGetStore.gameObject.SetActive(true);
                break;
            case ToolTipsShowType.ShowInShopRight:
                _BtnSale.gameObject.SetActive(true);
                break;
            case ToolTipsShowType.ShowInShopLeft:
                _BtnBuy.gameObject.SetActive(true);
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
    { }

    public void OnPutStore()
    { }

    public void OnGetStore()
    { }

    #endregion

}

