
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

public class UIEquipTooltips : UIBase
{

    #region static funs

    public static void ShowAsyn(ItemEquip itemEquip, ToolTipsShowType showType = ToolTipsShowType.ShowForInfo)
    {
        Hashtable hash = new Hashtable();
        hash.Add("ItemEquip", itemEquip);
        hash.Add("ShowType", showType);
        GameCore.Instance.UIManager.ShowUI("LogicUI/BagPack/UIEquipTooltips", UILayer.MessageUI, hash);
    }

    public static void HideAsyn()
    {
        UIManager.Instance.HideUI("LogicUI/BagPack/UIEquipTooltips");
    }

    #endregion

    #region 

    public UIEquipInfo _UIEquipInfo;
    public UIEquipInfo _CompareEquipInfo;

    public GameObject _BtnPanel;
    public Button _BtnPutOn;
    public Button _BtnPutOff;
    public Button _BtnSale;
    public Button _BtnBuy;
    public Button _BtnPutStore;
    public Button _BtnGetStore;

    #endregion

    #region 

    private ItemEquip _ShowItem;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);
        ItemEquip itemEquip = hash["ItemEquip"] as ItemEquip;
        ToolTipsShowType showType = (ToolTipsShowType)hash["ShowType"];
        ShowTips(itemEquip);
        ShowCompare();
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

    private void ShowTips(ItemEquip itemEquip)
    {
        if (itemEquip == null)
        {
            _ShowItem = null;
            return;
        }
        _ShowItem = itemEquip;

        _UIEquipInfo.ShowTips(_ShowItem);
    }

    private void ShowCompare()
    {
        HideCompare();
    }

    private void HideCompare()
    {
        _CompareEquipInfo.gameObject.SetActive(false);
    }

    #endregion

    #region operate

    public void OnPutOn()
    {
        RoleData.SelectRole.PutOnEquip(_ShowItem.EquipItemRecord.Slot, _ShowItem);
        Hide();
    }

    public void OnPutOff()
    {
        RoleData.SelectRole.PutOffEquip(_ShowItem.EquipItemRecord.Slot, _ShowItem);
        Hide();
    }

    public void OnSale()
    {
        if (_ShowItem != null)
        {
            _ShowItem.ResetItem();
        }
        
    }

    public void OnBuy()
    { }

    public void OnPutStore()
    { }

    public void OnGetStore()
    { }

    #endregion

}

